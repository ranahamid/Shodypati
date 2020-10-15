using System;
using System.ComponentModel.DataAnnotations;

namespace Shodypati.Models
{
    public class CustomerSearch
    {
        public int Id { get; set; }

        [Required] public int UserId { get; set; }

        public string Keyword { get; set; }

        public string IpAddress { get; set; }

        public DateTime? CreatedOnUtc { get; set; }
    }
}