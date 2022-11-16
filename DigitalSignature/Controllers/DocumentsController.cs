using Digital.Data.Enums;
using Digital.Data.Utilities.Paging.PaginationModel;
using Digital.Infrastructure.Interface;
using Digital.Infrastructure.Model.DocumentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DigitalSignature.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentService _service;

        public DocumentsController(IDocumentService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create new Document by current user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateDoccument([FromForm] DocumentUploadApiRequest model)
        {
            var result = await _service.CreateAsync(model);
            if (result.IsSuccess && result.Code == 200) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// get all Document 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAll")]
        public async Task<IActionResult> GetDocument()
        {
            var result = await _service.GetDocAsync();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        /// <summary>
        /// get all Document with paging with Current User
        /// </summary>
        /// <returns></returns>
        [HttpGet("Paging")]
        public async Task<IActionResult> GetPagingDocument([FromQuery]PagingParam<DocumentSortCriteria> paginationModel)
        {
            var result = await _service.GetPagingDocument(paginationModel);

            if (result.IsSuccess && result.Code == 200) return Ok(result);
            return BadRequest(result);
        }
        /// <summary>
        /// search  Document by fileName
        /// </summary>
        /// <returns></returns>
        [HttpGet("search")]
        public async Task<IActionResult> SearchDocbyName(string textSearch)
        {
            var result = await _service.SearchDocbyName(textSearch);

            if (result.IsSuccess && result.Code == 200) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// delete doc
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteDoc([Required] Guid id)
        {
            var result = await _service.DeleteDocument(id);

            if (result.IsSuccess && result.Code == 200) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// update Doc
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPut("{Id}")]

        public async Task<IActionResult> UpdateDoc(DocumentUpdateModel model, Guid Id)
        {
            var result = await _service.UpdateDocument(model, Id);

            if (result.IsSuccess && result.Code == 200) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// get a Document detail by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetDocumentDetail(Guid Id)
        {
            var result = await _service.GetDocumentDetail(Id);
            if (result.IsSuccess && result.Code == 200) return Ok(result);
            return BadRequest(result);
        }


        /// get a Document detail by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("content/{Id}")]
        public async Task<IActionResult> GetContent(Guid Id)
        {
            DocumentResponse? file = await _service.GetContent(Id);

            // Check if file was found
            if (file == null)
            {
                // Was not, return error message to client
                return StatusCode(StatusCodes.Status500InternalServerError, $"File {Id} could not be downloaded.");
            }
            else
            {
                // File was found, return it to client
                return File(file.Content, file.ContentType, file.Name);
            }
        }
    }
}
