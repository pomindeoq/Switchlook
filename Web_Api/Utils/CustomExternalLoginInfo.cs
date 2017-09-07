using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebApi.Models;

namespace WebApi.Utils
{
    public class CustomExternalLoginInfo
    {
        public static ExternalLoginInfo FromFacebookLoginModel(IFacebookLoginModel facebookLoginModel)
        {
            string issuer = "Facebook";
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, facebookLoginModel.Id, ClaimValueTypes.String, issuer),
                new Claim(ClaimTypes.Email, facebookLoginModel.Email, ClaimValueTypes.String, issuer),
                new Claim(ClaimTypes.Name, facebookLoginModel.Name, ClaimValueTypes.String, issuer),
                new Claim(ClaimTypes.GivenName, facebookLoginModel.FirstName, ClaimValueTypes.String, issuer),
                new Claim(ClaimTypes.Surname, facebookLoginModel.LastName, ClaimValueTypes.String, issuer)
            };
            var identity = new ClaimsIdentity(claims, "Facebook", ClaimTypes.Name, ClaimTypes.Role);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            ExternalLoginInfo info = new ExternalLoginInfo(
                claimsPrincipal,
                "Facebook",
                facebookLoginModel.Id,
                facebookLoginModel.Name
            );

            return info;
        }
    }
}
