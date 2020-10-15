using System.Collections.Generic;
using System.Web.Mvc;
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
        List<SelectListItem> GetAllDoctorsSelectList();
        IEnumerable<TEntity> GetByDoctor(TPrimaryKey id);
    }
}