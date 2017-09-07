using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.Edm.Csdl;
using Microsoft.EntityFrameworkCore.Query.Internal;
using WebApi.Models;
using WebApi.Models.Response;
using Microsoft.Extensions.Logging;
using WebApi.Utils;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private readonly UserManager<Account> _userManager;
        private readonly SignInManager<Account> _signInManager;
        private readonly ILogger _logger;
        public AccountController(UserManager<Account> userManager, SignInManager<Account> signInManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [HttpPost, Route("facebookRegister")]
        public async Task<ExternalLoginResponse> FacebookRegister([FromBody] FacebookRegisterModel facebookRegisterModel)
        {
            ExternalLoginResponse externalLoginResponse = new ExternalLoginResponse();
            externalLoginResponse.IsModelValid = ModelState.IsValid;
            if (ModelState.IsValid)
            {
                ExternalLoginInfo info = CustomExternalLoginInfo.FromFacebookLoginModel(facebookRegisterModel);
                Account account = new Account{UserName = facebookRegisterModel.Username, Email = facebookRegisterModel.Email};

                var result = await _userManager.CreateAsync(account);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(account, info);

                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(account, true);
                    }

                }

                externalLoginResponse.CreateResult = result;
            }
            else
            {
                externalLoginResponse.Errors = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));

            }

            return externalLoginResponse;
        }

        [HttpPost, Route("facebookSignIn")]
        public async Task<ExternalLoginResponse> FacebookSignIn([FromBody] FacebookLoginModel facebookLoginModel)
        {
            ExternalLoginResponse externalLoginResponse = new ExternalLoginResponse();
            externalLoginResponse.IsModelValid = ModelState.IsValid;
            if (ModelState.IsValid)
            {
                ExternalLoginInfo info = CustomExternalLoginInfo.FromFacebookLoginModel(facebookLoginModel);

                var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true);
                externalLoginResponse.IsRegistered = result.Succeeded;
                externalLoginResponse.Result = result;
            }
            else
            {
                externalLoginResponse.Errors = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));

            }
            return externalLoginResponse;
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
            ClaimsPrincipal principal = new ClaimsPrincipal();
            ExternalLoginInfo info = new ExternalLoginInfo(principal, "Facebook", "", "");
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