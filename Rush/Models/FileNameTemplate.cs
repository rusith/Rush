using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Rush.Models
{
    public enum FileNameItem
    {
        Artist = 1,
        Album =2,
        Trak = 3,
        Title = 4,
        Literel = 5,
        Count = 6

    }
    public class FileNameTemplate
    {
        public List<FileNameItem> Template { get; } = new List<FileNameItem>();

        public List<string> Literel { get; set; } = new List<string>();

        public bool AddElement(FileNameItem elemet)
        {
            if (elemet == FileNameItem.Literel) return false;
            Template.Add(elemet);
            return false;
        }

        public void AddLiterel(string text)
        {
            Template.Add(FileNameItem.Literel);
            Literel.Add(text);
        }

        public string ToTemplateString()
        {
            return Template.Count < 1 ? "" : Template.Aggregate("", (current, item) => current + string.Format("[{0}]", ElemetToString(item)));
        }

        public override string ToString()
        {
            if (Template.Count < 1)
                return "File";
            var str = "";
            var next = 0;
            foreach (var item in Template)
            {
                if (item == FileNameItem.Literel)
                {
                    str += Literel.ElementAt(next);
                    next++;
                    continue;
                }
                str += "[" + ElemetToString(item) + "]";
            }
            return str;
        }


        public string ElemetToString(FileNameItem element)
        {
            switch (element)
            {
                case FileNameItem.Album:
                    return "Album";
                case FileNameItem.Artist:
                    return "Artist";
                case FileNameItem.Trak:
                    return "Track";
                case FileNameItem.Title:
                    return "Title";
                case FileNameItem.Count:
                    return "Count";
                default:
                    return "";
            }
        }

        public bool IsEmpty()
        {
            return Template.Count < 1;
        }
    }
}
