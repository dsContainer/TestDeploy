using Digital.Infrastructure.Interface;
using Digital.Infrastructure.Model.DocumentModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Digital_BE.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentTypeController : ControllerBase
    {
        private readonly IDocumentTypeService _service;

        public DocumentTypeController(IDocumentTypeService service)
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
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var result = await _service.DeleteDocumentType(Id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }


        /// <summary>
        /// update DocType
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPut("{Id}")]
        public async Task<IActionResult> Update(DocumentTypeUpdateModel model, Guid Id)
        {
            var result = await _service.UpdateDocumentType(model, Id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }
    }
}
