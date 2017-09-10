﻿using System;
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
        public static ExternalLoginInfo FromLoginModel(IExternalLoginModel loginModel)
        {
            string issuer = "Facebook";
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, loginModel.Id, ClaimValueTypes.String, issuer),
                new Claim(ClaimTypes.Email, loginModel.Email, ClaimValueTypes.String, issuer),
                new Claim(ClaimTypes.Name, loginModel.Name, ClaimValueTypes.String, issuer),
                new Claim(ClaimTypes.GivenName, loginModel.FirstName, ClaimValueTypes.String, issuer),
                new Claim(ClaimTypes.Surname, loginModel.LastName, ClaimValueTypes.String, issuer)
            };
            var identity = new ClaimsIdentity(claims, "Facebook", ClaimTypes.Name, ClaimTypes.Role);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            ExternalLoginInfo info = new ExternalLoginInfo(
                claimsPrincipal,
                "Facebook",
                loginModel.Id,
                loginModel.Name
            );

            return info;
        }
    }
}
