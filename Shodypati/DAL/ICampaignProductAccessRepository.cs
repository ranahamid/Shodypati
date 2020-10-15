using System.Collections.Generic;

namespace Shodypati.DAL
{
    public interface ICampaignProductAccessRepository<TEntity, in TPrimaryKey> where TEntity : class
    {
        IEnumerable<TEntity> Get();

        TEntity Get(TPrimaryKey id);

        //void Post(TEntity entity);
        void Put(TEntity entity);

        //void Delete(TPrimaryKey id);
        //custom
        List<string> GetAllCampaignProductsList();
    }
}