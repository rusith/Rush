
using System.IO;

namespace Rush.Models
{
    public class FileInformation
    {
        protected bool Equals(FileInformation other)
        {
            return IsDuplicate.Equals(other.IsDuplicate) && Equals(Duplicate, other.Duplicate) && Equals(SourceFile, other.SourceFile) && Equals(DestinationFile, other.DestinationFile);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = IsDuplicate.GetHashCode();
                hashCode = (hashCode*397) ^ (Duplicate != null ? Duplicate.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (SourceFile != null ? SourceFile.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (DestinationFile != null ? DestinationFile.GetHashCode() : 0);
                return hashCode;
            }
        }

        public bool IsDuplicate { get; set; }

        public FileInformation Duplicate { get; set; }

        public FileInfo SourceFile { get; set; }
        public FileInfo DestinationFile { get; set; }

    }
}
