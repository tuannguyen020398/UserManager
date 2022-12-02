using ApplicationService.Catalog.Users;
using ApplicationService.Model.UserModel;
using ApplicationService.Resource;
using BE.DAL.Entities;
using BE.DAL.ModelPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationManageUser.Controllers
{

    /// <summary><br /></summary>
    /// <Modified>
    /// Name Date Comments
    /// tuannx 11/25/2022 created
    /// </Modified>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            this._userService = userService;
        }
        [HttpPost("authenticate")]
        //[AllowAnonymous]
        //[Authorize]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.Authencate(request);

            //if (string.IsNullOrEmpty(result.ResultObj))
            //{
            //    return BadRequest(result);
            //}

            return Ok(result);
        }
        /// <summary>lấy danh sách user</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// tuannx 12/1/2022 created
        /// </Modified>
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var result = await _userService.GetAll();
            return Ok(result);

        }
        /// <summary>thêm mới user</summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// tuannx 12/1/2022 created
        /// </Modified>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userid = await _userService.Create(request);
            return Ok(userid);
        }
        /// <summary>Xóa user</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// tuannx 12/1/2022 created
        /// </Modified>
        [HttpDelete]
        public async Task<ActionResult<long>> Delete(long id)
        {
            await _userService.Remove(id);
            return Ok(id);
        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromBody] FilterResource request)
        {
            var result = await _userService.GetPading(request);
            return Ok(result);
        }
        /// <summary>Cập nhật thông tin user</summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// tuannx 12/1/2022 created
        /// </Modified>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var affectedResult = await _userService.Update(request);
            return Ok(affectedResult);
        }
        /// <summary>lấy thuộc tính user theo id</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// tuannx 12/1/2022 created
        /// </Modified>
        [HttpGet("id")]
        public async Task<User> GetByid(long id)
        {
            var reqs =await _userService.GetByid(id);
            return reqs;
        }
        /// <summary>lấy dánh sách user theo keywwork và giới tính</summary>
        /// <param name="filter">The filter.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// tuannx 12/1/2022 created
        /// </Modified>
        [HttpGet("keywork")]
        //[BnDAuthorize(ModuleEnum.Event, SystemPermissions.Allow)]
        public ActionResult<FilterResult<UserModelPading>> GetAll([FromQuery] FilterUserResource filter)
        {
            var res = _userService.GetkeyworkPading(filter);
            return Ok(res);
        }


    }
}
