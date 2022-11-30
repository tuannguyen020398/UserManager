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

    /// <summary>
    ///   <br />
    /// </summary>
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
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var result = await _userService.GetAll();
            return Ok(result);

        }
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
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var affectedResult = await _userService.Update(request);
            return Ok(affectedResult);
        }
        [HttpGet("id")]
        public async Task<User> GetByid(long id)
        {
            var reqs =await _userService.GetByid(id);
            return reqs;
        }
        [HttpGet("keywork")]
        //[BnDAuthorize(ModuleEnum.Event, SystemPermissions.Allow)]
        public ActionResult<FilterResult<UserModelPading>> GetAll([FromQuery] FilterUserResource filter)
        {
            var res = _userService.GetkeyworkPading(filter);
            return Ok(res);
        }


    }
}
