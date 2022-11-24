using ApplicationService.Model.UserModel;
using AutoMapper;
using BE.DAL.EF;
using BE.DAL.Entities;
using BE.DAL.Repository.UserRepository;
using BE.DAL.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ApplicationService.Catalog.Users
{
    public class UserService : IUserService
    {
        private readonly SystemDbContext _context;
        private readonly IUserRepositories _userRepositories;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<SystemDbContext> _unitOfWork;
        private Logger _logger = new Log().GetLogger();

        public UserService(SystemDbContext context, IUserRepositories userRepositories, IMapper mapper, IUnitOfWork<SystemDbContext> unitOfWork)
        {
            _context = context;
            _userRepositories = userRepositories;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            
        }

        public async Task<long> Create(CreateUserModel request)
        {
            User result = new User();
            try
            {
                result = _mapper.Map<CreateUserModel,User>(request,result);
                await _userRepositories.AddAsync(result);
                await BuildUrl(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return result.Id;
        }
        private async Task<User> BuildUrl(User obj)
        {
            string pass = obj.PasswordHash;
            obj.PasswordHash = Encrypt.encryption(pass);
            await _unitOfWork.SaveChangesAsync();
            return obj;
        }
        public async Task<List<ViewUserModel>> GetAll()
        {
            var query = from c in _context.Users
                        select new { c };
            var data = await query.Select(x => new ViewUserModel()
            {
                Id = x.c.Id,
                Name=x.c.Name,
                Dob=x.c.Dob,  
                Email=x.c.Email,
                Gt=x.c.Gt,
                PhoneNumber=x.c.PhoneNumber,
            }).ToListAsync();
            //var data = _userRepositories.GetAllAsync();
            return data;
        }

        public Task<User> GetByid(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Remove(long id)
        {
            bool check = false;
            try
            {
                var result = _userRepositories.GetById(id);
                _userRepositories.Remove(result);
                await _unitOfWork.SaveChangesAsync();
                check = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return check;
        }

        public Task<long> Update(UpdateUserModel request)
        {
            throw new NotImplementedException();
        }
    }
}
