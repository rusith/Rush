
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Windows.Documents;

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

    class OrganizeOrder
    {
        private readonly List<OrderElement> _order=new List<OrderElement>();

        public bool AddElement(OrderElement elemet)
        {
            if(elemet==OrderElement.File && _order.Contains(OrderElement.File))
                return false;
            if(_order.Any() && elemet==_order.Last())
                return false;
            _order.Add(elemet);
            return true;
        }

        public new string ToString()
        {
            if (_order.Count<1) return "";
            var text = "";
            for (var i = 0; i < _order.Count; i++)
                text = text + (i > 0 ? " --> " : "") + ElemetToString( _order[i]);
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
                    return "File";
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
            return _order.Count<1;
        }

    }
}
