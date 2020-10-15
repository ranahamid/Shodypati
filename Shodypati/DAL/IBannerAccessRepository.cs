using System.Collections.Generic;
using System.Web.Mvc;
using Shodypati.Models;

namespace Shodypati.DAL
{
    public interface IBannerAccessRepository<TEntity, in TPrimaryKey> where TEntity : class
    {
        IEnumerable<TEntity> Get();
        TEntity Get(TPrimaryKey id);
        void Post(TEntity entity);
        void Put(TPrimaryKey id, TEntity entity);

        void Delete(TPrimaryKey id);

        //custom
        List<SelectListItem> GetAllBannersSelectList();
        BannerMobile GetHomePageBanner();
        IEnumerable<TEntity> GetAllBanner();
    }
}