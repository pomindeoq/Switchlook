using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Models.User;
using Microsoft.EntityFrameworkCore;
using WebApi.Models.Points;
using WebApi.Models.Response;
using Microsoft.AspNetCore.Authorization;
using WebApi.Models.Accounts;
using Microsoft.AspNetCore.Identity;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
        private WebApiDataContext _context;
        private readonly UserManager<Account> _userManager;

        public UserController(WebApiDataContext context, UserManager<Account> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: User
        [AllowAnonymous]
        [HttpGet, Route("getUsers")]
        public async Task<IResponse> GetUsers()
        {
            UsersResponse usersResponse = new UsersResponse();
                   

            var users = await _context.Users.ToListAsync();

            var usersReturn = users.Select( x =>  SelectUserData(x));

            //var things = await Task.WhenAll(usersReturn);

            usersResponse.Users = usersReturn;

            return usersResponse;
            
        }

        private IUserResponseModel SelectUserData(Account account)
        {
            UserResponseModel userResponseModel = new UserResponseModel();

            userResponseModel.UserId = account.Id;
            userResponseModel.UserEmail = account.Email;
            userResponseModel.UserName = account.UserName;

            PointsModel pointsModel = _context.Points.FirstOrDefault(x => x.Account.Id == account.Id);
            userResponseModel.UserPoints = pointsModel != null ? pointsModel.Value : 0.0;

            return userResponseModel;
        }

        [AllowAnonymous]
        [HttpGet, Route("getUser/id={id}")]
        public async Task<IResponse> GetUser(string id)
        {
            UserResponse userResponse = new UserResponse();
            Account account = await _context.Users.SingleOrDefaultAsync(x => x.Id == id);

            if (account == null)
            {
                userResponse.User = null;
            }
            else
            {
                IUserResponseModel userReturn = new UserResponseModel()
                {
                    UserId = account.Id,
                    UserName = account.UserName,
                    UserEmail = account.Email,
                };

                userResponse.User = userReturn;
            }
            return userResponse;
        }

        //[AllowAnonymous]
        //[HttpPost, Route("updateUser/id={id}")]
        //public async Task UpdateUser([FromBody] UpdateUserModel updateUserModel)
        //{
        //    User user = new User();
        //    user.Name = await _userManager.FindByIdAsync(updateUserModel.UserName);
        //    user.Email = await _userManager.FindByEmailAsync(updateUserModel.UserEmail);
        //    user.Points = updateUserModel.UserPoints;
        //    _context.Users.Add(user);
        //    await _context.SaveChangesAsync();
        //}

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(GetUsers));
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(GetUsers));
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(GetUsers));
            }
            catch
            {
                return View();
            }
        }
    }
}