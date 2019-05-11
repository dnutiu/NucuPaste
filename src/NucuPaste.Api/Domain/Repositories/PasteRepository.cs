using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NucuPaste.Api.Data;
using NucuPaste.Api.Domain.Models;

namespace NucuPaste.Api.Domain.Repositories
{
    public class PasteRepository
    {
        private readonly NucuPasteContext _context;

        private async Task<bool> PasteExistsAsync(Guid id)
        {
            return await _context.Pastes.AnyAsync(p => p.Id == id);
        }

        public PasteRepository(NucuPasteContext context)
        {
            _context = context;
        }

        public async Task<List<Paste>> GetAllAsync()
        {
            var pastes = await (
                from p in _context.Pastes.AsNoTracking() select p
            ).ToListAsync();
            return pastes;
        }

        public async Task<Paste> GetByIdAsync(Guid id)
        {
            return await _context.Pastes.FindAsync(id);
        }

        public async Task<Paste> Create(PasteBindingModel pasteBinding)
        {
            var paste = new Paste(pasteBinding.FileName,
                pasteBinding.FileContent);

            _context.Pastes.Add(paste);
            await _context.SaveChangesAsync();
            return paste;
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            var paste = await GetByIdAsync(id);
            if (paste == null) return false;

            _context.Pastes.Remove(paste);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(Guid id, PasteBindingModel paste)
        {
            // We need to search for the old paste in order to get it's createdAt date.
            var oldPaste = await _context.Pastes.FindAsync(id);
            if (oldPaste == null)
            {
                return false;
            }

            // Update fields
            oldPaste.Update(paste.FileName, paste.FileContent);

            // Tell EF that the state of the paste has been modified.
            _context.Entry(oldPaste).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}