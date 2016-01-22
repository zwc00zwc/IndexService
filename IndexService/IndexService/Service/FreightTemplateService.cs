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
    public class FreightTemplateService:IFreightTemplate
    {
        public FreightTemplateInfo SearchByid(string id)
        {
            FreightTemplateInfo info = new FreightTemplateInfo();
            MySqlParameter idparamter = new MySqlParameter("id", MySqlDbType.Int64);
            idparamter.Value = id;
            DataTable dt = IndexService.Common.MySqlHelper.Search("SELECT * FROM himall_freighttemplate WHERE Id=@id", new MySqlParameter[] { idparamter });
            if (dt.Rows.Count == 1)
            {
                info.Id = long.Parse(dt.Rows[0]["Id"].ToString());
                info.Name = dt.Rows[0]["Name"].ToString();
                info.SourceAddress = int.Parse(dt.Rows[0]["SourceAddress"].ToString());
                info.SendTime = dt.Rows[0]["SendTime"].ToString();
                info.IsFree = int.Parse(dt.Rows[0]["IsFree"].ToString());
                info.ValuationMethod = int.Parse(dt.Rows[0]["ValuationMethod"].ToString());
                if (string.IsNullOrEmpty(dt.Rows[0]["ShippingMethod"].ToString()))
                {
                    info.ShippingMethod = null;
                }
                else
                {
                    info.ShippingMethod = int.Parse(dt.Rows[0]["ShippingMethod"].ToString());
                }
                info.ShopID = long.Parse(dt.Rows[0]["ShopID"].ToString());
                info.Description = dt.Rows[0]["Description"].ToString();
            }
            return info;
        }
    }
}
