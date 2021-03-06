﻿using System;
using System.Collections.Generic;

namespace Pharos.POS.Retailing.Models.ApiReturnResults
{
    public class SalesmanDayReportResult
    {
        /// <summary>
        /// 首笔时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 末笔时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 收银员
        /// </summary>
        public string Salesman { get; set; }
        /// <summary>
        /// 收银员工号
        /// </summary>
        public string UserCode { get; set; }
        /// <summary>
        /// 收银员销售日结记录
        /// </summary>
        public SalesmanDayReportSaleResult Sale { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<DayReportDetailItem> Other { get; set; }
        /// <summary>
        /// 剩余现金
        /// </summary>
        public decimal Cash { get; set; }

    }
    public class SalesmanDayReportSaleResult
    {
        public DayReportDetailItem SaleInfo { get; set; }

        public List<PayWayItem> PayWay { get; set; }
    }

    public class PayWayItem
    {
        public string Title { get; set; }

        public decimal Amount { get; set; }
    }
}
