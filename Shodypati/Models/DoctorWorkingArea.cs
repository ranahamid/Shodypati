using System.ComponentModel.DataAnnotations;

namespace Shodypati.Models
{
    public class DoctorWorkingArea
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Working Area")]
        public string WorkingArea { get; set; }

        [Display(Name = "Description")] public string Description { get; set; }
    }
}