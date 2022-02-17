using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pozitive.Entities;
using Pozitive.Entities.Repos;

namespace Positive.SqlDbContext.Repos
{
    public class UserRepos : IRepository<Person>
    {
        private readonly PozitiveSqlContext _db;

        public UserRepos(PozitiveSqlContext db)
        {
            _db = db;   
        }

        public void Dispose()
        {
            //_db.Dispose();
        }

        public void Add(Person item)
        {
            _db.Users.Add(item);
            _db.SaveChanges();
        }

        public Person GetItem(int id)
        {
            return _db.Users.Find(id);
        }

        public void Update(Person item)
        {
            var user = _db.Users.FindAsync(item.Id);
            if (user != null)
            {
                _db.Entry(item).State = EntityState.Modified;
                _db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var user = _db.Users.Find(id);
            if (user != null)
            {
                _db.Update(user);
                _db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return await _db.Users.ToListAsync();
        }

        public IEnumerable<Person> GetAll()
        {
            return _db.Users.ToListAsync().Result;
        }
    }
}
