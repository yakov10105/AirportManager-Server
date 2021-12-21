using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Server.DAL.Repo
{
    public class Repository<TModel> : IRepository<TModel> where TModel : class, new()
    {
        private readonly AirportDbContext _airportDbContext;
        public Repository(AirportDbContext airportDbContext)
        {
            _airportDbContext = airportDbContext;
        }

        public virtual async Task<TModel> AddAsync(TModel entity)
        {
            if(entity == null)
            {
                throw new ArgumentNullException("Entity must not be null .");
            }
            try
            {
               EntityEntry<TModel> entry = await _airportDbContext.AddAsync(entity);
               await _airportDbContext.SaveChangesAsync();
                return entry.Entity;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IQueryable<TModel> GetAll()
        {
            try
            {
                return _airportDbContext.Set<TModel>();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<TModel> UpdateAsync(TModel entity)
        {
            if(entity != null)
            {
                try
                {
                    _airportDbContext.Update(entity);
                    await _airportDbContext.SaveChangesAsync();
                    return entity;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("==================Update Error=======================");
                    throw;
                }
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
    }
}
