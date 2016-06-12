using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rush.Controllers
{
    public class FileTypesInfo
    {
        public bool Mp3 { get; set; }
        public bool M4A { get; set; }
        public bool Aac { get; set; }
        public bool Falc { get; set; }
        public bool Ogg { get; set; }
        public bool Wma { get; set; }

        public FileTypesInfo()
        {
            Mp3 = false;
            M4A = false;
            Aac = false;
            Falc = false;
            Ogg = false;
            Wma = false;
        }

        public string[] ToStringArray()
        {
            var list = new List<string>();
            if (Mp3)
                list.Add("mp3");
            if (M4A)
                list.Add("m4a");
            if (Falc)
                list.Add("falc");
            if (Aac)
                list.Add("aac");
            if (Ogg)
                list.Add("ogg");
            if (Wma)
                list.Add("wma");
            return list.ToArray();
        }




    }
}
