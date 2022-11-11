using DigitalSignature.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DigitalSignature.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignaturesController : ControllerBase
    {
        private readonly DigitalSignatureDBContext _context;

        public SignaturesController(DigitalSignatureDBContext context)
        {
            _context = context;
        }

        // GET: api/Signatures
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Signature>>> GetSignatures()
        {
            return await _context.Signatures.ToListAsync();
        }

        // GET: api/Signatures/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Signature>> GetSignature(Guid id)
        {
            var signature = await _context.Signatures.FindAsync(id);

            if (signature == null)
            {
                return NotFound();
            }

            return signature;
        }

        // PUT: api/Signatures/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSignature(Guid id, Signature signature)
        {
            if (id != signature.Id)
            {
                return BadRequest();
            }

            _context.Entry(signature).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SignatureExists(id))
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

        // POST: api/Signatures
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Signature>> PostSignature(Signature signature)
        {
            _context.Signatures.Add(signature);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SignatureExists(signature.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSignature", new { id = signature.Id }, signature);
        }

        // DELETE: api/Signatures/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSignature(Guid id)
        {
            var signature = await _context.Signatures.FindAsync(id);
            if (signature == null)
            {
                return NotFound();
            }

            _context.Signatures.Remove(signature);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SignatureExists(Guid id)
        {
            return _context.Signatures.Any(e => e.Id == id);
        }
    }
}
