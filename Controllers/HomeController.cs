using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Exam.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace Exam.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _DB;

        // here we can "inject" our context service into the constructor
        public HomeController(MyContext context)
        {
            _DB = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            if(ModelState.IsValid){
                // --------------- Email Validation 
                if (_DB.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
                // --------------- Generate Hasher
                PasswordHasher<User> HashPass = new PasswordHasher<User>();
                // --------------- Hash Password
                user.Password = HashPass.HashPassword(user, user.Password);
                // --------------- Add User To DB
                _DB.Users.Add(user);
                // --------------- Save changes on DB
                _DB.SaveChanges();
                // --------------- Keep User id on session 
                HttpContext.Session.SetInt32("User_Id", user.UserId);
                int? uid = HttpContext.Session.GetInt32("User_Id");
                return RedirectToAction("Dashboard", "Activity");
            }
            return View("Index");
        }

        [HttpPost("signin")]
        public IActionResult Signin(Login user)
        {
            if(ModelState.IsValid){
                // --------------- user From DB by Email
                var userFromDB = _DB.Users.FirstOrDefault(u => u.Email == user.LogEmail);
                // --------------- If no user show Error then redirect to index
                if (userFromDB == null)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
                    return View("Index");
                }
                // --------------- Generate Hasher 
                var hasher = new PasswordHasher<Login>();
                // --------------- Compare the passwords (DB,FormInput)
                var confirm = hasher.VerifyHashedPassword(user, userFromDB.Password, user.LogPassword);
                // --------------- If it return 0 (fail) show Error then redirect to index
                if (confirm == 0)
                {
                    ModelState.AddModelError("LoginPassword", "Invalid Email/Password");
                    return View("Index");
                }
                // --------------- Keep User id on session 
                HttpContext.Session.SetInt32("User_Id", userFromDB.UserId);
                return RedirectToAction("Dashboard", "Activity");
            }
            return View("Index");
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
