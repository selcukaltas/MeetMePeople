using MeetMePeople.Data;
using MeetMePeople.Models;
using MeetMePeople.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MeetMePeople.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        const int PageSize = 10;

        public HomeController(ILogger<HomeController> logger,ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _db = applicationDbContext;
        }
        public IActionResult Index(int page = 1)
        {
            var totalItemsCount = _db.Meetings.Count();
            var pageCount = (int)Math.Ceiling((double)totalItemsCount / PageSize); //double bölmesi yaptığımız için int'a yuvarlıyoruz.

            var loggedIn = User.Identity.IsAuthenticated;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var meetings = _db.Meetings.OrderByDescending(x => x.MeetingTime)
                .Select(x => new MeetingViewModel()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    MeetingTime = x.MeetingTime,
                    Photo = x.Photo,
                    Place = x.Place,
                    IsJoined = loggedIn && x.Participants.Any(p => p.Id == userId)
                }).Skip((page - 1) * PageSize).Take(PageSize).ToList();

            var vm = new HomeViewModel
            {
                Meetings = meetings,
                ItemsCount = meetings.Count,
                TotalItemsCount = totalItemsCount,
                PageCount = pageCount,
                PageSize = PageSize,
                Page = page,
                IsPrevious = page > 1,
                IsNext = page< pageCount
            };
            return View(vm);
        }
        [Authorize]
        public IActionResult MyMeetings()
        {
            var userId = User.Id();
            return View(_db.Meetings.Where(x => x.Participants.Any(y => y.Id == userId)).ToList());
        }
        [HttpPost,Authorize,ValidateAntiForgeryToken]
        public IActionResult JoinMeeting(int meetingId)
        {
            var meeting = _db.Meetings.Include("Participants").FirstOrDefault(x => x.Id == meetingId);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _db.Users.Find(userId);
            if (user==null||meeting==null)
                return Unauthorized();
            if (meeting==null)
              return NotFound();
            string result;
            if (meeting.Participants.Contains(user))
            {
                meeting.Participants.Remove(user);
                result = "unjoined";
            }
            else
            {
                meeting.Participants.Add(user);
                result = "joined";

            }
            _db.SaveChanges();
            return Json(new { result = result });

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
