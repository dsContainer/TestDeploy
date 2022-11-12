using DigitalSignature.Interface;
using DigitalSignature.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DigitalSignature.Model.Requests;
using DigitalSignature.Model.SignatureModel;

namespace DigitalSignature.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class SignatureController : ControllerBase
    {
        private readonly ISignatureService _service;

        public SignatureController(ISignatureService service)
        {
            _service = service;
        }


        /// <summary>
        /// Get list HSM from user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultModel))]
        [HttpGet("GetListSignature")]
        public async Task<IActionResult> GetListSignature()
        {
            var result = await _service.GetListSignature();

            if (result.IsSuccess && result.Code == 200) return Ok(result.ResponseSuccess);
            return BadRequest(result);
        }

        /// <summary>
        /// Gen Certificate
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultModel))]
        [HttpPost("GenCer")]
        public async Task<IActionResult> CreateSignatureByUserId(Guid userId)
        {
            var result = await _service.CreateSignatureByUserId(userId);

            if (result.IsSuccess && result.Code == 200) return Ok(result.ResponseSuccess);
            return BadRequest(result);
        }

        /// <summary>
        /// Search signature contain username or email or phone
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultModel))]
        [HttpGet("SearchContainData")]
        public async Task<IActionResult> SearchContainUserNamePhoneOrEmail(string data)
        {
            var result = await _service.SearchContainUserNamePhoneOrEmail(data);

            if (result.IsSuccess && result.Code == 200) return Ok(result.ResponseSuccess);
            return BadRequest(result);
        }

        /// <summary>
        /// Search signature by sigId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultModel))]
        [HttpGet("SearchByid")]
        public async Task<IActionResult> SearchBySignatureId(Guid sigId)
        {
            var result = await _service.SearchBySignatureId(sigId);

            if (result.IsSuccess && result.Code == 200) return Ok(result.ResponseSuccess);
            return BadRequest(result);
        }

        /// <summary>
        /// Filter signature in range date is active
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultModel))]
        [HttpGet("SearchRangeDate")]
        public async Task<IActionResult> SearchRangeDate(string fromDate, string toDate)
        {
            var result = await _service.SearchRangeDate(fromDate, toDate);

            if (result.IsSuccess && result.Code == 200) return Ok(result.ResponseSuccess);
            return BadRequest(result);
        }

        /// <summary>
        /// Sign Pdf
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultModel))]
        [HttpPost("SignPdf")]
        public async Task<IActionResult> SignPDF( SignModel signModel)
        {
            var result = await _service.SignPDF(signModel);

            if (result.IsSuccess && result.Code == 200) return Ok(result.ResponseSuccess);
            return BadRequest(result);
        }
    }
}
