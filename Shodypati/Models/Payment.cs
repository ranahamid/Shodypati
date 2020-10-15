using System;
using System.ComponentModel.DataAnnotations;

namespace Shodypati.Models
{
    public class Payment
    {
        public int Id { get; set; }

        [Required] public string Name { get; set; }

        public string PaymentType { get; set; }

        public DateTime? CreatedOnUtc { get; set; }

        public DateTime? UpdatedOnUtc { get; set; }

        public bool? Published { get; set; }
    }
}