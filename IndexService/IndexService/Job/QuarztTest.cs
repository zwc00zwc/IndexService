using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Common.Logging;
using System.IO;
using IndexService.Common;
using IndexService.Model;
using Lucene.Net.Store;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using Lucene.Net.Analysis.PanGu;
using log4net;
using IndexService.Interface;
using IndexService.Service;
using System.Configuration;

namespace IndexService.Job
{
    public class QuarztTest:IJob
    {
        public void Execute(JobExecutionContext context)
        {
            Utility.writelog("Quartz服务开始");
            Create();
            Utility.writelog("Quartz服务结束");
        }

        private static void Create()
        {
            ISellOffer sellofferBll = new SellOfferService();
            List<SellOffer> offerlist = new List<SellOffer>();
            int pageSize = 500;  //页容量
            int totalNum = sellofferBll.SearchCount(); //总数量
            int pageNum = totalNum % pageSize == 0 ? totalNum / pageSize : totalNum / pageSize + 1;   //页数
            int i = 1;
            try
            {
                Utility.writelog("开始创建索引");
                for (i = 1; i <= pageNum; i++)
                {
                    Utility.writelog("创建第" + i + "页数据索引");
                    offerlist = sellofferBll.SearchBypage(i, pageSize);
                    CreateIndex(offerlist);   //创建索引
                }
                Utility.writelog("创建完毕");
            }
            catch (Exception ex)
            {
                Utility.writelog("创建第" + i + "页索引出问题:" + ex + "");
            }
        }

        private static void CreateIndex(List<SellOffer> list)
        {
            string indexpath = ConfigurationManager.AppSettings["Indexpath"];
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
            ISellOfferDetail selldetailBll = new SellOfferDetailService();
            try
            {
                foreach (var pitem in list)
                {
                    SellOfferDetail offerdetail = new SellOfferDetail();
                    offerdetail = selldetailBll.SearchById(pitem.Id);
                    Document document = new Document();
                    Field id = new Field("id", pitem.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED);

                    Field title = new Field("title", pitem.Title, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS);
                    title.SetBoost(1.0f);

                    Field keywords = new Field("keywords", pitem.Keywords, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS);
                    keywords.SetBoost(0.7f);

                    Field detail = new Field("detail", offerdetail.Detail, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS);

                    Field sysattr = new Field("sysattr", pitem.SysAttr, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS);
                    sysattr.SetBoost(0.4f);

                    Field cusattr = new Field("cusattr", pitem.CusAttr, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS);
                    cusattr.SetBoost(0.1f);

                    document.Add(id);
                    document.Add(title);
                    document.Add(keywords);
                    document.Add(detail);
                    document.Add(sysattr);
                    document.Add(cusattr);
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
