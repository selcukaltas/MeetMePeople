using MeetMePeople.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
    }
}
