namespace Shodypati.Models
{
    public class NullObjectCache : ICacheStorage
    {
        void ICacheStorage.Remove(string key)
        {
        }

        void ICacheStorage.Add(string key, object data)
        {
        }

        T ICacheStorage.Get<T>(string key)
        {
            return default(T);
        }
    }
}