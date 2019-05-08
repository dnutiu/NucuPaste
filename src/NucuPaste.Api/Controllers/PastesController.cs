using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NucuPaste.Api.Models;
using NucuPaste.Api.Services;

namespace NucuPaste.Api.Controllers
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiVersion( "1" )]
    [Route( "api/v{version:apiVersion}/[controller]")]
    public class PastesController : ControllerBase
    {
        private readonly PasteService _pasteService;

        public PastesController(ILogger logger, PasteService pasteService)
        {
            _pasteService = pasteService;
            
            logger.LogInformation("{} says hello!", nameof(PastesController));
        }

        // GET: api/Pastes
        [HttpGet]
        public async Task<List<Paste>> GetPastes()
        {
            return await _pasteService.GetAll();
        }

        // GET: api/Pastes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaste([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var paste = await _pasteService.GetById(id);

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

            var updated = await _pasteService.Update(id, paste);
            if (updated == false)
            {
                return NotFound();
            }
            
            return Ok(paste);
        }

        // POST: api/Pastes
        [HttpPost]
        public async Task<IActionResult> PostPaste([FromBody] Paste paste, ApiVersion apiVersion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _pasteService.Create(paste);

            return CreatedAtAction("GetPaste", new { id = paste.Id, version = apiVersion.ToString() }, paste);
        }

        // DELETE: api/Pastes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaste([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var paste = await _pasteService.DeleteById(id);
            if (paste == false)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}