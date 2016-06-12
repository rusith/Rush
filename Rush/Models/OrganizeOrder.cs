
using System.Collections.Generic;
using System.Linq;

namespace Rush.Models
{
    public enum OrderElement
    {
         Album = 1
        ,Artist = 2
        ,Genre = 3
        ,Year = 4
        ,File = 6
    }

    public class OrganizeOrder
    {
        public List<OrderElement> Order { get; } = new List<OrderElement>();

        public FileNameTemplate FileNameTemplate { get; set; }=new FileNameTemplate();

        public bool AddElement(OrderElement elemet)
        {
            if(elemet==OrderElement.File && Order.Contains(OrderElement.File))
                return false;
            if(Order.Any() && elemet==Order.Last())
                return false;
            Order.Add(elemet);
            return true;
        }

        public new string ToString()
        {
            if (Order.Count<1) return "";
            var text = "";
            for (var i = 0; i < Order.Count; i++)
                text = text + (i > 0 ? " → " : "") + ElemetToString( Order[i]);
            return text;
        }


        public string ElemetToString(OrderElement element)
        {
            switch (element)
            {
                case OrderElement.Album:
                    return "Album";
                case OrderElement.Artist:
                    return "Artist";
                case OrderElement.File:
                    return string.Format("<{0}>.<ext>",FileNameTemplate);
                case OrderElement.Genre:
                    return "Genre";
                case OrderElement.Year:
                    return "Year";
                default :
                    return "";
            }
        }

        public bool IsEmpty()
        {
            return Order.Count<1;
        }

    }
}
