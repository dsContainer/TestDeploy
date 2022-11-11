using DigitalSignature.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DigitalSignature.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessStepsController : ControllerBase
    {
        private readonly DigitalSignatureDBContext _context;

        public ProcessStepsController(DigitalSignatureDBContext context)
        {
            _context = context;
        }

        // GET: api/ProcessSteps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProcessStep>>> GetProcessSteps()
        {
            return await _context.ProcessSteps.ToListAsync();
        }

        // GET: api/ProcessSteps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProcessStep>> GetProcessStep(Guid id)
        {
            var processStep = await _context.ProcessSteps.FindAsync(id);

            if (processStep == null)
            {
                return NotFound();
            }

            return processStep;
        }

        // PUT: api/ProcessSteps/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProcessStep(Guid id, ProcessStep processStep)
        {
            if (id != processStep.Id)
            {
                return BadRequest();
            }

            _context.Entry(processStep).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProcessStepExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProcessSteps
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProcessStep>> PostProcessStep(ProcessStep processStep)
        {
            _context.ProcessSteps.Add(processStep);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProcessStepExists(processStep.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProcessStep", new { id = processStep.Id }, processStep);
        }

        // DELETE: api/ProcessSteps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProcessStep(Guid id)
        {
            var processStep = await _context.ProcessSteps.FindAsync(id);
            if (processStep == null)
            {
                return NotFound();
            }

            _context.ProcessSteps.Remove(processStep);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProcessStepExists(Guid id)
        {
            return _context.ProcessSteps.Any(e => e.Id == id);
        }
    }
}
