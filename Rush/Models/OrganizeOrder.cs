
using System.Collections.Generic;
using System.Linq;
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

        public void AddElement(OrderElement elemet)
        {
            if(elemet==OrderElement.File && _order.Contains(OrderElement.File))
                return;
            if(elemet==_order.Last())
                return;
            _order.Add(elemet);
        }
    }
}
