using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NucuPaste.Data;
using NucuPaste.Models;

namespace NucuPaste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PastesController : ControllerBase
    {
        private readonly PasteDbContext _context;

        public PastesController(PasteDbContext context)
        {
            _context = context;
        }

        // GET: api/Pastes
        [HttpGet]
        public IEnumerable<Paste> GetPastes()
        {
            return _context.Pastes;
        }

        // GET: api/Pastes/5
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
        [HttpPost]
        public async Task<IActionResult> PostPaste([FromBody] Paste paste)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Pastes.Add(paste);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPaste", new { id = paste.Id }, paste);
        }

        // DELETE: api/Pastes/5
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