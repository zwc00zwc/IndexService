using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndexService.Model;

namespace IndexService.Interface
{
    public interface ISellOfferDetail
    {
        /// <summary>
        /// 按SellOfferId查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SellOfferDetail SearchById(int id);
    }
}
