using DigitalSignature.Interface;
using DigitalSignature.Model;
using DigitalSignature.Model.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalSignature.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IJwtTokenService _jwtTokenService;
        public LoginController(ILoginService loginService, IJwtTokenService jwtTokenService)
        {
            _loginService = loginService;
            _jwtTokenService = jwtTokenService;
        }

        [AllowAnonymous]
        [HttpPost()]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var user = _loginService.AuthenticateUser(loginRequest.UserName, loginRequest.Password);

            try
            {
                if (user != null)
                {
                    var tokenResponse = _jwtTokenService.GenerateTokenUser(user);

                    var response = new { token = tokenResponse, user };
                    return await Task.FromResult(StatusCode(StatusCodes.Status200OK, new ResultModel() { IsSuccess = true, Code = StatusCodes.Status200OK, ResponseSuccess = response }));
                }
                else
                {
                    return await Task.FromResult(StatusCode(StatusCodes.Status401Unauthorized, new ResultModel() { IsSuccess = false, Code = StatusCodes.Status401Unauthorized, ResponseSuccess = "User name or password incorrect" }));
                }
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status400BadRequest, new ResultModel() { IsSuccess = false, Code = StatusCodes.Status400BadRequest, ResponseFailed = ex.Message }));
            }
        }
    }
}
