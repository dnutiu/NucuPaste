using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NucuPaste.Data;
using NucuPaste.Models;

namespace NucuPaste.Controllers
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiVersion( "1" )]
    [Route( "api/v{version:apiVersion}/[controller]")]
    public class PastesController : ControllerBase
    {
        private readonly PasteDbContext _context;
        private readonly ILogger _logger;

        public PastesController(PasteDbContext context, ILogger<PastesController> logger)
        {
            _logger = logger;
            _context = context;
            
            _logger.LogInformation("{} says hello!", nameof(PastesController));
        }

        // GET: api/Pastes
        [ProducesResponseType(typeof(IDictionary<string, Paste>), 200)]
        [HttpGet]
        public IEnumerable<Paste> GetPastes()
        {
            return _context.Pastes;
        }

        // GET: api/Pastes/5
        [ProducesResponseType(typeof(IDictionary<string, string>), 404)]
        [ProducesResponseType(typeof(Paste), 200)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaste([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var paste = await _context.Pastes.FindAsync(id);

            if (paste == null)
            {
                return NotFound();
            }

            return Ok(paste);
        }

        // PUT: api/Pastes/5
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 404)]
        [ProducesResponseType(typeof(Paste), 200)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaste([FromRoute] long id, [FromBody] Paste paste)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != paste.Id)
            {
                return BadRequest();
            }

            _context.Entry(paste).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PasteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(paste);
        }

        // POST: api/Pastes
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(Paste), 201)]
        [HttpPost]
        public async Task<IActionResult> PostPaste([FromBody] Paste paste, ApiVersion apiVersion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Pastes.Add(paste);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPaste", new { id = paste.Id, version = apiVersion.ToString() }, paste);
        }

        // DELETE: api/Pastes/5
        [ProducesResponseType(typeof(Paste), 200)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaste([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var paste = await _context.Pastes.FindAsync(id);
            if (paste == null)
            {
                return NotFound();
            }

            _context.Pastes.Remove(paste);
            await _context.SaveChangesAsync();

            return Ok(paste);
        }

        private bool PasteExists(long id)
        {
            return _context.Pastes.Any(e => e.Id == id);
        }
    }
}