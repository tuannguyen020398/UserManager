using ApplicationService.Catalog.Users;
using ApplicationService.Model.UserModel;
using ApplicationService.Resource;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationManageUser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            this._userService = userService;
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
        public async Task<IActionResult> GetAllPaging([FromQuery] FilterResource request)
        {
            var result = await _userService.GetPading(request);
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromForm] UpdateUserModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var affectedResult = await _userService.Update(id,request);
            return Ok(affectedResult);
        }



    }
}
