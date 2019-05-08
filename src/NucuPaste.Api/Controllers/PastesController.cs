using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NucuPaste.Api.Models;
using NucuPaste.Api.Services;

namespace NucuPaste.Api.Controllers
{
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiVersion("1")]
    public class PastesController : ApiBaseController
    {
        private readonly PasteService _pasteService;

        public PastesController(ILogger<PastesController> logger, PasteService pasteService)
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
        public async Task<IActionResult> GetPaste([FromRoute] Guid id)
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
        public async Task<IActionResult> PutPaste([FromRoute] Guid id, [FromBody] PasteBindingModel paste)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
        public async Task<IActionResult> PostPaste([FromBody] PasteBindingModel bindingModel, ApiVersion apiVersion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var paste = await _pasteService.Create(bindingModel);

            return CreatedAtAction("GetPaste", new {id = paste.Id, version = apiVersion.ToString()}, paste);
        }

        // DELETE: api/Pastes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaste([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deleted = await _pasteService.DeleteById(id);
            if (deleted == false)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}