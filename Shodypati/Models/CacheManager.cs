using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;

namespace Shodypati.Models
{
    public class CacheManager    
    {
        /// <summary>
        /// Example
        //object cachedObjectData2 = ContextCache.Get("AllProductskey");
        //    if (cachedObjectData2 != null)
        //    {
        //        allProducts = (List<ClinicalProduct>) cachedObjectData2;
        //    }
        //    else
        //    {

        //        allProducts = manager.GetProducts(service);
        //        ContextCache.Max("AllProductskey", allProducts);
        //    }
        /// </summary>
/// 
#region Fields
private static readonly Cache _cache;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new instance of the APCache class
        /// </CacheManager>
        static CacheManager()
        {
            var current = HttpContext.Current;
            if (current != null)
                _cache = current.Cache;
            else
                _cache = HttpRuntime.Cache;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the cache is enabled
        /// </summary>
        public static bool IsEnabled => true; //HulkERPConfig.CacheEnabled;

        #endregion

        #region Methods

        /// <summary>
        /// Removes all keys and values from the cache
        /// </summary>
        public static void Clear()
        {
            var enumerator = _cache.GetEnumerator();
            while (enumerator.MoveNext()) _cache.Remove(enumerator.Key.ToString());
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        public static object Get(string key)
        {
            return _cache[key];
        }

        /// <summary>
        /// Adds the specified key and object to the cache.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="obj">object</param>
        /// 
        //public static void Max(string key, object obj)
        //{
        //    Max(key, obj, null);
        //}

        /// <summary>
        /// Adds the specified key and object to the cache.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="obj">object</param>
        /// <param name="dep">cache dependency</param>
        public static void Max(string key, object obj)
        {

            if (IsEnabled && obj != null)
            //HttpRuntime.Cache.Insert("GetAllAnswer", allAnswers, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromHours(24));
                _cache.Insert(key, obj, null, Cache.NoAbsoluteExpiration, TimeSpan.FromHours(24));
        }

        public static void MaxOne(string key, object obj)
        {

            if (IsEnabled && obj != null)
            //HttpRuntime.Cache.Insert("GetAllAnswer", allAnswers, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromHours(24));
                _cache.Insert(key, obj, null, Cache.NoAbsoluteExpiration, TimeSpan.FromHours(1));
        }

        public static void MaxOneMinute(string key, object obj)
        {

            if (IsEnabled && obj != null)
            //HttpRuntime.Cache.Insert("GetAllAnswer", allAnswers, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromHours(24));
                _cache.Insert(key, obj, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(1));
        }

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            _cache.Remove(key);
        }

        /// <summary>
        /// Removes items by pattern
        /// </summary>
        /// <param name="pattern">pattern</param>
        public static void RemoveByPattern(string pattern)
        {
            var enumerator = _cache.GetEnumerator();
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            while (enumerator.MoveNext())
                if (regex.IsMatch(enumerator.Key.ToString())) _cache.Remove(enumerator.Key.ToString());
        }

        #endregion
    }
}