using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApi.Models;

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
        public async Task<string> Login([FromBody]LoginModel loginModel)
        {
            string response = "";
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginModel.Username, loginModel.Password, true, false);
                if (result.Succeeded)
                {
                    response = "Done";

                }
                else
                {
                    response = "Failed";
                }
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                    .Where(y => y.Count > 0)
                    .ToList();
                
            }
            return response;
        }

        [HttpPost, Route("register")]
        public async Task<string> Register([FromBody]RegisterModel registerModel)
        {
            string response = "";
            if (ModelState.IsValid)
            {
                var user = new Account { UserName = registerModel.Username, Email = registerModel.Email };
                var result = await _userManager.CreateAsync(user, registerModel.Password);

               

                if (result.Succeeded)
                {
                    response = "Done";
                }
                else
                {
                    response = "Failed";
                }
            } 
            else
            {
                response = "Model not valid";
                
            }
            return response;
        }

        [Authorize]
        [HttpGet, Route("SignOut")]
        public async Task<string> SignOut()
        {
            await _signInManager.SignOutAsync();
            return "Signed out";
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