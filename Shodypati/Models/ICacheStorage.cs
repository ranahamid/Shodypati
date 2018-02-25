using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shodypati.Models
{
    public interface ICacheStorage
    {
        void Remove(string key);
        void Add(string key, object data);
        T Get<T>(string key);
    }
}
