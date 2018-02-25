using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Shodypati.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public string PatientName { get; set; }
        [Required]
        public int? AssignDoctorId { get; set; }

        public string AssignDoctorName { get; set; }

        public string Address { get; set; }
        [Required]
        public string PhoneNumber { get; set; }

        public int? AdvanceAmount { get; set; }

        public int? RemainingAmount { get; set; }

        public long? StartTime { get; set; }

        public long? EndTime { get; set; }

        public string AppointmentTime { get; set; }

        public int Serial { get; set; }

        public DateTime? CreatedOnUtc { get; set; }

        public DateTime? UpdatedOnUtc { get; set; }
        
        public bool? IsPastDate { get; set; }
    }


    public class AppoinmentCreate
    {
        public List< Appointment> Appointments { get; set; }

        public Appointment NewAppointment { get; set; }

        public string DoctorId { get; set; }

        public Doctor DoctorDetails { get; set; }

    }

    public class AppointmentSelectList
    {
        public List<SelectListItem> AllDoctors { get; set; }

        public List<SelectListItem> AllWorkingSelectListItems { get; set; }

        [Display(Name = "Doctor Working Type")]
        public string SelectedDoctorWorkingTypeId { get; set; }

        [Required]
        [Display(Name = "Doctor Name")]
        public string SelectedDoctorId { get; set; }

        public string SelectedDoctorName { get; set; }
    }


    public class AppointmentTime
    {    
        public uint AppointmentTimeStart { get; set; }

        public int Serial { get; set; }
    }
}