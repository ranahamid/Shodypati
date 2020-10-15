using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Shodypati.Controllers;
using Shodypati.Filters;
using Shodypati.Models;

namespace Shodypati.DAL
{
    [ExceptionHandler]
    public class AppointmentDataAccessRepository : BaseController, IAppointmentAccessRepository<Appointment, int>
    {
        public AppointmentDataAccessRepository()
        {
            Db = new ShodypatiDataContext();
        }

        public IEnumerable<Appointment> Get()
        {
            var entities = Db.DoctorAppointmentTbls.Select(x => new Appointment
            {
                Id = x.Id,
                PatientName = x.PatientName,
                Address = x.Address,
                PhoneNumber = x.PhoneNumber,
                AssignDoctorId = x.AssignDoctorId,
                AssignDoctorName = x.AssignDoctorName,
                AdvanceAmount = x.AdvanceAmount,
                RemainingAmount = x.RemainingAmount,
                StartTime = long.Parse(x.StartTime),
                EndTime = long.Parse(x.EndTime),
                CreatedOnUtc = x.CreatedOnUtc,
                UpdatedOnUtc = x.UpdatedOnUtc,
                IsPastDate = x.IsPastDate
            }).ToList();

            return entities;
        }

        public IEnumerable<Appointment> GetByDoctor(int selectedDoctorId)
        {
            var entities = Db.DoctorAppointmentTbls.Where(x => x.AssignDoctorId == selectedDoctorId).Select(x =>
                new Appointment
                {
                    Id = x.Id,
                    PatientName = x.PatientName,
                    Address = x.Address,
                    PhoneNumber = x.PhoneNumber,
                    AssignDoctorId = x.AssignDoctorId,
                    AssignDoctorName = x.AssignDoctorName,
                    AdvanceAmount = x.AdvanceAmount,
                    RemainingAmount = x.RemainingAmount,
                    StartTime = long.Parse(x.StartTime),
                    EndTime = long.Parse(x.EndTime),
                    CreatedOnUtc = x.CreatedOnUtc,
                    UpdatedOnUtc = x.UpdatedOnUtc,
                    IsPastDate = x.IsPastDate
                }).ToList();

            return entities;
        }

        public Appointment Get(int id)
        {
            var entity = Db.DoctorAppointmentTbls.Where(x => x.Id == id).Select(x => new Appointment
            {
                Id = x.Id,
                PatientName = x.PatientName,
                Address = x.Address,
                PhoneNumber = x.PhoneNumber,
                AssignDoctorId = x.AssignDoctorId,
                AssignDoctorName = x.AssignDoctorName,
                AdvanceAmount = x.AdvanceAmount,
                RemainingAmount = x.RemainingAmount,
                StartTime = long.Parse(x.StartTime),
                EndTime = long.Parse(x.EndTime),
                CreatedOnUtc = x.CreatedOnUtc,
                UpdatedOnUtc = x.UpdatedOnUtc,
                IsPastDate = x.IsPastDate
            }).SingleOrDefault();

            return entity;
        }


        public Appointment PostWeb(Appointment entity)
            // public void Post(string PatientName, string AssignDoctorId, string Address, string PhoneNumber, string AdvanceAmount, string StartTime, string EndTime)
        {
            var doctorName = string.Empty;

            var patinetVisitTime = DateTime.MinValue;

            var doctorDetails = Db.DoctorTbls.FirstOrDefault(x => x.Id == entity.AssignDoctorId);
            if (doctorDetails != null)
            {
                doctorName = doctorDetails.FullName;
                entity.AssignDoctorName = doctorName;

                var remainAmount = doctorDetails.VisitFee - entity.AdvanceAmount;
                entity.RemainingAmount = remainAmount;

                entity.CreatedOnUtc = DateTime.Now;
                entity.UpdatedOnUtc = DateTime.Now;
            }

            Db.DoctorAppointmentTbls.InsertOnSubmit(new DoctorAppointmentTbl
            {
                PatientName = entity.PatientName,
                Address = entity.Address,
                PhoneNumber = entity.PhoneNumber,
                AssignDoctorId = entity.AssignDoctorId,
                AssignDoctorName = doctorName,
                AdvanceAmount = entity.AdvanceAmount,
                RemainingAmount = entity.RemainingAmount,
                StartTime = entity.StartTime.ToString(),
                EndTime = entity.EndTime.ToString(),
                AppointmentTime = patinetVisitTime.ToString("f"),
                Serial = 0,
                CreatedOnUtc = entity.CreatedOnUtc,
                UpdatedOnUtc = entity.UpdatedOnUtc,
                IsPastDate = entity.IsPastDate
            });

            try
            {
                Db.SubmitChanges();
            }
            catch (Exception)
            {
                throw new Exception("Exception");
            }

            return entity;
        }


        public Appointment Post(Appointment entity)
            // public void Post(string PatientName, string AssignDoctorId, string Address, string PhoneNumber, string AdvanceAmount, string StartTime, string EndTime)
        {
            var doctorName = string.Empty;
            uint resultVisitTime = 0;
            uint slotTime = 0;
            var serial = 0;
            var patinetVisitTime = DateTime.MinValue;

            var doctorDetails = Db.DoctorTbls.FirstOrDefault(x => x.Id == entity.AssignDoctorId);
            if (doctorDetails != null)
            {
                doctorName = doctorDetails.FullName;
                entity.AssignDoctorName = doctorName;

                var remainAmount = doctorDetails.VisitFee - entity.AdvanceAmount;
                entity.RemainingAmount = remainAmount;
                var visitingStart = doctorDetails.VisitTimeStart;
                var visitingEnd = doctorDetails.VisitTimeEnd;

                var visitTimeSlot = doctorDetails.SlotDuration;
                slotTime = (uint) visitTimeSlot * 60;

                entity.CreatedOnUtc = DateTime.Now;
                entity.UpdatedOnUtc = DateTime.Now;

                if (entity.StartTime == 0 || entity.EndTime == 0)
                {
                    //compute
                    var nextDay = DateTime.Today.AddDays(1);
                    var unixTimeNextDay = ToUnixTime(nextDay);

                    //First day of the week: Sunday (with a value of zero)
                    var day = (int) nextDay.DayOfWeek; //3
                    var doctorsVisitingDays = doctorDetails.CanVisitDays; //2,4,6
                    var daysIds = doctorsVisitingDays.Split(',').Select(int.Parse).ToList();

                    int willVisitDay;

                    if (daysIds.Contains(day))
                        //2
                        //2,4,6
                        //4

                        willVisitDay = 1;
                    else
                        willVisitDay = GetVisitDay(day, daysIds);


                    //GetVisitStartTime
                    var appointmentTime = GetVisitTimeAppointment(willVisitDay, visitingStart, visitingEnd,
                        visitTimeSlot, doctorDetails.Id, daysIds, day);
                    if (appointmentTime != null)
                    {
                        resultVisitTime = appointmentTime.AppointmentTimeStart;
                        serial = appointmentTime.Serial;
                        patinetVisitTime = ToDateTime(resultVisitTime);
                        entity.AppointmentTime = patinetVisitTime.ToString("f");
                        //DateTime now = DateTime.Now;
                        //Console.WriteLine(now.ToString("d"));
                        //Console.WriteLine(now.ToString("D"));
                        //Console.WriteLine(now.ToString("f"));

                        entity.Serial = serial;
                    }
                }
            }

            var startTimeDb = (long) resultVisitTime * 1000;
            var endTimeDb = (long) (resultVisitTime + slotTime) * 1000;
            entity.StartTime = startTimeDb;
            entity.EndTime = endTimeDb;

            Db.DoctorAppointmentTbls.InsertOnSubmit(new DoctorAppointmentTbl
            {
                PatientName = entity.PatientName,
                Address = entity.Address,
                PhoneNumber = entity.PhoneNumber,
                AssignDoctorId = entity.AssignDoctorId,
                AssignDoctorName = doctorName,
                AdvanceAmount = entity.AdvanceAmount,
                RemainingAmount = entity.RemainingAmount,
                StartTime = startTimeDb.ToString(),
                EndTime = endTimeDb.ToString(),
                AppointmentTime = patinetVisitTime.ToString("f"),
                Serial = serial,
                CreatedOnUtc = entity.CreatedOnUtc,
                UpdatedOnUtc = entity.UpdatedOnUtc,
                IsPastDate = entity.IsPastDate
            });

            try
            {
                Db.SubmitChanges();
            }
            catch (Exception)
            {
                throw new Exception("Exception");
            }

            return entity;
        }

        public void Put(int id, Appointment entity)
        {
            var isEntity = from x in Db.DoctorAppointmentTbls
                where x.Id == entity.Id
                select x;


            var entitySingle = isEntity.Single();


            entitySingle.PatientName = entity.PatientName;
            entitySingle.Address = entity.Address;
            entitySingle.PhoneNumber = entity.PhoneNumber;
            entitySingle.AssignDoctorId = entity.AssignDoctorId;
            entitySingle.AssignDoctorName = entity.AssignDoctorName;
            entitySingle.AdvanceAmount = entity.AdvanceAmount;
            entitySingle.RemainingAmount = entity.RemainingAmount;
            entitySingle.StartTime = entity.StartTime.ToString();
            entitySingle.EndTime = entity.EndTime.ToString();
            entitySingle.IsPastDate = entity.IsPastDate;
            entitySingle.UpdatedOnUtc = DateTime.Now;
            try
            {
                Db.SubmitChanges();
            }
            catch (Exception)
            {
                throw new Exception("Exception");
            }
        }


        public void Delete(int id)
        {
            var query = from x in Db.DoctorAppointmentTbls
                where x.Id == id
                select x;

            if (query.Count() == 1)
            {
                var entity = query.SingleOrDefault();
                Db.DoctorAppointmentTbls.DeleteOnSubmit(entity ?? throw new InvalidOperationException());
            }

            try
            {
                Db.SubmitChanges();
            }
            catch (Exception)
            {
                throw new Exception("Exception");
            }
        }

        //custom
        public List<SelectListItem> GetAllDoctorsSelectList()
        {
            var entities = Db.DoctorTbls.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.FullName
                //   Selected = (x.IsHomePageBanner != null && x.IsHomePageBanner == true)
            }).ToList();

            return entities;
        }

        public int GetVisitDay(int day, List<int> daysIds)
        {
            //3
            //2,4,6
            //5

            var j = 1; //4
            var nextDay = day + 1;

            for (var i = nextDay; i <= 6; i++)
                if (daysIds.Contains(i))
                    return j + 1;
                else
                    j++;

            //5 -fir
            //2,4 - mon,wed

            var minDay = daysIds.Min();
            var visitCountDay = minDay + j + 1;
            return visitCountDay;
        }


        private AppointmentTime GetVisitTimeAppointment(int willVisitDay, TimeSpan visitingStart, TimeSpan visitingEnd,
            int visitTimeSlot, int doctorDetailsId, List<int> daysIds, int day, int weeks = 1)
        {
            var toBeVisitDay = DateTime.Today.AddDays(willVisitDay);
            var startVisitTime = new DateTime(toBeVisitDay.Year, toBeVisitDay.Month, toBeVisitDay.Day,
                visitingStart.Hours, visitingStart.Minutes, visitingStart.Seconds);
            var staringVisitTimeDay = ToUnixTime(startVisitTime);

            var endVisitTime = new DateTime(toBeVisitDay.Year, toBeVisitDay.Month, toBeVisitDay.Day, visitingEnd.Hours,
                visitingEnd.Minutes, visitingEnd.Seconds);

            var endDateTimeLoop = ToUnixTime(endVisitTime) - (uint) visitTimeSlot;

            var appointmentTime =
                GetVisitTimeFromDay(staringVisitTimeDay, visitTimeSlot, doctorDetailsId, endDateTimeLoop);
            if (appointmentTime == null)
            {
                // daysIds.Remove(day + willVisitDay);
                //2
                //3,5,7

                willVisitDay = weeks * 7 + daysIds.IndexOf(0);
                weeks++;
                return GetVisitTimeAppointment(willVisitDay, visitingStart, visitingEnd, visitTimeSlot, doctorDetailsId,
                    daysIds, day, weeks);
            }

            return appointmentTime;
        }


        private AppointmentTime GetVisitTimeFromDay(uint staringVisitTimeDay, int visitTimeSlot, int doctorId,
            uint endDateTimeLoop, int counter = 0)
        {
            if (endDateTimeLoop < staringVisitTimeDay)
                return null;

            counter++;
            var startTimeForDb = (long) staringVisitTimeDay * 1000;
            var startVisitTimeDb = startTimeForDb.ToString();

            var query = (from result in Db.DoctorAppointmentTbls
                where result.AssignDoctorId == doctorId &&
                      result.StartTime == startVisitTimeDb
                select result).FirstOrDefault();


            if (string.IsNullOrEmpty(query?.StartTime))
            {
                var appointment = new AppointmentTime
                {
                    AppointmentTimeStart = staringVisitTimeDay,
                    Serial = counter
                };
                return appointment;
            }

            var startTime = staringVisitTimeDay + (uint) visitTimeSlot * 60;
            return GetVisitTimeFromDay(startTime, visitTimeSlot, doctorId, endDateTimeLoop, counter);
        }
    }
}