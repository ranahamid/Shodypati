using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shodypati.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Shodypati.DAL;
using System.Configuration;
using Shodypati.Filters;

namespace Shodypati.Controllers
{
    [ExceptionHandler]
    [Authorize(Roles = "Admin")]
    public class AppointmentsController : BaseController
    {

        public AppointmentsController()
        {
            //api url                  
            url = baseUrl + "api/AppointmentsApi";
        }


        // GET: Appointments
        public async Task<ActionResult> Index()
        {
            return await IndexBaseTask();
        }

        public async Task<ActionResult> IndexBaseTask()
        {
            var entity = await GetAllDoctor();
            var allWorkingTypes = await GetAllDoctorWorkingTypes();

            if (entity != null)
            {
                var model = new AppointmentSelectList
                {
                    AllDoctors = entity,
                    AllWorkingSelectListItems = allWorkingTypes
                };
                return View("Index", model);
            }
            throw new Exception("Exception");
        }

     
        public async Task<List<SelectListItem>> GetAllDoctor()
        {
            var responseMessage = await client.GetAsync(url + "/" + "GetAllDoctorsSelectList");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var entity = JsonConvert.DeserializeObject<List<SelectListItem>>(responseData);
                return entity;
            }
            return null;
        }


        [HttpPost]
        public async Task<ActionResult> Index(AppointmentSelectList appointmentSelect)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Create", appointmentSelect);
            }
            return await IndexBaseTask();
        }



        // GET: Appointments/Create
        public async Task< ActionResult > Create(AppointmentSelectList appointmentSelect)
        {
            var doctorId = appointmentSelect.SelectedDoctorId;

            var responseMessage = await client.GetAsync(url+"/"+ "GetByDoctor" + "/" + doctorId);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var entity = JsonConvert.DeserializeObject<List<Appointment>>(responseData);

                //doctor details
                HttpResponseMessage responseMessageDoctor = await client.GetAsync(baseUrl+ "api/DoctorsApi" + "/" + doctorId);
                if (!responseMessageDoctor.IsSuccessStatusCode) throw new Exception("Exception");

                var responseDataDoctro = responseMessageDoctor.Content.ReadAsStringAsync().Result;
                var detailsOfDoctor = JsonConvert.DeserializeObject<Doctor>(responseDataDoctro);

             

                //Appointment newAppointment = new Appointment {AssignDoctorId = int.Parse(doctorId) };

                AppoinmentCreate createAppointment = new AppoinmentCreate
                {
                    Appointments = entity,
                    DoctorId = doctorId,
                    DoctorDetails= detailsOfDoctor,                  
                };

                return View(createAppointment);
            }
            return View();
        }

      

        // POST: Appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]      
        public async Task<ActionResult> Create( string patientName, string startTime, string endtime, string address, string phoneNumber, string advanceAmount, string assigndoctorid, string doctorfullName)
        {
            //string patientName, string assignDoctorId, string address, string phoneNumber, string advanceAmount, string startTime, string endTime
            Appointment entity = new Appointment
            {
                PatientName = patientName,
                StartTime = long.Parse(startTime),
                EndTime = long.Parse(endtime),
                Address = address,
                PhoneNumber = phoneNumber,
                AdvanceAmount = Int32.Parse(advanceAmount),                
                AssignDoctorId = Int32.Parse( assigndoctorid),
                AssignDoctorName = doctorfullName,
            };

            if (!ModelState.IsValid)
                return Json(new { success = false, responseText = "An error occured." }, JsonRequestBehavior.AllowGet);
            //end parent name
            var responseMessage = await client.PostAsJsonAsync(url+"/"+ "PostWeb", entity);
            if (responseMessage.IsSuccessStatusCode)
            {
                // return View();
                return Json(new { success = true, responseText = "Successfully executed." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, responseText = "An error occured." }, JsonRequestBehavior.AllowGet);
        }


        [AllowAnonymous]
        public ActionResult FillDoctors(string id)
        {
            var vm = new Shodypati.Models.DoctorViewModel()
            {
                SelectedWorkingTypeId = id
            };

            return PartialView("_DoctorsDrop", vm);
        }

        public async  Task<ActionResult> GetDoctorAppoinmentLists(string doctorId)
        {
            var responseMessage = await client.GetAsync(url + "/" + "GetByDoctor" + "/" + doctorId);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var entity = JsonConvert.DeserializeObject<List<Appointment>>(responseData);

                string data = JsonConvert.SerializeObject(entity);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        // GET: Appointments/Details/5
        public ActionResult Details(int? id)
        {
            return View();
        }

        // GET: Appointments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View();
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Appointment appointment)
        {
            return View();
        }

        // GET: Appointments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View();
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            return View();
        }

    }
}
