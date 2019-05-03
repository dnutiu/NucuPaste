using System.ComponentModel.DataAnnotations;

namespace NucuPaste.Models
{
    public class Paste
    {
        public long Id { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public string FileContent { get; set; }
    }
}
