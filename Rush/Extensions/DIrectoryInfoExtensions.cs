using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rush.Extensions
{
    public static class DIrectoryInfoExtensions
    {
        public static List<FileInfo> GetFilesUsingExtensions(this DirectoryInfo dir, string[] extensions)
        {
            if (extensions == null)
                throw new ArgumentNullException("extensions");
            for (var i = 0; i < extensions.Length; i++)
            {
                if (!extensions[i].StartsWith("."))
                    extensions[i] = "." + extensions[i];
            }

            var files = new FileInfo[]{};
            try
            {
                files = dir.GetFiles("*", SearchOption.AllDirectories);
            }
            catch(Exception)
            {
                //ignored
            }
            
            return files.Where(file => extensions.Contains(file.Extension)).ToList();
        }
    }
}
