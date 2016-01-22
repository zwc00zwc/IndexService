using System.Collections.Generic;
using IndexService.Interface;
using IndexService.Model;
using MySql.Data.MySqlClient;
using IndexService.Common;
using System.Data;
using System;

namespace IndexService.Service
{
    public class ProductInfoService:IProductInfo
    {
        /// <summary>
        /// 查询通过审核及上架的商品
        /// </summary>
        /// <param name="startnum"></param>
        /// <param name="currpage"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public List<ProductInfo> SearchBypage(long startnum, long currpage, long pagesize)
        {
            long startindex = (currpage - 1) * pagesize;
            List<ProductInfo> list = new List<ProductInfo>();
            MySqlParameter startparmter = new MySqlParameter("startindex", MySqlDbType.Int32);
            startparmter.Value = startindex + startnum;
            MySqlParameter sizepartmter = new MySqlParameter("size", MySqlDbType.Int32);
            sizepartmter.Value = pagesize;
            try
            {
                DataTable dt = IndexService.Common.MySqlHelper.Search(@"SELECT * FROM himall_products WHERE AuditStatus=2 AND SaleStatus=1 AND Id LIMIT @startindex,@size", new MySqlParameter[] { startparmter, sizepartmter });
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ProductInfo info = new ProductInfo();
                        info.Id = (long)dt.Rows[i]["Id"];
                        info.ProductName = dt.Rows[i]["ProductName"].ToString();
                        info.AuditStatus = (int)dt.Rows[i]["AuditStatus"];
                        info.SaleStatus = (int)dt.Rows[i]["SaleStatus"];
                        info.ShopId = (long)dt.Rows[i]["ShopId"];
                        info.CategoryId = (long)dt.Rows[i]["CategoryId"];
                        info.CategoryPath = dt.Rows[i]["CategoryPath"].ToString();
                        info.BrandId = (long)dt.Rows[i]["BrandId"];
                        info.MinSalePrice = (decimal)dt.Rows[i]["MinSalePrice"];
                        info.SaleCounts = (long)dt.Rows[i]["SaleCounts"];
                        info.AddedDate = (DateTime)dt.Rows[i]["AddedDate"];
                        info.FreightTemplateId = (long)dt.Rows[i]["FreightTemplateId"];
                        info.MeasureUnit = dt.Rows[i]["MeasureUnit"].ToString();
                        info.ImagePath = dt.Rows[i]["ImagePath"].ToString();
                        list.Add(info);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.writelog("MySQL获取数据出错" + ex.ToString());
            }
            return list;
        }


        public long Searchcount()
        {
            long num = 0;
            try
            {
                DataTable dt = IndexService.Common.MySqlHelper.Search(@"select count(1) as 'count' from himall_products");
                if (dt.Rows.Count == 1)
                {
                    num = long.Parse(dt.Rows[0]["count"].ToString());
                }
            }
            catch (Exception ex)
            {
                Utility.writelog("MySQL获取条数出错" + ex.ToString());
            }
            return num;
        }


        public int SearchComments(string pid)
        {
            int num = 0;
            try
            {
                MySqlParameter pidparameter = new MySqlParameter("proid", MySqlDbType.Int64);
                pidparameter.Value = pid;
                DataTable dt = IndexService.Common.MySqlHelper.Search(@"SELECT COUNT(1) as 'count' FROM himall_productcomments WHERE ProductId=@proid", new MySqlParameter[] { pidparameter });
                if (dt.Rows.Count == 1)
                {
                    num = int.Parse(dt.Rows[0]["count"].ToString());
                }
            }
            catch (Exception ex)
            {
                Utility.writelog("获取评论数出错" + ex.ToString());
            }
            return num;
        }
    }
}
