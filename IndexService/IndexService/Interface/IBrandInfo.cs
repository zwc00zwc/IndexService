using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndexService.Model;

namespace IndexService.Interface
{
    public interface IBrandInfo
    {
        string GetNameById(long id);
    }
}
