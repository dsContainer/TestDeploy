using Digital.Infrastructure.Interface;
using Digital.Infrastructure.Model;
using Digital.Infrastructure.Model.SignatureModel;
using Digital.Infrastructure.Model.TemplateModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalSignature.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TemplateController : Controller
    {
        private readonly ITemplateService _service;
        public TemplateController(ITemplateService service)
        {
            _service = service ;
        }


        /// <summary>
        /// Get list Template
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultModel))]
        [HttpGet("GetTemplate")]
        public async Task<IActionResult> GetTemplate()
        {
            var result = await _service.GetTemplate();

            if (result.IsSuccess && result.Code == 200) return Ok(result.ResponseSuccess);
            return BadRequest(result);
        }

        /// <summary>
        /// Upload Template
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultModel))]
        [HttpPost("UploadTemplate")]
        public async Task<IActionResult> UploadTemplate([FromForm] TemplateModel model, Guid documentTypeId)
        {
            var result = await _service.UploadTemplate(model, documentTypeId);

            if (result.IsSuccess && result.Code == 200) return Ok(result.ResponseSuccess);
            return BadRequest(result);
        }
    }
}
