using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexService.Model
{
    /// <summary>
    /// 商家商品分类表
    /// </summary>
    public class ProductShopCategoryInfo
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public long ShopCategoryId { get; set; }
    }
}
