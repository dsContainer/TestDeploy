using Digital.Infrastructure.Interface;
using Digital.Infrastructure.Model;
using Digital.Infrastructure.Model.TemplateModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalSignature.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class TemplateController : Controller
    {
        private readonly ITemplateService _service;
        public TemplateController(ITemplateService service)
        {
            _service = service;
        }


        /// <summary>
        /// Get list Template
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultModel))]
        [HttpGet("Templates")]
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
        [HttpPost("Template")]
        public async Task<IActionResult> UploadTemplate([FromForm] TemplateModel model, Guid documentTypeId)
        {
            var result = await _service.UploadTemplate(model, documentTypeId);

            if (result.IsSuccess && result.Code == 200) return Ok(result.ResponseSuccess);
            return BadRequest(result);
        }

        /// <summary>
        /// Get Template By Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultModel))]
        [HttpGet("Template/{id}")]
        public async Task<IActionResult> GetTemplateById(Guid id)
        {
            var result = await _service.GetTemplateById(id);

            if (result.IsSuccess && result.Code == 200) return Ok(result.ResponseSuccess);
            return BadRequest(result);
        }

        /// <summary>
        /// Change Status
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultModel))]
        [HttpPut("{id}/{isDeleted}")]
        public async Task<IActionResult> ChangeStatus(string data, Guid templateId)
        {
            var result = await _service.ChangeStatus( data,  templateId);

            if (result.IsSuccess && result.Code == 200) return Ok(result.ResponseSuccess);
            return BadRequest(result);
        }

        
    }
}
