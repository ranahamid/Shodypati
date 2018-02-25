using Microsoft.Practices.Unity;
using Shodypati.Controllers;
using Shodypati.Filters;
using Shodypati.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Shodypati.DAL
{
    [ExceptionHandlerAttribute]
    public class DoctorDataAccessRepository : BaseController, IDoctorAccessRepository<Doctor, int>
    {
        public DoctorDataAccessRepository()
        {
            Db = new ShodypatiDataContext();
        }
        public IEnumerable<Doctor> Get()
        {
            var entities = Db.DoctorTbls.Select(x => new Doctor()
            {
                Id = x.Id,
                FullName = x.FullName,
                RegistrationNo = x.RegistrationNo,
                RegistrationType = x.RegistrationType,
                ClinicHospitalName = x.ClinicHospitalName,
                Designation = x.Designation,
                YearOfExperience = x.YearOfExperience,
                SelectedDoctorWorkingTypeId = x.WorkingArea,
         
                Addresss = x.Addresss,
                DateOfBirth = x.DateOfBirth,
                PhoneNumber = x.PhoneNumber,
                Email = x.Email,
                SelectedVisitDaysStr = GetDaysNameFromNumbers(x.CanVisitDays),
                VisitTimeStart = x.VisitTimeStart,
                VisitTimeEnd = x.VisitTimeEnd,
                VisitTime = GetTimeFromTimeSpan(x.VisitTimeStart, x.VisitTimeEnd),
                SlotDuration = x.SlotDuration,
                VisitFee = x.VisitFee,
                VisitingCard = HttpUtility.UrlPathEncode(baseUrl + x.VisitingCard),
                RawDBImagePath = x.VisitingCard,
                CreatedOnUtc = x.CreatedOnUtc,
                UpdatedOnUtc = x.UpdatedOnUtc,
                Active = x.Active,
            }).ToList();

            return entities;
        }

        public string GetTimeFromTimeSpan(TimeSpan start,TimeSpan end)
        {
            var timeStart = start.ToString(@"hh\:mm\:ss");
            var timeEnd = end.ToString(@"hh\:mm\:ss");
            return timeStart+" - " +timeEnd;
        }

      

        public Doctor Get(int id)
        {
            var entity = Db.DoctorTbls.Where(x => x.Id == id).Select(x => new Doctor()
            {
                Id = x.Id,
                FullName = x.FullName,
                RegistrationNo = x.RegistrationNo,
                RegistrationType = x.RegistrationType,
                ClinicHospitalName = x.ClinicHospitalName,
                Designation = x.Designation,
                YearOfExperience = x.YearOfExperience,
                SelectedDoctorWorkingTypeId = x.WorkingArea,
                //WorkingTypeName = Db.DoctorWorkingAreaTbls.SingleOrDefault(y => y.Id.ToString() == x.WorkingArea).WorkingArea,
                Addresss = x.Addresss,
                DateOfBirth = x.DateOfBirth,
                PhoneNumber = x.PhoneNumber,
                Email = x.Email,
                SelectedVisitDaysStr = GetDaysNameFromNumbers(x.CanVisitDays),
                SelectedVisitDays = GetListFromCommaSeparatedIntList(x.CanVisitDays),
                HiddenDays =GetHiddenDaysFromActiveDays(x.CanVisitDays),
                VisitTimeStart = x.VisitTimeStart,
                VisitTimeEnd = x.VisitTimeEnd,
                VisitTime = GetTimeFromTimeSpan(x.VisitTimeStart, x.VisitTimeEnd) ,
                SlotDuration = x.SlotDuration,
                VisitFee = x.VisitFee,
                VisitingCard = HttpUtility.UrlPathEncode(baseUrl + x.VisitingCard),
                RawDBImagePath = x.VisitingCard,
                CreatedOnUtc = x.CreatedOnUtc,
                UpdatedOnUtc = x.UpdatedOnUtc,
                Active = x.Active,

            }).SingleOrDefault();

            var query = Db.DoctorWorkingAreaTbls.SingleOrDefault(y => y.Id.ToString() == entity.SelectedDoctorWorkingTypeId);
            if (query != null && entity != null)
                entity.WorkingTypeName = query.WorkingArea;

            return entity;
        }

        public void Post(Doctor entity)
        {
            var imgAddress = string.Empty;
            if (entity.VisitingCard != null)
            {
                imgAddress = entity.VisitingCard.TrimStart('/');
            }

            // string visitDays=string.Empty;

            var visitDays = new StringBuilder();
            foreach (var item in entity.SelectedVisitDays)
            {

                if (visitDays.ToString() != string.Empty)
                {
                    visitDays.Append("," + item);
                }
                else
                {
                    visitDays.Append(item);
                }
            }

            if (entity.SlotDuration == 0)
                entity.SlotDuration = 10;

            if (entity.VisitTimeEnd.ToString() == "00:00:00")
            {
                entity.VisitTimeEnd = new TimeSpan(0, 23, 0, 0);
            }

            Db.DoctorTbls.InsertOnSubmit(new DoctorTbl
            {
                //   Id              = entity.Id,           

                FullName = entity.FullName,
                RegistrationNo = entity.RegistrationNo,
                RegistrationType = entity.RegistrationType,
                ClinicHospitalName = entity.ClinicHospitalName,
                Designation = entity.Designation,
                YearOfExperience = entity.YearOfExperience,
                WorkingArea = entity.SelectedDoctorWorkingTypeId,
                Addresss = entity.Addresss,
                DateOfBirth = entity.DateOfBirth,
                PhoneNumber = entity.PhoneNumber,
                Email = entity.Email,
                CanVisitDays = visitDays.ToString(),

                VisitTimeStart = entity.VisitTimeStart,
                VisitTimeEnd = entity.VisitTimeEnd,
                SlotDuration = entity.SlotDuration,
                VisitFee = entity.VisitFee,

                VisitingCard = imgAddress,
                CreatedOnUtc = DateTime.Now,
                UpdatedOnUtc = DateTime.Now,
                Active = entity.Active,
            });
            try
            {
                Db.SubmitChanges();
            }
            catch (Exception)
            {
                throw new Exception("Exception");
            }
        }

        public void Put(int id, Doctor entity)
        {
            var isEntity = from x in Db.DoctorTbls
                           where x.Id == entity.Id
                           select x;

            var imgAddress = string.Empty;
            if (entity.RawDBImagePath != null)
            {
                imgAddress = entity.RawDBImagePath.TrimStart('/');
            }

            var visitDays = new StringBuilder();
            foreach (var item in entity.SelectedVisitDays)
            {

                if (visitDays.ToString() != string.Empty)
                {
                    visitDays.Append("," + item);
                }
                else
                {
                    visitDays.Append(item);
                }
            }

            if (entity.SlotDuration == 0)
                entity.SlotDuration = 10;


            var entitySingle = isEntity.Single();
            entitySingle.FullName = entity.FullName;
            entitySingle.RegistrationNo = entity.RegistrationNo;
            entitySingle.RegistrationType = entity.RegistrationType;
            entitySingle.ClinicHospitalName = entity.ClinicHospitalName;
            entitySingle.Designation = entity.Designation;
            entitySingle.YearOfExperience = entity.YearOfExperience;
            entitySingle.WorkingArea = entity.SelectedDoctorWorkingTypeId;
            entitySingle.Addresss = entity.Addresss;
            entitySingle.DateOfBirth = entity.DateOfBirth;
            entitySingle.PhoneNumber = entity.PhoneNumber;
            entitySingle.Email = entity.Email;
            entitySingle.CanVisitDays = visitDays.ToString();
            entitySingle.VisitTimeStart = entity.VisitTimeStart;
            entitySingle.VisitTimeEnd = entity.VisitTimeEnd;
            entitySingle.SlotDuration = entity.SlotDuration;
            entitySingle.VisitFee = entity.VisitFee;
            entitySingle.VisitingCard = imgAddress;
            entitySingle.UpdatedOnUtc = DateTime.Now;
            entitySingle.Active = entity.Active;

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
            var query = from x in Db.DoctorTbls
                        where x.Id == id
                        select x;

            if (query.Count() == 1)
            {
                var entity = query.SingleOrDefault();
                Db.DoctorTbls.DeleteOnSubmit(entity ?? throw new InvalidOperationException());
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

    }
}