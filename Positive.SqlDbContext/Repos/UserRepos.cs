using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Pozitive.Entities;
using Pozitive.Entities.Repos;

namespace Positive.SqlDbContext.Repos
{
    class UserRepos : IRepository<Person>
    {
        private readonly PozitiveSqlContext _db;

        public UserRepos(PozitiveSqlContext db)
        {
            _db = db;   
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public async void Add(Person item)
        {
            _db.Users.Add(item);
            await _db.SaveChangesAsync();
        }

        public Person GetItem(int id)
        {
            return _db.Users.Find(id);
        }

        public async void Update(Person item)
        {
            var user = await _db.Users.FindAsync(item.Id);
            if (user != null)
            {
                _db.Update(user);
                await _db.SaveChangesAsync();
            }
        }

        public async void Delete(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user != null)
            {
                _db.Update(user);
                await _db.SaveChangesAsync();
            }
        }

        public IEnumerator<Person> GetEnumerator()
        {
            return ((IEnumerable<Person>)_db.Users).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
