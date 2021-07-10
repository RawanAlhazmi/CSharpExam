using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Exam.Models;

namespace Exam.Controllers
{
    public class ActivityController : Controller
    {
        private MyContext _DB;
        // here we can "inject" our context service into the constructor
        public ActivityController(MyContext context)
        {
            _DB = context;
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            int? uid = HttpContext.Session.GetInt32("User_Id");
            if(uid == null){
                return RedirectToAction("Index", "Home");
            }
            List<ActivityT> allActivities = _DB.Activities
                .Include(a => a.Coordinator)
                .Include(a => a.Attends)
                .OrderByDescending(a => a.CreatedAt)
                .ToList();

            foreach (var activity in allActivities){
                if(activity.ActivityDate < DateTime.Now){
                // Remove it
                    return Redirect($"delete/{activity.ActivityId}");
                }
            }
            User u = _DB.Users.FirstOrDefault(u => u.UserId == (int)uid);
            ViewBag.User = u;
            return View(allActivities);
        }

        [HttpGet("new")]
        public IActionResult NewActivity()
        {
            int? uid = HttpContext.Session.GetInt32("User_Id");
            User u = _DB.Users.FirstOrDefault(u => u.UserId == (int)uid);
            ViewBag.User = u;
            return View();
        }

        [HttpPost("add")]
        public IActionResult Add(ActivityT A)
        {
            // if (A.ActivityDate < DateTime.Now)
            // {
            //     ModelState.AddModelError("ActivityDate", "Activity Date must be in the Future!");
            // }
            int? uid = HttpContext.Session.GetInt32("User_Id");
            if (ModelState.IsValid)
            {
                A.UserId = (int)uid;
                _DB.Activities.Add(A);
                _DB.SaveChanges();
                return Redirect($"activity/{A.ActivityId}");
            }

            User u = _DB.Users.FirstOrDefault(u => u.UserId == (int)uid);
            ViewBag.User = u;
            return View("NewActivity");
        }

        [HttpGet("activity/{ActivityId}")]
        public IActionResult OneActivity(int ActivityId)
        {
            int? uid = HttpContext.Session.GetInt32("User_Id");
            ActivityT oneActivity = _DB
            .Activities
            .Include(a => a.Coordinator)
            .Include(a => a.Attends)
            .ThenInclude(j => j.user)
            .FirstOrDefault(a => a.ActivityId == ActivityId);

            Console.WriteLine("oneActivity.Coordinator.FirstName");
            Console.WriteLine(oneActivity.Coordinator);

            User u = _DB.Users.FirstOrDefault(u => u.UserId == (int)uid);
            ViewBag.User = u;
            return View(oneActivity);
        }
        [HttpGet("join/{ActivityId}")]
        public IActionResult Join(int ActivityId)
        {
            int? uid = HttpContext.Session.GetInt32("User_Id");
            ActivityT A = _DB.Activities.FirstOrDefault(a => a.ActivityId == ActivityId);
            //     foreach (var date in A.Attends){
            //         foreach (var userdates in date.user.Joins){
            //             if(A.ActivityDate == userdates.Activity.ActivityDate){
            //                 Console.WriteLine("Cannot");
            //                 return RedirectToAction("Dashboard");
            //             }
            //      }
            // }
            Join join = new Join();
            join.UserId = (int)uid;
            join.ActivityId = ActivityId;
            _DB.Joins.Add(join);
            _DB.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("leave/{ActivityId}")]
        public IActionResult Leave(int ActivityId)
        {
            int? uid = HttpContext.Session.GetInt32("User_Id");
            Join leave = _DB.Joins.FirstOrDefault(l => l.ActivityId == ActivityId && l.UserId == (int)uid);
            _DB.Joins.Remove(leave);
            _DB.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("delete/{ActivityId}")]  // movieId has to match the asp-route-movieId
        public IActionResult Delete(int ActivityId)
        {
            int? uid = HttpContext.Session.GetInt32("User_Id");
            if(uid == null){
                return RedirectToAction("Index", "Home");
            }
            ActivityT act = _DB.Activities.FirstOrDefault(a => a.ActivityId == ActivityId);
            _DB.Activities.Remove(act);
            _DB.SaveChanges();
            return RedirectToAction("Dashboard");
        }
    }
}