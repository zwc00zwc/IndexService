using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexService.Model
{
    public class ProductAttributeInfo
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public long AttributeId { get; set; }
        public long ValueId { get; set; }
        public string ValueName { get; set; }
    }
}
