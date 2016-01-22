using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndexService.Common;
using IndexService.Interface;
using IndexService.Model;
using System.Data;
using System.Data.SqlClient;

namespace IndexService.Service
{
    public class SellOfferDetailService:ISellOfferDetail
    {
        public SellOfferDetail SearchById(int id)
        {
            SellOfferDetail detail = new SellOfferDetail();
            DataTable dt = new DataTable();
            dt = SqlHelper.Search(@"select * from Products_SellOffer_Detail where SellOfferId=@id", new SqlParameter[] { new SqlParameter("@id", id) });
            if (dt.Rows.Count == 1)
            {
                detail.Id = (int)dt.Rows[0]["Id"];
                detail.SellOfferId = (int)dt.Rows[0]["SellOfferId"];
                detail.Detail = dt.Rows[0]["Detail"].ToString();
            }
            return detail;
        }
    }
}
