using Digital.Infrastructure.Interface;
using Digital.Infrastructure.Model;
using Digital.Infrastructure.Model.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace DigitalSignature.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("Users")]
        public async Task<ActionResult> GetUsers()
        {
            try
            {
                var users = _userService.GetUsers();

                return await Task.FromResult(StatusCode(StatusCodes.Status200OK, new ResultModel() { IsSuccess = true, Code = StatusCodes.Status200OK, ResponseSuccess = users }));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status400BadRequest, new ResultModel() { IsSuccess = false, Code = StatusCodes.Status400BadRequest, ResponseFailed = ex.Message }));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetUser(Guid id)
        {
            try
            {
                var user = _userService.GetUser(id);

                if (user == null) return await Task.FromResult(StatusCode(StatusCodes.Status404NotFound, new ResultModel() { IsSuccess = true, Code = StatusCodes.Status404NotFound, ResponseFailed = "Not found User" }));

                return await Task.FromResult(StatusCode(StatusCodes.Status200OK, new ResultModel() { IsSuccess = true, Code = StatusCodes.Status200OK, ResponseSuccess = user }));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status400BadRequest, new ResultModel() { IsSuccess = false, Code = StatusCodes.Status400BadRequest, ResponseFailed = ex.Message }));
            }
        }

        [HttpPost()]
        public async Task<ActionResult> PostUser([FromBody] UserRequest userRequest)
        {
            try
            {
                var user = _userService.CreateUser(userRequest);

                return await Task.FromResult(StatusCode(StatusCodes.Status201Created, new ResultModel() { IsSuccess = true, Code = StatusCodes.Status201Created, ResponseSuccess = user }));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status400BadRequest, new ResultModel() { IsSuccess = false, Code = StatusCodes.Status400BadRequest, ResponseFailed = ex.Message }));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutUser(Guid id, [FromBody] UserRequest userRequest)
        {
            try
            {
                var user = _userService.UpdateUser(id, userRequest);

                if (user == null) return await Task.FromResult(StatusCode(StatusCodes.Status404NotFound, new ResultModel() { IsSuccess = true, Code = StatusCodes.Status404NotFound, ResponseFailed = "Not found User" }));

                return await Task.FromResult(StatusCode(StatusCodes.Status200OK, new ResultModel() { IsSuccess = true, Code = StatusCodes.Status200OK, ResponseSuccess = user }));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status400BadRequest, new ResultModel() { IsSuccess = false, Code = StatusCodes.Status400BadRequest, ResponseFailed = ex.Message }));
            }
        }

        [HttpPut("{id}/{isDeleted}")]
        public async Task<ActionResult> PutDeletedUser(Guid id, bool isDeleted)
        {
            try
            {
                var user = _userService.DeletedUser(id, isDeleted);

                if (user == null) return await Task.FromResult(StatusCode(StatusCodes.Status404NotFound, new ResultModel() { IsSuccess = true, Code = StatusCodes.Status404NotFound, ResponseFailed = "Not found User" }));

                return await Task.FromResult(StatusCode(StatusCodes.Status200OK, new ResultModel() { IsSuccess = true, Code = StatusCodes.Status200OK, ResponseSuccess = user }));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status400BadRequest, new ResultModel() { IsSuccess = false, Code = StatusCodes.Status400BadRequest, ResponseFailed = ex.Message }));
            }
        }
    }
}
