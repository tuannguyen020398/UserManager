using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using BE.DAL.Utility;

namespace BE.DAL.EF
{
    public interface IUnitOfWork<TContext> where TContext : DbContext
    {
        Task<int> SaveChangesAsync();
        TContext GetDbContext();
    }
    public interface IDbFactory<TContext> : IDisposable where TContext : DbContext
    {
        TContext Init();
    }
    public class UnitOfWork<TContext> : IDisposable, IUnitOfWork<TContext> where TContext : DbContext
    {
        //private readonly ILogger _logger;
        private Logger _logger = new Log().GetLogger();
        private TContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public UnitOfWork(TContext context)
        {
            
            this._context = context;
            //_logger = logger;
        }

        public TContext GetDbContext()
        {
            return this._context;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw;
            }
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
