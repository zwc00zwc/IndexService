using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndexService.Interface;
using System.Data;
using MySql.Data.MySqlClient;
using IndexService.Model;

namespace IndexService.Service
{
    public class AttributeValueService:IAttributeValueInfo
    {
        public string GetAttributeValueById(long id)
        {
            string info = "";
            DataTable dt = new DataTable();
            MySqlParameter idPartmter = new MySqlParameter("_id", MySqlDbType.Int64);
            idPartmter.Value = id;
            dt = IndexService.Common.MySqlHelper.Search("SELECT * FROM Himall_AttributeValues WHERE Id=@_id", new MySqlParameter[] { idPartmter });
            if (dt.Rows.Count > 1)
            {
                info = dt.Rows[0]["Value"].ToString();
            }
            return info;
        }
    }
}
