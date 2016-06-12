using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rush.Models
{
    public class FileInformation
    {
        protected bool Equals(FileInformation other)
        {
            return Equals(SourceFile, other.SourceFile) && Equals(DestinationFile, other.DestinationFile);
        }

        public bool IsDuplicate { get; set; }

        public FileInformation Duplicate { get; set; }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((SourceFile != null ? SourceFile.GetHashCode() : 0)*397) ^ (DestinationFile != null ? DestinationFile.GetHashCode() : 0);
            }
        }

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
