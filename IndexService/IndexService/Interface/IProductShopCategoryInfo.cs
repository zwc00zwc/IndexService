using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndexService.Model;

namespace IndexService.Interface
{
    public interface IProductShopCategoryInfo
    {
        /// <summary>
        /// 根据产品ID查询商家商品信息
        /// </summary>
        /// <param name="pid">产品ID
        /// </param>
        /// <returns></returns>
        List<ProductShopCategoryInfo> SearchbyProid(string pid);
    }
}
