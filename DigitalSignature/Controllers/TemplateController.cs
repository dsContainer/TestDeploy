﻿using Digital.Infrastructure.Interface;
using Digital.Infrastructure.Model;
using Digital.Infrastructure.Model.TemplateModel;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
        public async Task<ActionResult> GetTemplate()
        {
            var result = await _service.GetTemplate();

            if (result.IsSuccess && result.Code == 200) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// Create Template
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultModel))]
        [HttpPost("Template")]
        public async Task<IActionResult> UploadTemplate([FromForm] TemplateCreateModel model, Guid documentTypeId)
        {
            var result = await _service.UploadTemplate(model, documentTypeId);

            if (result.IsSuccess && result.Code == 200) return Ok(result);
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

            if (result.IsSuccess && result.Code == 200) return Ok(result);
            return BadRequest(result);
        }


        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultModel))]
        [HttpPut("{id}/{isDeleted}")]
        public async Task<IActionResult> ChangeStatus(Guid id, bool isDeleted)
        {
            var result = await _service.ChangeStatus(id, isDeleted);

            if (result.IsSuccess && result.Code == 200) return Ok(result);
            return BadRequest(result);
        }



        /// <summary>
        /// Update template
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultModel))]
        [HttpPut]
        public async Task<IActionResult> UpdateTemplate(Guid id, [FromForm] TemplateModel model)
        {
            var result = await _service.UpdateTemplate(id, model);

            if (result.IsSuccess && result.Code == 200) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// delete tmp
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteTemp([Required] Guid id)
        {
            var result = await _service.DeleteTemplate(id);

            if (result.IsSuccess && result.Code == 200) return Ok(result);
            return BadRequest(result);
        }
    }
}
