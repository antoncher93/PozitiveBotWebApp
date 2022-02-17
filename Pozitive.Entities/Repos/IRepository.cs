using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pozitive.Entities.Repos
{
    public interface IRepository<T> : IDisposable
    {
        void Add(T item);
        T GetItem(int id);
        IEnumerable<Person> GetAll();
        void Update(T item);
        void Delete(int id);
    }
}
