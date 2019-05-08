using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
// ReSharper disable MemberCanBePrivate.Global

namespace NucuPaste.Api.Models
{
    public class Paste
    {
        [Key] public long Id { get; set; }

        [Required] public string FileName { get; set; }
        
        [Required] public string FileContent { get; set; }

        private sealed class EqualityComparerClass : IEqualityComparer<Paste>
        {
            public bool Equals(Paste x, Paste y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Id == y.Id && string.Equals(x.FileName, y.FileName) &&
                       string.Equals(x.FileContent, y.FileContent);
            }

            public int GetHashCode(Paste obj)
            {
                unchecked
                {
                    var hashCode = obj.Id.GetHashCode();
                    hashCode = (hashCode * 397) ^ (obj.FileName != null ? obj.FileName.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.FileContent != null ? obj.FileContent.GetHashCode() : 0);
                    return hashCode;
                }
            }
        }

        public static IEqualityComparer<Paste> EqualityComparer { get; } = new EqualityComparerClass();
    }
}