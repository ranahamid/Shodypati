using System.ComponentModel.DataAnnotations;

namespace Shodypati.Models
{
    public class GeoZone
    {
        public int Id { get; set; }

        [Required] public string Name { get; set; }

        public string Description { get; set; }

        public string DateAdded { get; set; }

        public string DateModified { get; set; }
    }
}