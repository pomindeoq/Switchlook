using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.Edm.Csdl;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using WebApi.Models;
using WebApi.Models.Accounts;
using WebApi.Models.Response;
using WebApi.Utils;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private readonly UserManager<Account> _userManager;
        private readonly SignInManager<Account> _signInManager;
        private readonly WebApiDataContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        public AccountController(WebApiDataContext context, 
            UserManager<Account> userManager, 
            SignInManager<Account> signInManager, 
            ILogger<AccountController> logger, 
            RoleManager<IdentityRole> roleManager ,
            IConfiguration config)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _config = config;
        }

        [HttpPost, Route("facebookRegister")]
        public async Task<IResponse> FacebookRegister([FromBody] FacebookRegisterModel facebookRegisterModel)
        {
            ExternalLoginResponse externalLoginResponse = new ExternalLoginResponse();
            externalLoginResponse.IsModelValid = ModelState.IsValid;
            if (ModelState.IsValid)
            {
                FacebookDataModel facebookData = await FacebookAPI.GetUserLoginData(facebookRegisterModel.AccessToken);

                ExternalLoginInfo info = CustomExternalLoginInfo.FromLoginModel("Facebook", facebookData);
                Account account = new Account{UserName = facebookRegisterModel.Username, Email = facebookData.Email};

                var result = await _userManager.CreateAsync(account);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(account, info);
                    account.SetUpPoints(_context);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(account, true);
                    }

                }
                externalLoginResponse.CreateResult = result;
                externalLoginResponse.Errors = result.Errors.Select(x => x.Description);
            }
            else
            {
                externalLoginResponse.Errors = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));

            }

            return externalLoginResponse;
        }

        [HttpPost, Route("facebookSignIn")]
        public async Task<IResponse> FacebookSignIn([FromBody] FacebookLoginModel facebookLoginModel)
        {
            ExternalLoginResponse externalLoginResponse = new ExternalLoginResponse();
            externalLoginResponse.IsModelValid = ModelState.IsValid;
            if (ModelState.IsValid)
            {
                FacebookDataModel facebookData = await FacebookAPI.GetUserLoginData(facebookLoginModel.AccessToken);
                ExternalLoginInfo info = CustomExternalLoginInfo.FromLoginModel("Facebook", facebookData);

                var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in");
                }
                externalLoginResponse.IsRegistered = result.Succeeded;
                externalLoginResponse.Result = result;
            }
            else
            {
                externalLoginResponse.Errors = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));

            }
            return externalLoginResponse;
        }

        [HttpPost, Route("googleRegister")]
        public async Task<IResponse> GoogleRegister([FromBody] GoogleRegisterModel googleRegisterModel)
        {
            ExternalLoginResponse externalLoginResponse = new ExternalLoginResponse();
            externalLoginResponse.IsModelValid = ModelState.IsValid;
            if (ModelState.IsValid)
            {
                GoogleDataModel googleData = await GoogleAPI.GetUserLoginData(googleRegisterModel.AccessToken);

                ExternalLoginInfo info = CustomExternalLoginInfo.FromLoginModel("Google", googleData);
                Account account = new Account { UserName = googleRegisterModel.Username, Email = googleData.Email };

                var result = await _userManager.CreateAsync(account);
                if (result.Succeeded)
                {
                    
                    result = await _userManager.AddLoginAsync(account, info);
                    account.SetUpPoints(_context);
                    await _context.SaveChangesAsync();

                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(account, true);
                    }

                }

                externalLoginResponse.CreateResult = result;
                externalLoginResponse.Errors = result.Errors.Select(x => x.Description);
            }
            else
            {
                externalLoginResponse.Errors = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));

            }

            return externalLoginResponse;
        }

        [HttpPost, Route("googleSignIn")]
        public async Task<IResponse> GoogleSignIn([FromBody] GoogleLoginModel googleLoginModel)
        {
            ExternalLoginResponse externalLoginResponse = new ExternalLoginResponse();
            externalLoginResponse.IsModelValid = ModelState.IsValid;
            if (ModelState.IsValid)
            {
                GoogleDataModel googleData = await GoogleAPI.GetUserLoginData(googleLoginModel.AccessToken);
                ExternalLoginInfo info = CustomExternalLoginInfo.FromLoginModel("Google", googleData);

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
        public async Task<IResponse> Login([FromBody]LoginModel model)
        { 
            LoginResponse loginResponse = new LoginResponse();
            loginResponse.IsModelValid = ModelState.IsValid;
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, true, false);
                _logger.LogInformation("User logged in");
                loginResponse.Result = result;

                HttpContext.Response.Cookies.Append("TestCookieName", "ValueForTestCookie");
            }
            else
            {
                loginResponse.Errors = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
            }
            return loginResponse;
        }

        [HttpPost, Route("register")]
        public async Task<IResponse> Register([FromBody]RegisterModel model)
        {
            RegisterResponse registerResponse = new RegisterResponse();
            registerResponse.IsModelValid = ModelState.IsValid;
            if (ModelState.IsValid)
            {
                var user = new Account { UserName = model.Username, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    user.SetUpPoints(_context);
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

        [Authorize(AuthenticationSchemes = "Identity.Application, Bearer")]
        [HttpGet, Route("Restricted")]
        public string Restricted()
        {
            if (User.Identity.IsAuthenticated)
            {
                return "IsAuthenticated";
            }
            return "false. not Authenticated";
        }
        
        [Authorize]
        [HttpGet, Route("isAuthenticated")]
        public bool IsAuthenticated()
        {
            if (User.Identity.IsAuthenticated)
            {
                return true;
            }
            return false;
        }

        [AllowAnonymous]
        [HttpPost, Route("token")]
        public async Task<IActionResult> GenerateToken([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);

                var userclaims = await _userManager.GetClaimsAsync(user);

                if (user != null)
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                    if (result.Succeeded)
                    {

                        var claims = new[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id, ClaimValueTypes.String),
                            new Claim(ClaimTypes.Name, user.UserName, ClaimValueTypes.String),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                            _config["Tokens:Issuer"],
                            claims,
                            expires: DateTime.Now.AddMinutes(30),
                            signingCredentials: creds);

                        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
                    }
                }
            }

            return BadRequest("Could not create token");
        }

    }
}