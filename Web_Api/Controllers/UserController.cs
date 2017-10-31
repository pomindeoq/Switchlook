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

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
        private WebApiDataContext _context;

        public UserController(WebApiDataContext context)
        {
            _context = context;           
        }

        // GET: User
        [AllowAnonymous]
        [HttpGet, Route("getUsers")]
        public async Task<IResponse> GetUsers()
        {
            UsersResponse usersResponse = new UsersResponse();

            var users = await _context.Users.ToListAsync();

            IEnumerable<IUserResponseModel> usersReturn = users.Select(x => new UserResponseModel
            {
                UserName = x.UserName,
                UserEmail = x.Email
                         
            });

            usersResponse.Users = usersReturn;

            return usersResponse;
            
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

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