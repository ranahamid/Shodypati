using System;
using System.ComponentModel.DataAnnotations;

namespace Shodypati.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required] public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Company { get; set; }

        public int NoOfVisits { get; set; }

        public DateTime? LastLoginDateUtc { get; set; }

        public string LastLoginIP { get; set; }

        public int FailedLoginAttempts { get; set; }

        public DateTime? CannotLoginUntilDateUtc { get; set; }

        public DateTime? CreatedOnUtc { get; set; }

        public DateTime? UpdatedOnUtc { get; set; }

        public bool? Active { get; set; }
    }
}