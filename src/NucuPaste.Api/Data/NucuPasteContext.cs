using Microsoft.EntityFrameworkCore;
using NucuPaste.Api.Domain.Models;

namespace NucuPaste.Api.Data
{
    public class NucuPasteContext : DbContext
    {
        public NucuPasteContext() : base() 
        {

        }

        public NucuPasteContext(DbContextOptions<NucuPasteContext> options) : base(options)
        {

        }

        public DbSet<Paste> Pastes { get; set; }
    }
}
