using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MeetMePeople.Data
{
    public class Meeting
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? MeetingTime { get; set; }
        public string Place { get; set; }
        public string Photo { get; set; }

        public ICollection<ApplicationUser> Participants { get; set; }

    }
}
