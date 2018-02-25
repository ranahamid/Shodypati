using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shodypati.Models
{
    public class UserStatusInfo
    {
        public string message { get; set; }

        public bool success { get; set; }

        public string PhoneNumber { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string UserId { get; set; }

        //extends
        public bool PatientRole { get; set; }
    }

}