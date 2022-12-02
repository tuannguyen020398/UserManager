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
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NLog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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
        private readonly IConfiguration _config;
        private readonly IUnitOfWork<SystemDbContext> _unitOfWork;
        private Logger _logger = new Log().GetLogger();

        public UserService(SystemDbContext context, IUserRepositories userRepositories, IMapper mapper, IUnitOfWork<SystemDbContext> unitOfWork, IConfiguration config)
        {
            _context = context;
            _userRepositories = userRepositories;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _config = config;
        }

        /// <summary>phương thức tạo mới một user</summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// tuannx 12/1/2022 created
        /// </Modified>
        public async Task<ApiResult<bool>> Create(CreateUserModel request)
        {
            //check email
            var user = _context.Users.FirstOrDefault(x => x.Email == request.Email);
            if (user != null) return new ApiErrorResult<bool>("Email đã tồn tại");
            //check phonenumber
            var userphone = _context.Users.FirstOrDefault(x => x.PhoneNumber == request.PhoneNumber);
            if (userphone != null) return new ApiErrorResult<bool>("Phonenumber đã tồn tại");
            User result = new User();
            try
            {
                //mapping dữ liệu
                result = _mapper.Map<CreateUserModel, User>(request, result);
                //save phương thức
                await _userRepositories.AddAsync(result);
                await BuildUrl(result);               
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return new ApiAccountSuccessResult<bool>();
        }
        /// <summary>phương thức cập nhật user</summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// tuannx 12/1/2022 created
        /// </Modified>
        public async Task<ApiResult<bool>> Update(UpdateUserModel request)
        {
            //var user = _context.Users.FirstOrDefault(x => x.Email == request.Email);
            //if (user != null) return new ApiErrorResult<bool>("Email đã tồn tại");
            //var userphone = _context.Users.FirstOrDefault(x => x.PhoneNumber == request.PhoneNumber);
            //if (userphone != null) return new ApiErrorResult<bool>("Phonenumber đã tồn tại");
            try
            {
                var Itemid = _userRepositories.GetById(request.Id);
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
        /// <summary>save phương thức</summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// tuannx 12/1/2022 created
        /// </Modified>
        private async Task<User> BuildUrl(User obj)
        {
            //mã hóa mật khẩu Md5
            string pass = obj.PasswordHash;
            obj.PasswordHash = Encrypt.encryption(pass);
            // save thuộc tính
            await _unitOfWork.SaveChangesAsync();
            return obj;
        }
        /// <summary>phương thức lấy ra danh sach người dùng</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// tuannx 12/1/2022 created
        /// </Modified>
        public async Task<List<ViewUserModel>> GetAll()
        {
            //query 
            var query = from c in _context.Users
                        select new { c };
            //select and projection
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

        /// <summary>phương thức lấy danh sách theo id</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// tuannx 12/1/2022 created
        /// </Modified>
        public async Task<User> GetByid(long id)
        {
            //khởi tạo một new user
            var req = new User();
            try
            {
                //lây user theo id
                var reqbyid = await _userRepositories.GetByIdAsync(id);
                //mapping dữ liệu
                req = _mapper.Map<User, User>(reqbyid);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return req;
        }

        /// <summary>xóa người dùng theo id</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <exception cref="Utilities.Exceptions.ManagerException">cannot find a user:{id}</exception>
        /// <Modified>
        /// Name Date Comments
        /// tuannx 12/1/2022 created
        /// </Modified>
        public async Task<bool> Remove(long id)
        {
            bool check = false;
            try
            {
                //lấy user theo id
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

        /// <summary>phương thức lấy danh sách</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// tuannx 12/1/2022 created
        /// </Modified>
        public async Task<FilterResult<UserModelPading>> GetPading(FilterResource filterResource)
        {
            // query 
            var query = from u in _context.Users
                        select new {u};
            int totalRow = await query.CountAsync();
            // select and projection
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

        /// <summary>phương thức xác thực login</summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// tuannx 12/1/2022 created
        /// </Modified>
        public async Task<ApiResult<string>> Authencate(LoginRequest request)
        {
            //checkemail
            var user = _context.Users.SingleOrDefault(x => x.Email == request.Email);
            if (user == null) return new ApiErrorResult<string>("Tài khoản không tồn tại");
            //var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            string passWithSalt = Encrypt.encryption(request.Password);
            //check pass
            if (!passWithSalt.Equals(user.PasswordHash))
            {
                return new ApiErrorResult<string>("Mật khẩu không đúng");
            }
            // khởi tạo token
            var claims = new[]
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.Name),
                new Claim(ClaimTypes.Name, request.Email),

            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(48),
                signingCredentials: creds);

            return new ApiAccountSuccessResult<string>(new JwtSecurityTokenHandler().WriteToken(token), user.Id);
        }

        /// <summary>phương thức lấy danh sách theo keywwork ,theo thuộc tính</summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// tuannx 12/1/2022 created
        /// </Modified>
        public async Task<FilterResult<UserModelPading>> GetkeyworkPading(FilterUserResource request)
        {
            var result = new FilterResult<UserModelPading>();
            try
            {
                //filter
                var req = _userRepositories.Filter<UserModelPading>(request);               
                result = req;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            //select and projection
            var results = new FilterResult<UserModelPading>()
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                ItemsData = result.Data.ToList()
            };
            return results;

        }
    }
}
