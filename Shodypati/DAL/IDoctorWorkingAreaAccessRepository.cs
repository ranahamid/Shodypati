using System.Collections.Generic;
using System.Web.Mvc;

namespace Shodypati.DAL
{
    public interface IDoctorWorkingAreaAccessRepository<TEntity, in TPrimaryKey> where TEntity : class
    {
        IEnumerable<TEntity> Get();
        TEntity Get(TPrimaryKey id);
        void Post(TEntity entity);
        void Put(TPrimaryKey id, TEntity entity);

        void Delete(TPrimaryKey id);

        //custom
        List<SelectListItem> GetAllDoctorWorkingAreasSelectList();
    }
}