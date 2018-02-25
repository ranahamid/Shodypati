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
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Shodypati.DAL
{
    [ExceptionHandlerAttribute]
    public class DoctorWorkingAreaDataAccessRepository : BaseController, IDoctorWorkingAreaAccessRepository<DoctorWorkingArea, int>
    {
        public DoctorWorkingAreaDataAccessRepository()
        {
            Db = new ShodypatiDataContext();
        }

        public IEnumerable<DoctorWorkingArea> Get()
        {
            var entities = Db.DoctorWorkingAreaTbls.Select(x => new DoctorWorkingArea()
            {
                Id = x.Id,
                Description = x.Description,
                WorkingArea = x.WorkingArea
            }).ToList();

            return entities;
        }

        public DoctorWorkingArea Get(int id)
        {
            var entity = Db.DoctorWorkingAreaTbls.Where(x => x.Id == id).Select(x => new DoctorWorkingArea()
            {
                Id = x.Id,
                Description = x.Description,
                WorkingArea = x.WorkingArea
            }).SingleOrDefault();

            return entity;
        }

        public void Post(DoctorWorkingArea entity)
        {

            Db.DoctorWorkingAreaTbls.InsertOnSubmit(new DoctorWorkingAreaTbl
            {
                Description = entity.Description,
                WorkingArea = entity.WorkingArea
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

        public void Put(int id, DoctorWorkingArea entity)
        {
            var isEntity = from x in Db.DoctorWorkingAreaTbls
                           where x.Id == entity.Id
                           select x;
            
            var entitySingle = isEntity.Single();
            entitySingle.Description = entity.Description;
            entitySingle.WorkingArea = entity.WorkingArea;

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
            var query = from x in Db.DoctorWorkingAreaTbls
                        where x.Id == id
                        select x;

            if (query.Count() == 1)
            {
                var entity = query.SingleOrDefault();
                Db.DoctorWorkingAreaTbls.DeleteOnSubmit(entity ?? throw new InvalidOperationException());
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
        public List<SelectListItem> GetAllDoctorWorkingAreasSelectList()
        {
            var entities = Db.DoctorWorkingAreaTbls.Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.WorkingArea,
                //   Selected = (x.IsHomePageBanner != null && x.IsHomePageBanner == true)
            }).ToList();

            return entities;
        }
    }
}