using ApplicationService.Model.UserModel;
using BE.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.Catalog.Users
{
    public interface IUserService
    {
        Task<long> Create(CreateUserModel request);
        Task<List<ViewUserModel>> GetAll();
        Task<long> Update(UpdateUserModel request);
        Task<User> GetByid(long id);
        Task<bool> Remove(long id);
    }
}
