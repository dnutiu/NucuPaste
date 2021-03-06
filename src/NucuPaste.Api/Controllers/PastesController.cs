﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NucuPaste.Api.Domain.Models;
using NucuPaste.Api.Domain.Repositories;

namespace NucuPaste.Api.Controllers
{
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiVersion("1")]
    public class PastesController : ApiBaseController
    {
        private readonly PasteRepository _pasteRepository;

        public PastesController(ILogger<PastesController> logger, PasteRepository pasteRepository)
        {
            _pasteRepository = pasteRepository;

            logger.LogInformation("{} says hello!", nameof(PastesController));
        }

        // GET: api/Pastes
        [HttpGet]
        public async Task<List<Paste>> GetPastes()
        {
            return await _pasteRepository.GetAllAsync();
        }

        // GET: api/Pastes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaste([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var paste = await _pasteRepository.GetByIdAsync(id);

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

            var updated = await _pasteRepository.UpdateAsync(id, paste);
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

            var paste = await _pasteRepository.Create(bindingModel);

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

            var deleted = await _pasteRepository.DeleteByIdAsync(id);
            if (deleted == false)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}