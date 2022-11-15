using Digital.Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace DigitalSignature.Controllers
{
    [Route("api/OTP")]
    [Authorize]
    [ApiController]
    public class OTPController : ControllerBase
    {
        private readonly IOTPService _serOTP;

        public OTPController(IOTPService serOTP)
        {
            _serOTP = serOTP;
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail()
        {
            await _serOTP.SendEmailMessage();
            return StatusCode(204);
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmCode(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                await _serOTP.ConfirmCode(code);
                return StatusCode(201);

            }
            return BadRequest();
        }
    }
}
