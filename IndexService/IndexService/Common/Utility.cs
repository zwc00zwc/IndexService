using IndexService.Model;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndexService.Common;
using System.Configuration;
using System.Xml;
using System.Xml.Linq;

namespace IndexService.Common
{
    public class Utility
    {
        private static readonly string filename = ConfigurationManager.AppSettings["SuccNum"];
        private static string Getpath
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", filename); }
        }
        public static void writelog(string str)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("indexlog");
            log.Info(str);
        }

        /// <summary>
        /// 读取上次生成结束时的数字
        /// </summary>
        /// <returns></returns>
        public static long GetSuccNum()
        {
            long SuccessNum = 0;
            try
            {
                XmlDocument doc = new XmlDocument();
                using (XmlReader xr = XmlReader.Create(Getpath))
                {
                    doc.Load(xr);
                }
                XmlNodeList sl = doc.GetElementsByTagName("SuccNum");
                if (sl.Count == 1)
                {
                    if (!string.IsNullOrEmpty(sl[0].InnerText))
                    {
                        SuccessNum = long.Parse(sl[0].InnerText);
                    }
                }
            }
            catch (Exception ex)
            {
                writelog("获取上次结束数字出错:" + ex.ToString());
            }
            return SuccessNum;
        }

        /// <summary>
        /// 设置新的结束数字
        /// </summary>
        /// <param name="succ"></param>
        public static void SetSuccNem(long succ)
        {
            try
            {
                XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", null), new XElement("SuccNum", succ));
                XmlWriterSettings setting = new XmlWriterSettings();
                setting.Indent = true;
                using (XmlWriter xw = XmlWriter.Create(Getpath, setting))
                {
                    doc.WriteTo(xw);
                }
            }
            catch (Exception ex)
            {
                writelog("获取上次结束数字出错:" + ex.ToString());
            }
        }
    }
}
