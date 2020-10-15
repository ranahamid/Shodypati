using System.ComponentModel.DataAnnotations;

namespace Shodypati.Models
{
    public class Shipment
    {
        public int Id { get; set; }

        [Required] public string Name { get; set; }

        public int OrderId { get; set; }

        public string TrackingNumber { get; set; }

        public string CreatedOnUtc { get; set; }

        public string ShippedDateUtc { get; set; }

        public string DeliveryDateUtc { get; set; }

        public string AdminComment { get; set; }
    }
}