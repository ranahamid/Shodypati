using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shodypati.Models;

namespace Shodypati.DAL
{
    public interface IAppointmentAccessRepository<TEntity, in TPrimaryKey> where TEntity : class
    {
        IEnumerable<TEntity> Get();
        TEntity Get(TPrimaryKey id);
        Appointment Post(TEntity entity);
        Appointment PostWeb(TEntity entity);
        void Put(TPrimaryKey id, TEntity entity);
        void Delete(TPrimaryKey id);
        //custom
        List<System.Web.Mvc.SelectListItem> GetAllDoctorsSelectList();
        IEnumerable<TEntity> GetByDoctor(TPrimaryKey id);
        
    }
}