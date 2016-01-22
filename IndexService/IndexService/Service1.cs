using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Common.Logging;
using IndexService.Job;
using Quartz.Impl;
using System.IO;
using IndexService.Common;
using IndexService.Model;
using log4net;
using System.Reflection;
using System.Configuration;
using IndexService.Service;
using IndexService.Interface;

namespace IndexService
{
    public partial class Service1 : ServiceBase
    {
        private IScheduler _ische;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Utility.writelog("服务开始");
            string time = ConfigurationManager.AppSettings["indextime"];
            int indexHour = Convert.ToInt32((time.Split(':'))[0]);
            int indexMinute = Convert.ToInt32((time.Split(':'))[1]);
            ISchedulerFactory sf = new StdSchedulerFactory();
            _ische = sf.GetScheduler();
            JobDetail job1 = new JobDetail("job1", "group1", typeof(ProductJob));

            Trigger triger1 = TriggerUtils.MakeDailyTrigger("triger1", indexHour, indexMinute);
            triger1.JobName = "job1";
            triger1.JobGroup = "group1";
            triger1.Group = "group1";

            _ische.ScheduleJob(job1, triger1);
            _ische.Start();
           
        }

        protected override void OnStop()
        {
            _ische.Shutdown();
            Utility.writelog("服务结束");
        }
    }
}
