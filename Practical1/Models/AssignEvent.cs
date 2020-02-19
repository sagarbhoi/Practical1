using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Practical1.Models
{
    public class AssignEvent
    {
        public IEnumerable<User> users { get; set; }
        public IEnumerable<Event> events { get; set; }

        public IEnumerable<Event_User> event_user { get; set; }
        

    }
}