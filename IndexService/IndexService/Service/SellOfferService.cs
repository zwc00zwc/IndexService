using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndexService.Model;
using IndexService.Interface;
using IndexService.Common;
using System.Data;
using System.Data.SqlClient;

namespace IndexService.Service
{
    public class SellOfferService:ISellOffer
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagecurr">当前页</param>
        /// <param name="pagesize">页容量</param>
        /// <returns></returns>
        public List<SellOffer> SearchBypage(int pagecurr,int pagesize)
        {
            List<SellOffer> list = new List<SellOffer>();
            int startindex = (pagecurr - 1) * pagesize;
            int endindex = pagecurr * pagesize;

            DataTable dt = SqlHelper.Search(@"select * from (SELECT *, ROW_NUMBER() over(order by Id) as 'row' FROM Products_SellOffer) as A where A.row between @startindex and @endindex", new SqlParameter[] { new SqlParameter("startindex", startindex), new SqlParameter("endindex", endindex) });
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SellOffer selloffer = new SellOffer();
                    selloffer.Id = (int)dt.Rows[i]["Id"];
                    selloffer.Title = dt.Rows[i]["Title"].ToString();
                    selloffer.Keywords = dt.Rows[i]["Keywords"].ToString();
                    selloffer.SysAttr = dt.Rows[i]["SysAttr"].ToString();
                    selloffer.CusAttr = dt.Rows[i]["CusAttr"].ToString();
                    list.Add(selloffer);
                }
            }
            return list;
        }

        /// <summary>
        /// 查询数量
        /// </summary>
        /// <returns></returns>
        public int SearchCount()
        {
            int Count = 0;
            DataTable dt = new DataTable();
            dt = SqlHelper.Search(@"select count(1) as 'count' from Products_SellOffer");
            if (dt.Rows.Count == 1)
            {
                Count = (int)dt.Rows[0]["count"];
            }
            return Count;
        }
    }
}
