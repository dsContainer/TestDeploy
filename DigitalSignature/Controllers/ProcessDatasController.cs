using Digital.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DigitalSignature.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProcessDatasController : ControllerBase
    {
        private readonly DigitalSignatureDBContext _context;

        public ProcessDatasController(DigitalSignatureDBContext context)
        {
            _context = context;
        }

        // GET: api/ProcessDatas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProcessData>>> GetProcessDatas()
        {
            return await _context.ProcessDatas.ToListAsync();
        }

        // GET: api/ProcessDatas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProcessData>> GetProcessData(Guid id)
        {
            var processData = await _context.ProcessDatas.FindAsync(id);

            if (processData == null)
            {
                return NotFound();
            }

            return processData;
        }

        // PUT: api/ProcessDatas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProcessData(Guid id, ProcessData processData)
        {
            if (id != processData.Id)
            {
                return BadRequest();
            }

            _context.Entry(processData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProcessDataExists(id))
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

        // POST: api/ProcessDatas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProcessData>> PostProcessData(ProcessData processData)
        {
            _context.ProcessDatas.Add(processData);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProcessDataExists(processData.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProcessData", new { id = processData.Id }, processData);
        }

        // DELETE: api/ProcessDatas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProcessData(Guid id)
        {
            var processData = await _context.ProcessDatas.FindAsync(id);
            if (processData == null)
            {
                return NotFound();
            }

            _context.ProcessDatas.Remove(processData);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProcessDataExists(Guid id)
        {
            return _context.ProcessDatas.Any(e => e.Id == id);
        }
    }
}
