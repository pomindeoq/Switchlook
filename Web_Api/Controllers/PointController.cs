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
using Microsoft.WindowsAzure.Storage.Blob.Protocol;
using WebApi.Mangers;
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

        private readonly WebApiDataContext _context;
        private readonly UserManager<Account> _userManager;
        private readonly PointManager _pointManager;

        public PointController(WebApiDataContext context, UserManager<Account> userManager, PointManager pointManager)
        {
            _context = context;
            _userManager = userManager;
            _pointManager = pointManager;
        }

        [AllowAnonymous]
        [HttpGet, Route("getPointsValue={userId}")]
        public async Task<IResponse> GetPointsValue(string userId)
        {
            GetPointsValueResponse getPointsValueResponse = new GetPointsValueResponse();
            
            Account account = await _userManager.FindByIdAsync(userId);
            if (account == null)
            {
                List<string> errors = new List<string>();
                errors.Add("UserId does not exist.");
                getPointsValueResponse.Errors = errors;
            }
            else
            {
                PointsModel points = await _context.Points.SingleOrDefaultAsync(x => x.Account.Id == userId);
                getPointsValueResponse.Points = points.Value;
            }
            return getPointsValueResponse;
        }

        [AllowAnonymous]
        [HttpPost, Route("addPoints")]
        public async Task<IResponse> AddPoints([FromBody]AddPointsModel addPointsModel)
        {
            AddPointsResponse addPointsResponse = new AddPointsResponse();
            PointsModel points = await _context.Points.SingleOrDefaultAsync(x => x.Account.Id == addPointsModel.UserId);
            Account account = await _userManager.FindByIdAsync(addPointsModel.UserId);
            if (account == null)
            {
                List<string> errors = new List<string>();
                errors.Add("User does not exist.");
                addPointsResponse.Errors = errors;
            }
            else
            {
                if (points == null)
                {

                    _context.Points.Add(new PointsModel { Account = account, Value = 0 });
                    await _context.SaveChangesAsync();
                    points = await _context.Points.SingleOrDefaultAsync(x => x.Account.Id == addPointsModel.UserId);
                   
                }

                await _pointManager.AddToUserAsync(account, addPointsModel.Value);

                points.Value = points.Value + addPointsModel.Value;

                _context.Points.Update(points);
                await _context.SaveChangesAsync();
                addPointsResponse.Succeeded = true;
            }


            return addPointsResponse;
        }

        [AllowAnonymous]
        [HttpPost, Route("removePoints")]
        public async Task<IResponse> RemovePoints([FromBody]AddPointsModel addPointsModel)
        {
            AddPointsResponse addPointsResponse = new AddPointsResponse();
            PointsModel points = await _context.Points.SingleOrDefaultAsync(x => x.Account.Id == addPointsModel.UserId);
            Account account = await _userManager.FindByIdAsync(addPointsModel.UserId);
            List<string> errors = new List<string>();
            if (account == null)
            {
                
                errors.Add("User does not exist.");
                addPointsResponse.Errors = errors;
            }
            else
            {
                if (points == null)
                {

                    _context.Points.Add(new PointsModel { Account = account, Value = 0 });
                    await _context.SaveChangesAsync();
                    points = await _context.Points.SingleOrDefaultAsync(x => x.Account.Id == addPointsModel.UserId);

                }

                if (points.Value >= addPointsModel.Value && !points.Value.Equals(0.0))
                {
                    points.Value = points.Value - addPointsModel.Value;

                    _context.Points.Update(points);
                    await _context.SaveChangesAsync();

                    var result = await _pointManager.RemoveFromUserAsync(account, addPointsModel.Value);

                    if (result.succeeded)
                    {
                        addPointsResponse.Succeeded = true;
                    }
                    else
                    {
                        errors.Add("Action failed.");
                    }
                }
                else
                {
                    errors.Add("User doesn't have enough points");
                }
                
            }
            if (errors.Count > 0)
            {
                addPointsResponse.Errors = errors;
            }
            

            return addPointsResponse;
        }

        //[AllowAnonymous]
        //[HttpGet, Route("halfPoints")]
        //public async Task HalfPoints()
        //{
        //    var points = await _context.Points.Include(x => x.Account).ToListAsync();
        //    int count = points.Count;
        //    for (int i = 0; i < count; i++)
        //    {
        //        if (points[i].Value > 0.0)
        //        {
        //            points[i].Value = points[i].Value / 2;
        //            _context.Points.Update(points[i]);
        //        }               
        //    }
        //    await _context.SaveChangesAsync();
        //}
    }
}