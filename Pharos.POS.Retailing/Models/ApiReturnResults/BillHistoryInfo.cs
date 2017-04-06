﻿using System;
using System.Collections.Generic;

namespace Pharos.POS.Retailing.Models.ApiReturnResults
{
    public class BillHistoryInfo
    {
        public IEnumerable<BillDetails> Details { get; set; }
        /// <summary>
        /// 实收金额
        /// </summary>
        public decimal TotalAmount { get; set; }
        public decimal WipeZeroAfterTotalAmount { get; set; }

        /// <summary>
        /// 优惠总额
        /// </summary>
        public decimal PreferentialAmount { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public string Payment { get; set; }

        /// <summary>
        /// 商品件数
        /// </summary>
        public decimal ProductCount { get; set; }

        /// <summary>
        ///流水号
        /// </summary>
        public string PaySn { get; set; }
        /// <summary>
        /// 门店
        /// </summary>
        public string StoreId { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        public string MachineSn { get; set; }
        /// <summary>
        /// 订单时间
        /// </summary>
        public DateTime OrderTime { get; set; }
        /// <summary>
        /// 收银员员工编号
        /// </summary>
        public string CashierId { get; set; }

        public string CashierName { get; set; }
        /// <summary>
        /// 实收金额
        /// </summary>
        public decimal Received { get; set; }

        public decimal Change { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        public int OrderType { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public int OrderStatus { get; set; }
        /// <summary>
        /// 导购员
        /// </summary>
        public string SaleManName { get; set; }
        /// <summary>
        /// 导购员编号
        /// </summary>
        public string SaleManUserCode { get; set; }
        /// <summary>
        /// 退单时间
        /// </summary>
        public DateTime? ReturnDT { get; set; }

        public decimal WipeZero { get; set; }
        /// <summary>
        /// 整单折扣金额
        /// </summary>
        public decimal OrderDiscount { get; set; }

        /// <summary>
        /// 该单是否已退单
        /// </summary>
        public bool IsRefundOrder { get; set; }

        public string OldOrderSN { get; set; }
        /// <summary>
        /// 订单支付方式
        /// </summary>
        public string ApiCodes { get; set; }
        /// <summary>
        /// 会员卡支付时剩余的余额
        /// </summary>
        public List<Dictionary<string, string>> CardAndBalances { get; set; }

    }
}