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
    public class DoctorsApiController : ApiController
    {
        private readonly IDoctorAccessRepository<Doctor, int> _repository;

        public DoctorsApiController(IDoctorAccessRepository<Doctor, int> r)
        {
            _repository = r;
        }

        [Route("api/DoctorsApi/")]
        // GET: api/DoctorsApi
        public IEnumerable<Doctor> GetDoctors()
        {
            return _repository.Get();
        }

        [Route("api/DoctorsApi/{id}")]
        // GET: api/DoctorsApi/5
        [ResponseType(typeof(Doctor))]
        public IHttpActionResult GetDoctor(int id)
        {
            var item = _repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [Route("api/DoctorsApi/{id}")]
        // PUT: api/DoctorsApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDoctor(int id, Doctor entity)
        {
            _repository.Put(id, entity);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("api/DoctorsApi/")]
        // POST: api/DoctorsApi
        [ResponseType(typeof(Doctor))]
        public IHttpActionResult PostDoctor(Doctor entity)
        {
            _repository.Post(entity);
            return Ok(entity);
        }

        [Route("api/DoctorsApi/{id}")]
        // DELETE: api/DoctorsApi/5
        [ResponseType(typeof(Doctor))]
        public IHttpActionResult DeleteDoctor(int id)
        {
            _repository.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }

    }
}