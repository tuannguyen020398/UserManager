using ApplicationService.Common;
using ApplicationService.Model.UserModel;
using ApplicationService.Resource;
using BE.DAL.Entities;
using BE.DAL.ModelPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.Catalog.Users
{
    public interface IUserService
    {
        Task<ApiResult<string>> Authencate(LoginRequest request);
        Task<ApiResult<bool>> Create(CreateUserModel request);
        Task<List<ViewUserModel>> GetAll();
        Task<FilterResult<UserModelPading>> GetPading(FilterResource filterResource);
        Task<ApiResult<bool>> Update(UpdateUserModel request);
        Task<User> GetByid(long id);
        Task<bool> Remove(long id);
        Task<FilterResult<UserModelPading>> GetkeyworkPading(FilterUserResource request);
    }
}
