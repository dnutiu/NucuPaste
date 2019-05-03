using Microsoft.EntityFrameworkCore;
using NucuPaste.Models;

namespace NucuPaste.Data
{
    public class PasteDbContext : DbContext
    {
        public PasteDbContext(DbContextOptions<PasteDbContext> options) : base(options)
        {

        }

        public DbSet<Paste> Pastes { get; set; }
    }
}
