using System;
using System.Collections.Generic;
using System.Text;

namespace Pozitive.Entities.Repos
{
    public interface IRepository<T> : IEnumerable<T>, IDisposable
    {
        void Add(T item);
        T GetItem(int id);
        void Update(T item);
        void Delete(int id);
    }
}
