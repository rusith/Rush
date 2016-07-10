using System.IO;

namespace Rush.Models
{
    public class FileInformation
    {
        protected bool Equals(FileInformation other)
        {
            return IsDuplicate.Equals(other.IsDuplicate) && Equals(Duplicate, other.Duplicate) && Equals(SourceFile, other.SourceFile) && Equals(DestinationFile, other.DestinationFile);
        }
        public bool IsDuplicate { get; set; }
        public FileInformation Duplicate { get; set; }
        public FileInfo SourceFile { get; set; }
        public FileInfo DestinationFile { get; set; }
        public bool Processed { get; set; }
        public bool Copied { get; set; }
        public bool SourceDeleted { get; set; }
        public bool Skip { get; set; }

    }
}
