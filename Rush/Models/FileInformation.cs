
using System.IO;

namespace Rush.Models
{
    public class FileInformation
    {
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = SourceFile?.GetHashCode() ?? 0;
                hashCode = (hashCode*397) ^ (DestinationFile?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ IsDuplicate.GetHashCode();
                hashCode = (hashCode*397) ^ (Duplicate?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        protected bool Equals(FileInformation other)
        {
            return Equals(SourceFile, other.SourceFile) && Equals(DestinationFile, other.DestinationFile) && IsDuplicate.Equals(other.IsDuplicate) && Equals(Duplicate, other.Duplicate);
        }

        public bool IsDuplicate { get; set; }

        public FileInformation Duplicate { get; set; }

        public FileInfo SourceFile { get; set; }
        public FileInfo DestinationFile { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((FileInformation) obj);
        }
    }
}
