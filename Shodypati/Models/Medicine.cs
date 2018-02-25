using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Shodypati.Models
{

    public class MedicineList
    {
        public string MedicineName{ get; set; }

        public string Quantity { get; set; }

        public int? Price { get; set; }

        public DateTime? FinishTime { get; set; }
    }
    public class UserIds
    {
        public Guid UserId { get; set; }
    }
    public class Medicine
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public List<string> MedicineName { get; set; }

        public List<string> Quantity { get; set; }

        public List<int> Price { get; set; }

        public List<DateTime> FinishTime { get; set; }

        public List<MedicineList> medicineList { get; set; }
    }
}