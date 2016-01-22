using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Lucene.Net;
using Lucene.Net.Store;
using Lucene.Net.Index;
using System.Configuration;
using System.IO;
using IndexService.Model;
using IndexService.Interface;
using IndexService.Service;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using IndexService.Common;

namespace IndexService.Job
{
    public class ProductJob:IJob
    {
        public void Execute(JobExecutionContext context)
        {
            Utility.writelog("创建产品索引开始");
            Create();
            Utility.writelog("创建产品索引结束");
        }

        private static void Create()
        {
            Utility.writelog("进入方法");
            IProductInfo proBll = new ProductInfoService();
            List<ProductInfo> prolist = new List<ProductInfo>();
            int pageSize = 500;  //页容量
            long totalNum = proBll.Searchcount(); //总数量
            long succNum = Utility.GetSuccNum();  //已完成的数量
            long pageNum = 0;
            if (totalNum - succNum > 0)
            {
                pageNum = (totalNum - succNum) % pageSize == 0 ? (totalNum - succNum) / pageSize : (totalNum - succNum) / pageSize + 1;   //需要进行创建索引的页数
            }
            int i = 1;
            try
            {
                Utility.writelog("开始创建索引");
                for (i = 1; i <= pageNum; i++)
                {
                    Utility.writelog("创建第" + i + "页数据索引");
                    prolist = proBll.SearchBypage(succNum, i, pageSize);
                    CreateIndex(prolist);   //创建索引
                    Utility.writelog("创建第" + i + "页数据索引完毕");
                }
                Utility.writelog("创建完毕");
                Utility.SetSuccNem(totalNum);
            }
            catch (Exception ex)
            {
                Utility.writelog("创建第" + i + "页索引出问题:" + ex + "");
            }
        }

        private static void CreateIndex(List<ProductInfo> list)
        {
            IProductShopCategoryInfo shopBll = new ProductShopCategoryService();
            IFreightTemplate freBll = new FreightTemplateService();
            IProductInfo pBll = new ProductInfoService();
            IProductAttributeInfo proAttrBll = new ProductAttributeService();
            IAttributeValueInfo attrBll = new AttributeValueService();
            string indexpath = ConfigurationManager.AppSettings["Indexpath"];
            IProductDescriptionInfo desBll = new ProductDescriptionInfoService();
            IBrandInfo braBll = new BrandInfoService();

            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexpath), new NativeFSLockFactory());
            //IndexReader:对索引库进行读取的类
            bool isExist = IndexReader.IndexExists(directory); //是否存在索引库文件夹以及索引库特征文件
            if (isExist)
            {
                //如果索引目录被锁定（比如索引过程中程序异常退出或另一进程在操作索引库），则解锁
                //Q:存在问题 如果一个用户正在对索引库写操作 此时是上锁的 而另一个用户过来操作时 将锁解开了 于是产生冲突 --解决方法后续
                if (IndexWriter.IsLocked(directory))
                {
                    IndexWriter.Unlock(directory);
                }
            }

            IndexWriter writer = new IndexWriter(directory, new PanGuAnalyzer(), !isExist, IndexWriter.MaxFieldLength.UNLIMITED);
            try
            {
                foreach (var pitem in list)
                {
                    string shopcategoryid="";
                    List<ProductShopCategoryInfo> shoplist = new List<ProductShopCategoryInfo>();
                    shoplist = shopBll.SearchbyProid(pitem.Id.ToString());
                    foreach (var shopitem in shoplist)
                    {
                        shopcategoryid += shopitem.ShopCategoryId.ToString() + ",";
                    }
                    FreightTemplateInfo freinfo = new FreightTemplateInfo();
                    freinfo = freBll.SearchByid(pitem.FreightTemplateId.ToString());

                    List<ProductAttributeInfo> attrlist = new List<ProductAttributeInfo>();
                    string proattr = ",";
                    string attrval = "";
                    string attr = "";
                    attrlist = proAttrBll.SearchByproductid(pitem.Id.ToString());     //产品属性
                    foreach (var attitem in attrlist)
                    {
                        proattr += attitem.ValueId + ",";  //属性ID用于精确查找
                        attr = attrBll.GetAttributeValueById(attitem.ValueId);
                        attrval += attr + ",";       //属性值用于模糊查询
                    }
                    string desinfo = "";
                    desinfo = desBll.GetByProductId(pitem.Id);  //产品描述  用于模糊查询

                    string brand = "";
                    brand = braBll.GetNameById(pitem.BrandId);  //品牌名字  用于模糊查询
                    attrval += brand;   //品牌属性

                    Document document = new Document();
                    Field id = new Field("Id", pitem.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED); //商品ID
                    Field ProductName = new Field("ProductName", pitem.ProductName, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS); //商品名称
                    ProductName.SetBoost(0.5f);
                    //Field BrandName = new Field("BrandName", brand, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS);   //品牌名字
                    Field Description = new Field("Description", desinfo, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS);  //描述
                    Description.SetBoost(0.15f);
                    Field BrandAttribution = new Field("BrandAttribution", attrval, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS);  //品牌属性
                    BrandAttribution.SetBoost(0.35f);

                    Field AuditStatus = new Field("AuditStatus", pitem.AuditStatus.ToString(), Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS); //
                    Field SaleStatus = new Field("SaleStatus", pitem.SaleStatus.ToString(), Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS);  //销售状态
                    NumericField ShopId = new NumericField("ShopId", Field.Store.YES, true).SetLongValue(pitem.ShopId); //商家ID
                    Field CategoryPath = new Field("CategoryPath", pitem.CategoryPath, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS); //分类路径
                    NumericField CategoryId = new NumericField("CategoryId", Field.Store.YES, true).SetLongValue(pitem.CategoryId); //分类ID
                    NumericField BrandId = new NumericField("BrandId", Field.Store.YES, true).SetLongValue(pitem.BrandId);  //品牌ID
                    NumericField MinSalePrice = new NumericField("MinSalePrice", Field.Store.YES, true).SetDoubleValue(Convert.ToDouble(pitem.MinSalePrice)); //价格
                    NumericField SaleCounts = new NumericField("SaleCounts", Field.Store.YES, true).SetLongValue(pitem.SaleCounts);  //销售数量
                    NumericField AddedDate = new NumericField("AddedDate", Field.Store.YES, true).SetLongValue(pitem.AddedDate.Ticks);   //商品上架时间
                    Field FreightTemplateId = new Field("FreightTemplateId", pitem.FreightTemplateId.ToString(), Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS);
                    Field Shopcategoryid = new Field("Shopcategoryid", shopcategoryid, Field.Store.YES, Field.Index.NOT_ANALYZED);  //商家分类ID
                    Field ProductAttribute = new Field("ProductAttribute", proattr, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS);   //商品属性
                    Field ProductImage = new Field("ProductImage", string.IsNullOrEmpty(pitem.ImagePath) ? "未知" : pitem.ImagePath, Field.Store.YES, Field.Index.NOT_ANALYZED);
                    Field SaleUnit = new Field("SaleUnit", string.IsNullOrEmpty(pitem.MeasureUnit) ? "未知" : pitem.MeasureUnit, Field.Store.YES, Field.Index.NOT_ANALYZED);  //销售单位

                    int SourceAddress = 0;
                    if (freinfo.SourceAddress != null)
                    {
                        SourceAddress = (int)freinfo.SourceAddress;
                    }
                    NumericField AddressId = new NumericField("AddressId", Field.Store.YES, true).SetIntValue(SourceAddress);  //地区ID
                    int commentnum = pBll.SearchComments(pitem.Id.ToString());
                    NumericField CommentNum = new NumericField("CommentNum", Field.Store.YES, true).SetIntValue(commentnum); //评论数
                    document.Add(id);
                    document.Add(ProductName);
                    //document.Add(BrandName);
                    document.Add(Description);
                    document.Add(BrandAttribution);
                    document.Add(AuditStatus);
                    document.Add(SaleStatus);
                    document.Add(ShopId);
                    document.Add(CategoryPath);
                    document.Add(CategoryId);
                    document.Add(BrandId);
                    document.Add(MinSalePrice);
                    document.Add(SaleCounts);
                    document.Add(AddedDate);
                    document.Add(FreightTemplateId);
                    document.Add(Shopcategoryid);
                    document.Add(ProductAttribute);
                    document.Add(ProductImage);
                    document.Add(SaleUnit);
                    document.Add(AddressId);
                    document.Add(CommentNum);
                    writer.AddDocument(document); //文档写入索引库
                }
                writer.Optimize();
                writer.Close();//会自动解锁
                directory.Close(); //不要忘了Close，否则索引结果搜不到
            }
            catch (Exception ex)
            {
                Utility.writelog("创建索引出问题:" + ex + "");
            }
        }
    }
}
