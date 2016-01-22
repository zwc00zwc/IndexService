using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexService.Model
{
    public class FreightTemplateInfo
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> SourceAddress { get; set; }
        public string SendTime { get; set; }
        public int IsFree { get; set; }
        public int ValuationMethod { get; set; }
        public Nullable<int> ShippingMethod { get; set; }
        public long ShopID { get; set; }
        public string Description { get; set; }
    }
}
