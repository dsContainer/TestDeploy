using DigitalSignature.Interface;
using DigitalSignature.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DigitalSignature.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessStepController : ControllerBase
    {
        private readonly IProcessStepService _service;

        public ProcessStepController(IProcessStepService service)
        {
            _service = service;
        }

        /// <summary>
        /// get a process step by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetProcessStepById(Guid Id)
        {
            if (Id != null)
            {
                var result = await _service.GetProcessStepById(Id);
                return Ok(result);
            }
            return NotFound();
        }

        /// <summary>
        /// delete process step
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("{Id}/{isDeleted}")]
        public async Task<IActionResult> Delete(Guid Id, bool isDeleted)
        {
            var result = await _service.DeleteProcessStep(Id, isDeleted);
            if (result > 0)
            {
                return Ok(result);
            }
            return NotFound();
        }


        /// <summary>
        /// update process step
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update(ProcessStepUpdateModel model)
        {
            var result = await _service.UpdateProcessStep(model);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }
    }
}
