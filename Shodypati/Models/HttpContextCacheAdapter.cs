using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace Shodypati.Models
{
    public class HttpContextCacheAdapter : ICacheStorage
    {
        public void Remove(string key)
        {
            HttpContext.Current.Cache.Remove(key);
        }
        public void Add(string key, object data)
        {
          //  HttpContext.Current.Cache.Insert(key, data);
          
            HttpContext.Current.Cache.Insert(key, data, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 60, 0), CacheItemPriority.Normal, null);
        }
        public T Get<T>(string key)
        {
            T item = (T)HttpContext.Current.Cache.Get(key);
            if (item == null)
            {
                item = default(T);
            }
            return item;
        }
    }
}