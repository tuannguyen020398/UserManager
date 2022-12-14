using AutoMapper;
using BE.DAL.BaserRepository;
using BE.DAL.EF;
using BE.DAL.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.DAL.Repository.UserRepository
{
    /// <summary>phương thức reponsitories user</summary>
    /// <Modified>
    /// Name Date Comments
    /// tuannx 12/1/2022 created
    /// </Modified>
    public class UserRepositories : GenericRepository<User,SystemDbContext>,IUserRepositories
    {
        public UserRepositories(SystemDbContext context,IMapper mapper) : base(context, mapper) 
        { }
        private SystemDbContext SystemDbContext
        {
            get { return Context as SystemDbContext; }
        }

        public SystemDbContext GetDbContext()
        {
            return this.SystemDbContext;
        }

    }
}
