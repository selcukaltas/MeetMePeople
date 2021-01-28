﻿using MeetMePeople.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetMePeople.ViewComponents
{
    public class UpcomingEventViewComponent :ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public UpcomingEventViewComponent(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var meeting = await _db.Meetings.Where(x => x.MeetingTime > DateTime.Now).OrderBy(x => x.MeetingTime).FirstOrDefaultAsync();
            return View(meeting);
        }
    }
}
