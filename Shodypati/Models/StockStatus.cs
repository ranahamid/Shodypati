using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shodypati.Models
{
    public class StockStatus
    {
        public int Id { get; set; }
        [Required]
        public string NameEnglish { get; set; }

        public string NameBangla { get; set; }

        public string DescriptionEnglish { get; set; }

        public string DescriptionBangla { get; set; }

    }

}