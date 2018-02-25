using Shodypati.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shodypati.DAL
{
   

    public interface IOrderAccessRepository<TEntity, in TPrimaryKey> where TEntity : class
    {
        IEnumerable<OrderMobile> Get();
        OrderMobile Get(TPrimaryKey id);
        OrderMobile Post(TEntity entity, ApplicationUserManager UserManager);
        void Put(TPrimaryKey id, TEntity entity);
        void Delete(TPrimaryKey id);
    }
}
