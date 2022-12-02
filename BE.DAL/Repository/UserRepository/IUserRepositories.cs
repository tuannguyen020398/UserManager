using BE.DAL.BaserRepository;
using BE.DAL.EF;
using BE.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.DAL.Repository.UserRepository
{
    /// <summary>interface phương thức reponsitory user</summary>
    /// <Modified>
    /// Name Date Comments
    /// tuannx 12/1/2022 created
    /// </Modified>
    public interface IUserRepositories : IGenericRepository<User, SystemDbContext>
    {
        SystemDbContext GetDbContext();
    }
}
