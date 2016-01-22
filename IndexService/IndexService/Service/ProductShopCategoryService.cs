using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndexService.Interface;
using IndexService.Model;
using System.Data;
using IndexService.Common;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace IndexService.Service
{
    public class ProductShopCategoryService:IProductShopCategoryInfo
    {
        /// <summary>
        /// 根据产品ID查询商家商品信息
        /// </summary>
        /// <param name="pid">产品ID</param>
        /// <returns></returns>
        public List<ProductShopCategoryInfo> SearchbyProid(string pid)
        {
            List<ProductShopCategoryInfo> list = new List<ProductShopCategoryInfo>();
            MySqlParameter pidparamter = new MySqlParameter("pid", MySqlDbType.Int64);
            pidparamter.Value = pid;
            DataTable dt = IndexService.Common.MySqlHelper.Search("SELECT * FROM himall_productshopcategories WHERE ProductId=@pid", new MySqlParameter[] { pidparamter });
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ProductShopCategoryInfo info = new ProductShopCategoryInfo();
                    info.Id = long.Parse(dt.Rows[i]["Id"].ToString());
                    info.ProductId = long.Parse(dt.Rows[i]["ProductId"].ToString());
                    info.ShopCategoryId = long.Parse(dt.Rows[i]["ShopCategoryId"].ToString());
                    list.Add(info);
                }
            }
            return list;
        }
    }
}
