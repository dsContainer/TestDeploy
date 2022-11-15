using Digital.Infrastructure.Interface;
using Digital.Infrastructure.Model.ProcessModel;
using Microsoft.AspNetCore.Mvc;

namespace DigitalSignature.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class ProcessesController : ControllerBase
    {
        private readonly IProcessService _service;

        public ProcessesController(IProcessService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create a new process
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateProcess(ProcessCreateModel model)
        {
            var result = await _service.CreateProcess(model);

            if (result.IsSuccess && result.Code == 200) return Ok(result.ResponseSuccess);
            return BadRequest(result);
        }

        /// <summary>
        /// Get a process by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetProcessById(Guid Id)
        {
            if (Id != null)
            {
                var result = await _service.GetProcessById(Id);
                return Ok(result);
            }
            return NotFound();
        }

        /// <summary>
        /// Get all processes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetProcesses([FromQuery] ProcessSearchModel searchModel)
        {
            var result = await _service.GetProcesses(searchModel);
            if (result.Code == 200)
                return Ok(result);
            else if (result.Code == 404)
                return NotFound(result);
            return BadRequest(result);
        }

        /// <summary>
        /// Delete process
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPut("{Id}/{isDeleted}")]
        public async Task<IActionResult> Delete(Guid Id, bool isDeleted)
        {
            var result = await _service.DeleteProcess(Id, isDeleted);
            if (result > 0)
            {
                return Ok(result);
            }
            return NotFound();
        }


        /// <summary>
        /// Update process
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update(ProcessUpdateModel model)
        {
            var result = await _service.UpdateProcess(model);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }
    }
}
