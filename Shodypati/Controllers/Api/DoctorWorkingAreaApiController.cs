using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Shodypati.Models;
using Shodypati.DAL;


namespace Shodypati.Controllers.Api
{
    public class DoctorWorkingAreaApiController : ApiController
    {
        private readonly IDoctorWorkingAreaAccessRepository<DoctorWorkingArea, int> _repository;
        public DoctorWorkingAreaApiController(IDoctorWorkingAreaAccessRepository<DoctorWorkingArea, int> r)
        {
            _repository = r;
        }

        [Route("api/DoctorWorkingAreaApi/")]
        // GET: api/DoctorWorkingAreaApi
        public IEnumerable<DoctorWorkingArea> GetDoctors()
        {
            return _repository.Get();
        }

        [Route("api/DoctorWorkingAreaApi/{id}")]
        // GET: api/DoctorWorkingAreaApi/5
        [ResponseType(typeof(DoctorWorkingArea))]
        public IHttpActionResult GetDoctor(int id)
        {
            var item = _repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [Route("api/DoctorWorkingAreaApi/{id}")]
        // PUT: api/DoctorWorkingAreaApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDoctor(int id, DoctorWorkingArea entity)
        {
            _repository.Put(id, entity);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("api/DoctorWorkingAreaApi/")]
        // POST: api/DoctorWorkingAreaApi
        [ResponseType(typeof(DoctorWorkingArea))]
        public IHttpActionResult PostDoctor(DoctorWorkingArea entity)
        {
            _repository.Post(entity);
            return Ok(entity);
        }

        [Route("api/DoctorWorkingAreaApi/{id}")]
        // DELETE: api/DoctorWorkingAreaApi/5
        [ResponseType(typeof(DoctorWorkingArea))]
        public IHttpActionResult DeleteDoctor(int id)
        {
            _repository.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }
        
        //custom
        [Route("api/DoctorWorkingAreaApi/GetAllDoctorWorkingAreasSelectList/")]
        // GetAllBrandsSelectList: api/DoctorWorkingAreaApi/GetAllDoctorWorkingAreasSelectList
        public List<System.Web.Mvc.SelectListItem> GetAllDoctorWorkingAreasSelectList()
        {
            return _repository.GetAllDoctorWorkingAreasSelectList();
        }
    }
}