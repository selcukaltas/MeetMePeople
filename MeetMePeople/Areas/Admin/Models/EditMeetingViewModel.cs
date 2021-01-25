using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MeetMePeople.Models
{
    public class NewMeetingViewModel
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? MeetingTime { get; set; }
        public string Place { get; set; }
        public IFormFile PhotoFile { get; set; }
    }
}
