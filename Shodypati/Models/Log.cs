using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shodypati.Models
{
    public class Log
    {
        public int Id { get; set; }

        public string ExceptionMessage { get; set; }

        public string ExceptionStackTrace { get; set; }

        public string ControllerName { get; set; }

        public string IpAddress { get; set; }

        public string Browser { get; set; }

        public string OS { get; set; }

        public Guid? UserId { get; set; }

        public string ActionName { get; set; }

        public DateTime? CreatedOnUtc { get; set; }
    }

}