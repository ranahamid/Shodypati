using Microsoft.Practices.Unity;
using Shodypati.Filters;
using Shodypati.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace Shodypati.DAL
{
    [ExceptionHandlerAttribute]
    public class LogDataAccessRepository : ILogAccessRepository<Log, int>
    {
        public ShodypatiDataContext db;

        public LogDataAccessRepository()
        {
            db = new ShodypatiDataContext();
        }


        public IEnumerable<Log> Get()
        {
            List<Log> entities = db.LogTbls.Select(x => new Log()
            {
                Id                          =x.Id                   ,
                ExceptionMessage            =x.ExceptionMessage     ,
                ExceptionStackTrace         =x.ExceptionStackTrace  ,
                ControllerName              =x.ControllerName       ,
                IpAddress                   =x.IpAddress            ,
                Browser                     =x.Browser              ,
                OS                          =x.OS                   ,
                UserId                      =x.UserId               ,
                ActionName                  =x.ActionName           ,
                CreatedOnUtc                =x.CreatedOnUtc
            }).ToList();

            return entities;
        }

        public Log Get(int id)
        {
            Log entity = db.LogTbls.Where(x=>x.Id==id).Select(x=>new Log()
            {
                Id                          =x.Id                   ,
                ExceptionMessage            =x.ExceptionMessage     ,
                ExceptionStackTrace         =x.ExceptionStackTrace  ,
                ControllerName              =x.ControllerName       ,
                IpAddress                   =x.IpAddress            ,
                Browser                     =x.Browser              ,
                OS                          =x.OS                   ,
                UserId                      =x.UserId               ,
                ActionName                  =x.ActionName           ,
                CreatedOnUtc                =x.CreatedOnUtc
                     
            }).FirstOrDefault();

            if (entity == null)
            {
                return null;
            }
            return entity;
        }



        public void Delete(int id)
        {
            var q = from x in db.LogTbls
                where x.Id == id
                select x;

            if (q != null && q.Count() == 1)
            {
                LogTbl entity = q.FirstOrDefault();
                db.LogTbls.DeleteOnSubmit(entity);
            }

            try
            {
                db.SubmitChanges();
            }
            catch (Exception)
            {
                throw new Exception("Exception");
            }
        }

        
        public void Post(Log entity)
        {
            throw new Exception("Log Error!");
        }

        public void Put(int id, Log entity)
        {
            throw new Exception("Log Error!");
        }
    }
}