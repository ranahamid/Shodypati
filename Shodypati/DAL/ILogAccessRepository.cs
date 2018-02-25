using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shodypati.DAL
{
    public interface ILogAccessRepository<TEntity, in TPrimaryKey> where TEntity : class
    {
        IEnumerable<TEntity> Get();
        TEntity Get(TPrimaryKey id);
        void Post(TEntity entity);
        void Put(TPrimaryKey id, TEntity entity);
        void Delete(TPrimaryKey id);
        
    }
}