using MeetMePeople.Data;
using MeetMePeople.Models;
using MeetMePeople.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MeetMePeople.Areas.Admin.Controllers
{
    public class EventsController : AdminBaseController
    {
        public EventsController(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public IActionResult Index()
        {
            return View(_db.Meetings.ToList());
        }

        public IActionResult New()
        {
            return View();  
        }

        [HttpPost]
        public IActionResult New(NewMeetingViewModel vm,[FromServices] IWebHostEnvironment env)
        {

            if (ModelState.IsValid)
            {
                string fileName = null;
                if (vm.PhotoFile!=null && vm.PhotoFile.Length >0)
                {
                    fileName = vm.PhotoFile.GenerateFileName();
                    var savePath = Path.Combine(env.WebRootPath, "img",fileName);
                    vm.PhotoFile.CopyTo(new FileStream(savePath, FileMode.Create));
                }
                var meeting = new Meeting()
                {
                    Title = vm.Title,
                    Description = vm.Description,
                    MeetingTime = vm.MeetingTime,
                    Place = vm.Place,
                    Photo = fileName
                };
                _db.Add(meeting);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int id)
        {
            var meeting = _db.Meetings.Find(id);
            if (meeting==null)
            {
                return NotFound();
            }
            var vm = new EditMeetingViewModel()
            {
                Id = meeting.Id,
                Description = meeting.Description,
                Title = meeting.Title,
                MeetingTime = meeting.MeetingTime,
                ExistingPhotoPath = meeting.Photo,
                Place = meeting.Place
            };

            return View(vm);
        }
        [HttpPost]
        public IActionResult Edit(EditMeetingViewModel vm,[FromServices] IWebHostEnvironment env)
        {
            if (ModelState.IsValid)
            {
                string fileName = null;
                if (vm.PhotoFile != null && vm.PhotoFile.Length > 0)
                {
                    fileName = vm.PhotoFile.GenerateFileName();
                    var savePath = Path.Combine(env.WebRootPath, "img", fileName);
                    vm.PhotoFile.CopyTo(new FileStream(savePath, FileMode.Create));
                }
                var meeting = _db.Meetings.Find(vm.Id);
                meeting.MeetingTime = vm.MeetingTime;
                meeting.Description = vm.Description;
                meeting.Place = vm.Place;
                meeting.Title = vm.Title;
                if (!string.IsNullOrEmpty(fileName))
                {
                    meeting.Photo = fileName;
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
   
}
    
