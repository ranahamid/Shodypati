using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Shodypati.DAL;

namespace Shodypati.Models
{
    public class DoctorViewModel : AppointmentSelectList
    {
        public ShodypatiDataContext Db;

        public DoctorViewModel()
        {
            Db = new ShodypatiDataContext();
        }

        public string SelectedWorkingTypeId { get; set; }

        // public string SelectedDoctorId { get; set; }

        public IEnumerable<SelectListItem> DoctorslList
        {
            get
            {
                IQueryable<DoctorTbl> docts;

                if (SelectedWorkingTypeId == null)
                    docts = from x in Db.DoctorTbls
                        select x;
                else
                    docts = from x in Db.DoctorTbls
                        where x.WorkingArea == SelectedWorkingTypeId
                        select x;


                var teachers = new List<SelectListItem>();


                foreach (var item in docts)
                    teachers.Add(new SelectListItem
                    {
                        Text = item.FullName,
                        Value = item.Id.ToString()
                    });

                return teachers;
            }
        }
    }
}