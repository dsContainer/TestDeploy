using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using Digital.Infrastructure.Interface;
using Digital.Infrastructure.Model;
using Digital.Infrastructure.Model.DocumentModel;
using Digital.Infrastructure.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalSignature.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class DocumentTypesController : ControllerBase
    {
        private readonly IDocumentTypeService _service;

        public DocumentTypesController(IDocumentTypeService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create a new Document Type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateDoccument(DocumentTypeCreateModel model)
        {
            var result = await _service.CreateDocumentType(model);

            if (result.IsSuccess && result.Code == 200) return Ok(result.ResponseSuccess);
            return BadRequest(result);
        }

        /// <summary>
        /// get a Document Type by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetDocumentTypeById(Guid Id)
        {
            if (Id != null)
            {
                var result = await _service.GetDocumentTypeById(Id);
                return Ok(result);
            }
            return NotFound();
        }

        /// <summary>
        /// get all Document Type
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAll")]
        public async Task<IActionResult> GetDocumentTypes()
        {
            var result = await _service.GetDocumentTypes();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        /// <summary>
        /// delete DocType
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoc([Required] Guid id)
        {
            var result = await _service.DeleteDocumentType(id);

            if (result.IsSuccess && result.Code == 200) return Ok(result.ResponseSuccess);
            return BadRequest(result);
        }


        /// <summary>
        /// update DocType
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPut("{Id}")]
        public async Task<IActionResult> Update(Guid Id, [FromQuery] DocumentTypeUpdateModel model)
        {
            var result = await _service.UpdateDocumentType(Id, model);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpPut("{id}/{isDeleted}")]
        public async Task<ActionResult> PutDeletedDocument(Guid id, bool isDeleted)
        {
            try
            {
                var data = _service.DeletedDocument(id, isDeleted);

                if (data == null) return await Task.FromResult(StatusCode(StatusCodes.Status404NotFound, new ResultModel() { IsSuccess = true, Code = StatusCodes.Status404NotFound, ResponseFailed = "Not found Document" }));

                return await Task.FromResult(StatusCode(StatusCodes.Status200OK, new ResultModel() { IsSuccess = true, Code = StatusCodes.Status200OK, ResponseSuccess = data }));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status400BadRequest, new ResultModel() { IsSuccess = false, Code = StatusCodes.Status400BadRequest, ResponseFailed = ex.Message }));
            }
        }
    }
}
