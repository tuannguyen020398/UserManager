using BE.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BE.DAL.EF
{
    public class SystemDbContext:DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public SystemDbContext(DbContextOptions<SystemDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>(x =>
            {
                x.ToTable("Users");
                x.HasKey(x => x.Id);
                x.Property(x => x.UserName).IsRequired().HasMaxLength(200);
                x.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(20).IsUnicode(false);
            });
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<User> Users { get; set; }
    }
}
