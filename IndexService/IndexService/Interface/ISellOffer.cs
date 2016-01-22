using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndexService.Model;

namespace IndexService.Interface
{
    public interface ISellOffer
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagecurr">页数</param>
        /// <param name="pagesize">页容量</param>
        /// <returns></returns>
        List<SellOffer> SearchBypage(int pagecurr, int pagesize);

        /// <summary>
        /// 查询条数
        /// </summary>
        /// <returns></returns>
        int SearchCount();
    }
}
