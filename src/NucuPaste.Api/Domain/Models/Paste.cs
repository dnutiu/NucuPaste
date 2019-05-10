using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// ReSharper disable MemberCanBePrivate.Global

namespace NucuPaste.Api.Domain.Models
{
    // PasteBindingModel used to bind only certain fields of the Paste model.
    public class PasteBindingModel 
    {
        [Required] public string FileName { get; set; }

        [Required] public string FileContent { get; set; }
    }
    
    public class Paste : PasteBindingModel
    {
        [Key] public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }
        
        public DateTime? LastUpdated { get; set; }

        private sealed class PasteEqualityComparer : IEqualityComparer<Paste>
        {
            public bool Equals(Paste x, Paste y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Id.Equals(y.Id) && string.Equals(x.FileName, y.FileName) && string.Equals(x.FileContent, y.FileContent) && x.CreatedAt.Equals(y.CreatedAt) && x.LastUpdated == y.LastUpdated;
            }

            public int GetHashCode(Paste obj)
            {
                unchecked
                {
                    var hashCode = obj.Id.GetHashCode();
                    hashCode = (hashCode * 397) ^ (obj.FileName != null ? obj.FileName.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.FileContent != null ? obj.FileContent.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ obj.CreatedAt.GetHashCode();
                    hashCode = (hashCode * 397) ^ obj.LastUpdated.GetHashCode();
                    return hashCode;
                }
            }
        }

        public static IEqualityComparer<Paste> EqualityComparer { get; } = new PasteEqualityComparer();
    }
}