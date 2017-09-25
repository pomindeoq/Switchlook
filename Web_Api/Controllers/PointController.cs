using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApi.Models;
using WebApi.Models.Accounts;
using WebApi.Models.Points;
using WebApi.Models.Response;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Point")]
    public class PointController : Controller
    {

        private WebApiDataContext _context;
        private readonly UserManager<Account> _userManager;

        public PointController(WebApiDataContext context, UserManager<Account> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost, Route("addPoints")]
        private async Task<IResponse> AddPoints([FromBody]AddPointsModel addPointsModel)
        {
            // ONLY FOR EVENT!!!
            AddPointsResponse addPointsResponse = new AddPointsResponse();
            PointsModel points = await _context.Points.SingleOrDefaultAsync(x => x.Account.UserName == addPointsModel.UserName);
            Account account = await _userManager.FindByNameAsync(addPointsModel.UserName);
            if (account == null)
            {
                List<string> errors = new List<string>();
                errors.Add("Username does not exist.");
                addPointsResponse.Errors = errors;
            }
            else
            {
                if (points == null)
                {

                    _context.Points.Add(new PointsModel { Account = account, Value = 0 });
                    points = await _context.Points.SingleOrDefaultAsync(x => x.Account.UserName == addPointsModel.UserName);
                    await _context.SaveChangesAsync();
                }

                points.Value = points.Value + addPointsModel.Value;

                _context.Points.Update(points);
                await _context.SaveChangesAsync();
                addPointsResponse.Succeeded = true;
            }


            return addPointsResponse;
        }
    }
}