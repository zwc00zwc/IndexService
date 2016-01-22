using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexService.Model
{
    public class AttributeValueInfo
    {
        public long Id { get; set; }
        public long AttributeId { get; set; }
        public string Value { get; set; }
        public long DisplaySequence { get; set; }
    }
}
