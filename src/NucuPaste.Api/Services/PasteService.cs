using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NucuPaste.Api.Data;
using NucuPaste.Api.Models;

namespace NucuPaste.Api.Services
{
    public class PasteService
    {
        private readonly NucuPasteContext _context;

        internal bool PasteExists(long id)
        {
            return _context.Pastes.Any(e => e.Id == id);
        }

        public PasteService(NucuPasteContext context)
        {
            _context = context;
        }

        public async Task<List<Paste>> GetAll()
        {
            var pastes = await (
                from p in _context.Pastes.AsNoTracking() select p
            ).ToListAsync();
            return pastes;
        }

        public async Task<Paste> GetById(long id)
        {
            return await _context.Pastes.FindAsync(id);
        }

        public async Task<Paste> Create(Paste paste)
        {
            _context.Pastes.Add(paste);
            await _context.SaveChangesAsync();
            return paste;
        }

        public async Task<bool> DeleteById(long id)
        {
            var paste = await GetById(id);
            if (paste == null) return false;
            
            _context.Pastes.Remove(paste);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(long id, Paste paste)
        {
            // Tell EF that the state of the paste has been modified.
            _context.Entry(paste).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PasteExists(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
            return true;
        }


    }
}