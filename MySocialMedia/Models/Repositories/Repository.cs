﻿using Microsoft.EntityFrameworkCore;
using MySocialMedia.Models.UoW;

namespace MySocialMedia.Models.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DbContext _db;

        public  DbSet<T> Set
        {
            get;
            private set;
        }

        public   Repository(ApplicationDbContext db)
        {
            _db = db;
            var set = _db.Set<T>();
            set.Load();

            Set = set;
        }

        public async Task Create(T item)
        {
            Set.Add(item);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(T item)
        {
            Set.Remove(item);
            await _db.SaveChangesAsync();
        }

        public async Task <T> Get(int id)
        {
            return await Set.FindAsync(id);
        }

        public async Task <IEnumerable<T>> GetAll()  
        {
            return await _db.Set<T>().ToListAsync();
        }

        public async Task Update(T item)
        {
            Set.Update(item);
            await _db.SaveChangesAsync();
        }
    }
}
