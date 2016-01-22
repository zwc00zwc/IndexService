using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndexService.Model;
using IndexService.Interface;
using System.Data;
using IndexService.Common;
using MySql.Data.MySqlClient;

namespace IndexService.Service
{
    public class ProductAttributeService:IProductAttributeInfo
    {
        public List<ProductAttributeInfo> SearchByproductid(string id)
        {
            List<ProductAttributeInfo> list = new List<ProductAttributeInfo>();
            DataTable dt = new DataTable();
            MySqlParameter pidpartmer = new MySqlParameter("pid", MySqlDbType.Int64);
            pidpartmer.Value = id;
            dt = IndexService.Common.MySqlHelper.Search("SELECT * FROM himall_productattributes WHERE ProductId=@pid", new MySqlParameter[] { pidpartmer });
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ProductAttributeInfo info = new ProductAttributeInfo();
                    info.Id = long.Parse(dt.Rows[i]["Id"].ToString());
                    info.ProductId = long.Parse(dt.Rows[i]["ProductId"].ToString());
                    info.AttributeId = long.Parse(dt.Rows[i]["AttributeId"].ToString());
                    info.ValueId = long.Parse(dt.Rows[i]["ValueId"].ToString());
                    info.ValueName = dt.Rows[i]["ValueName"].ToString();
                    list.Add(info);
                }
            }
            return list;
        }
    }
}
