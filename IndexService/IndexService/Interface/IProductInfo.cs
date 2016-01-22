using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndexService.Model;

namespace IndexService.Interface
{
    public interface IProductInfo
    {
        /// <summary>
        /// 查询通过审核及上架的商品
        /// </summary>
        /// <param name="startnum">起始数字</param>
        /// <param name="currpage"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        List<ProductInfo> SearchBypage(long startnum, long currpage, long pagesize);

        long Searchcount();

        /// <summary>
        /// 查询评论数
        /// </summary>
        int SearchComments(string pid);
    }
}
