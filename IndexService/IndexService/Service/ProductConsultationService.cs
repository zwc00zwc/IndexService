using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndexService.Interface;
using IndexService.Model;
using IndexService.Common;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace IndexService.Service
{
    public class ProductConsultationService:IProductConsultationInfo
    {

        public List<ProductConsultationInfo> SearchbyPid(string pid)
        {
            List<ProductConsultationInfo> list = new List<ProductConsultationInfo>();
            MySqlParameter pidparamter = new MySqlParameter("pid", MySqlDbType.Int64);
            pidparamter.Value = pid;
            DataTable dt = IndexService.Common.MySqlHelper.Search("SELECT c.* FROM himall_productconsultations AS c JOIN himall_products AS p ON c.ProductId=@pid", new MySqlParameter[] { pidparamter });
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ProductConsultationInfo info = new ProductConsultationInfo();
                    info.Id = long.Parse(dt.Rows[i]["Id"].ToString());
                    info.ProductId = long.Parse(dt.Rows[i]["ProductId"].ToString());
                    info.ShopId = long.Parse(dt.Rows[i]["ShopId"].ToString());
                    info.ShopName = dt.Rows[i]["ShopName"].ToString();
                    info.UserId = long.Parse(dt.Rows[i]["UserId"].ToString());
                    info.UserName = dt.Rows[i]["UserName"].ToString();
                    info.Email = dt.Rows[i]["Email"].ToString();
                    info.ConsultationContent = dt.Rows[i]["ConsultationContent"].ToString();
                    info.ConsultationDate = DateTime.Parse(dt.Rows[i]["ConsultationDate"].ToString());
                    info.ReplyContent = dt.Rows[i]["ReplyContent"].ToString();
                    info.ReplyDate = DateTime.Parse(dt.Rows[i]["ReplyDate"].ToString());
                    list.Add(info);
                }
            }
            return list;
        }
    }
}
