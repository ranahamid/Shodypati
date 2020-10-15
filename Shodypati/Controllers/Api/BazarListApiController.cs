using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Description;
using Shodypati.DAL;
using Shodypati.Models;

namespace Shodypati.Controllers.Api
{
    public class BazarListApiController : ApiController
    {
        private readonly IBazarListAccessRepository<BazarList, int> _repository;

        private int _randomNmbr;
        private readonly string serverMapPath = "~/Content/images/BazarList/";

        public BazarListApiController(IBazarListAccessRepository<BazarList, int> r)
        {
            _repository = r;
        }

        private string StorageRoot => Path.Combine(HostingEnvironment.MapPath(serverMapPath));

        [Route("api/BazarListApi/")]
        // GET: api/BazarListApi
        public IEnumerable<BazarList> GetBazarList()
        {
            return _repository.Get();
        }

        [Route("api/BazarListApi/{id}")]
        // GET: api/BazarListApi/5
        [ResponseType(typeof(BazarList))]
        public IHttpActionResult GetBazarList(int id)
        {
            var item = _repository.Get(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [Route("api/BazarListApi/{id}")]
        // PUT: api/BazarListApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBazarList(int id, BazarList entity)
        {
            _repository.Put(id, entity);
            return StatusCode(HttpStatusCode.NoContent);
        }

        // Generate random number for email address
        public int GetRandomNumber()
        {
            return new Random().Next(100000, 100000000);
        }

        [Route("api/BazarListApi/")]
        // POST: api/CategoriesApi
        [ResponseType(typeof(Category))]
        public IHttpActionResult Post(BazarList entity)
        {
            _repository.Post(entity);
            return Ok(entity);
        }


        [Route("api/BazarListApi/PostBazarList")]
        // POST: api/BazarListApi
        //From Mobile API
        [ResponseType(typeof(BazarList))]
        public IHttpActionResult PostBazarList()
        {
            var imgAddress = string.Empty;
            //file
            _randomNmbr = GetRandomNumber();
            var totalpath = StorageRoot + _randomNmbr + "/";

            var fullPath = Path.Combine(totalpath);
            Directory.CreateDirectory(fullPath);

            var response = new HttpResponseMessage();
            var httpRequest = HttpContext.Current.Request;
            var filePath = string.Empty;

            if (httpRequest.Files.Count > 0)
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null)
                    {
                        filePath = HttpContext.Current.Server.MapPath(
                            "~/Content/images/BazarList/" + _randomNmbr + "/" + postedFile.FileName);
                        postedFile.SaveAs(filePath);
                        imgAddress = "/Content/images/BazarList/" + _randomNmbr + "/" + postedFile.FileName;
                    }
                }

            var entity = new BazarList
            {
                Name = httpRequest.Form["Name"],
                Address = httpRequest.Form["Address"],
                Number = httpRequest.Form["Number"],
                Description = httpRequest.Form["Description"],
                CreatedOnUtc = DateTime.Now,
                UpdatedOnUtc = DateTime.Now,
                MainImagePath = imgAddress
            };


            //path

            _repository.Post(entity);

            //in return 
            entity.MainImagePath = filePath;
            return Ok(entity);
        }

        [Route("api/BazarListApi/{id}")]
        // DELETE: api/BazarListApi/5
        [ResponseType(typeof(BazarList))]
        public IHttpActionResult DeleteBazarList(int id)
        {
            _repository.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}