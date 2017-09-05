﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApi.Models;
using WebApi.Models.Response;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private readonly UserManager<Account> _userManager;
        private readonly SignInManager<Account> _signInManager;

        public AccountController(UserManager<Account> userManager, SignInManager<Account> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost, Route("login")]
        public async Task<LoginResponse> Login([FromBody]LoginModel loginModel)
        {
            LoginResponse loginResponse = new LoginResponse();
            loginResponse.IsModelValid = ModelState.IsValid;
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginModel.Username, loginModel.Password, true, false);
                loginResponse.Result = result;
            }
            else
            {
                loginResponse.Errors = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
                
            }
            return loginResponse;
        }

        [HttpPost, Route("register")]
        public async Task<RegisterResponse> Register([FromBody]RegisterModel registerModel)
        {
            RegisterResponse registerResponse = new RegisterResponse();
            registerResponse.IsModelValid = ModelState.IsValid;
            if (ModelState.IsValid)
            {
                var user = new Account { UserName = registerModel.Username, Email = registerModel.Email };
                var result = await _userManager.CreateAsync(user, registerModel.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, true);
                }
                registerResponse.Succeeded = result.Succeeded;
                registerResponse.Errors = result.Errors.Select(v => v.Description);
            } 
            else
            {
                registerResponse.Errors = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));

            }
            return registerResponse;
        }

        [Authorize]
        [HttpGet, Route("signOut")]
        public async void SignOut()
        {
            await _signInManager.SignOutAsync();
        }

        [Authorize]
        [HttpGet, Route("Restricted")]
        public string Restricted()
        {
            if (User.Identity.IsAuthenticated)
            {
                return "IsAuthenticated";
            }
            return "false. not Authenticated";
        }

        [HttpGet, Route("isAuthenticated")]
        public bool IsAuthenticated()
        {
            if (User.Identity.IsAuthenticated)
            {
                return true;
            }
            return false;
        }

    }
}