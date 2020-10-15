using System.Collections.Generic;
using System.Web.Mvc;

namespace Shodypati.DAL
{
    public interface ICategoryAccessRepository<TEntity, in TPrimaryKey> where TEntity : class
    {
        IEnumerable<TEntity> Get();
        TEntity Get(TPrimaryKey id);
        void Post(TEntity entity);
        void Put(TPrimaryKey id, TEntity entity);
        void Delete(TPrimaryKey id);

        List<SelectListItem> GetAllCategoriesSelectList();
    }
}