using Shodypati.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shodypati.DAL
{
    public interface IProductAccessRepository<TEntity, in TPrimaryKey> where TEntity : class
    {
        IEnumerable<TEntity> Get();
        TEntity Get(TPrimaryKey id);
        void Post(TEntity entity);
        void Put(TPrimaryKey id, TEntity entity);
        void Delete(TPrimaryKey id);
        //custom
        List<System.Web.Mvc.SelectListItem> GetAllProductsSelectList();

        List<CategoryMobile> GetProductsByCategoriesList();
        List<CategoryMobile> GetProductsByCategoriesListWeb();

        List<ProductMobile> GetProductsBy(int categoryid, int merchantid);
    
    }
}