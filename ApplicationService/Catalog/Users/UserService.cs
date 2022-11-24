using ApplicationService.Common;
using ApplicationService.Model.UserModel;
using ApplicationService.Resource;
using AutoMapper;
using BE.DAL.EF;
using BE.DAL.Entities;
using BE.DAL.ModelPages;
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
using Utilities.Exceptions;

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

        public async Task<ApiResult<bool>> Create(CreateUserModel request)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == request.Email);
            if (user != null) return new ApiErrorResult<bool>("Email đã tồn tại");
            var userphone = _context.Users.FirstOrDefault(x => x.PhoneNumber == request.PhoneNumber);
            if (userphone != null) return new ApiErrorResult<bool>("Phonenumber đã tồn tại");
            User result = new User();
            try
            {
                result = _mapper.Map<CreateUserModel, User>(request, result);
                await _userRepositories.AddAsync(result);
                await BuildUrl(result);               
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return new ApiAccountSuccessResult<bool>();
        }
        public async Task<ApiResult<bool>> Update(long id,UpdateUserModel request)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == request.Email);
            if (user != null) return new ApiErrorResult<bool>("Email đã tồn tại");
            var userphone = _context.Users.FirstOrDefault(x => x.PhoneNumber == request.PhoneNumber);
            if (userphone != null) return new ApiErrorResult<bool>("Phonenumber đã tồn tại");
            try
            {
                var Itemid = _userRepositories.GetById(id);
                Itemid.Name = request.Name;
                Itemid.Dob = request.Dob;
                Itemid.PhoneNumber = request.PhoneNumber;
                Itemid.Email = request.Email;
                Itemid.Gt = request.Gt;
                await _unitOfWork.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return new ApiAccountSuccessResult<bool>();
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
                if (result == null) throw new ManagerException($"cannot find a user:{id}");
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

        public async Task<FilterResult<UserModelPading>> GetPading(FilterResource filterResource)
        {
            var query = from u in _context.Users
                        select new {u};
            int totalRow = await query.CountAsync();
            var data = await query.DefaultIfEmpty()
                .Select(x => new UserModelPading()
                {
                    Id = x.u.Id,
                    Name=x.u.Name,
                    Dob=x.u.Dob,
                    Email=x.u.Email,
                    Gt=x.u.Gt,
                    PhoneNumber=x.u.PhoneNumber,
                    UserName=x.u.UserName
                    
                }).ToListAsync();
            var result = new FilterResult<UserModelPading>()
            {
                TotalRows=totalRow,
                PageIndex = filterResource.PageIndex,
                PageSize = filterResource.PageSize,
                ItemsData = data
            };

            return result;
        }
    }
}
