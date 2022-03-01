using Pozitive.Entities;
using Pozitive.Entities.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Positive.SqlDbContext.Repos
{
    public class DocumentRepos : IRepository<Document>
    {
        private readonly PozitiveSqlContext _db;

        public DocumentRepos(PozitiveSqlContext db)
        {
            _db = db;
        }
        public void Add(Document item)
        {
            _db.Documents.Add(item);
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var doc = _db.Documents.Find(id);
            if(doc != null)
            {
                _db.Documents.Remove(doc);
                _db.SaveChanges();
            }
        }

        public void Dispose()
        {
            //_db.Dispose();
        }

        public IEnumerable<Document> GetAll()
        {
            return _db.Documents;
        }

        public Document GetItem(int id)
        {
            return _db.Documents.Find(id);
        }

        public void Update(Document item)
        {
            _db.Documents.Update(item);
            _db.SaveChanges();
        }
    }
}
