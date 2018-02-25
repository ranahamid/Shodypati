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
    public class LogApiController : ApiController
    {
        private ILogAccessRepository<Log, int> _repository;

        public LogApiController(ILogAccessRepository<Log, int> r)
        {
            _repository = r;
        }


        [Route("api/LogApi/")]
        // GET: api/LogApi
        public IEnumerable<Log> GetLogs()
        {
            return _repository.Get();
        }


        [Route("api/LogApi/{id}")]
        // GET: api/LogApi/5
        [ResponseType(typeof(Log))]
        public IHttpActionResult GetLog(int id)
        {
            var item = _repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [Route("api/LogApi/{id}")]
        // PUT: api/LogApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLog(int id, Log log)
        {
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("api/LogApi/")]
        // POST: api/LogApi
        [ResponseType(typeof(Log))]
        public IHttpActionResult PostLog(Log log)
        {
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("api/LogApi/{id}")]
        // DELETE: api/LogApi/5
        [ResponseType(typeof(Log))]
        public IHttpActionResult DeleteLog(int id)
        {
            _repository.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }



    }
}