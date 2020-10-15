using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using Shodypati.DAL;
using Shodypati.Models;

namespace Shodypati.Controllers.Api
{
    public class AppointmentsApiController : ApiController
    {
        private readonly IAppointmentAccessRepository<Appointment, int> _repository;

        public AppointmentsApiController(IAppointmentAccessRepository<Appointment, int> r)
        {
            _repository = r;
        }

        [System.Web.Http.Route("api/AppointmentsApi/")]
        // GET: api/AppointmentsApi
        public IEnumerable<Appointment> GetAppointments()
        {
            return _repository.Get();
        }

        [System.Web.Http.Route("api/AppointmentsApi/{id}")]
        // GET: api/AppointmentsApi/5
        [ResponseType(typeof(Appointment))]
        public IHttpActionResult GetAppointment(int id)
        {
            var item = _repository.Get(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [System.Web.Http.Route("api/AppointmentsApi/{id}")]
        // PUT: api/AppointmentsApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAppointment(int id, Appointment entity)
        {
            _repository.Put(id, entity);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [System.Web.Http.Route("api/AppointmentsApi/")]
        // POST: api/AppointmentsApi
        [ResponseType(typeof(Appointment))]
        //public IHttpActionResult PostAppointment(Appointment entity)
        public IHttpActionResult PostAppointment(Appointment entity)
        {
            //need to compute this 
            //from MOBILE

            entity.StartTime = 0;
            entity.EndTime = 0;
            entity.AdvanceAmount = 0;

            var entityPost = _repository.Post(entity);
            return Ok(entityPost);
        }


        [System.Web.Http.Route("api/AppointmentsApi/PostWeb")]
        // POST: api/AppointmentsApi/PostWeb
        [ResponseType(typeof(Appointment))]
        public IHttpActionResult PostAppointmentWeb(Appointment entity)
        {
            _repository.PostWeb(entity);
            return Ok(entity);
        }

        [System.Web.Http.Route("api/AppointmentsApi/{id}")]
        // DELETE: api/AppointmentsApi/5
        [ResponseType(typeof(Appointment))]
        public IHttpActionResult DeleteAppointment(int id)
        {
            _repository.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        //custom

        [System.Web.Http.Route("api/AppointmentsApi/GetAllDoctorsSelectList/")]
        // GetAllBrandsSelectList: api/OrderPaymentMethodApi/GetAllOrderPaymentMethodSelectList
        public List<SelectListItem> GetAllDoctorsSelectList()
        {
            return _repository.GetAllDoctorsSelectList();
        }

        [System.Web.Http.Route("api/AppointmentsApi/GetByDoctor/{id}")]
        // GET: api/AppointmentsApi
        public IEnumerable<Appointment> GetByDoctor(int id)
        {
            return _repository.GetByDoctor(id);
        }
    }
}