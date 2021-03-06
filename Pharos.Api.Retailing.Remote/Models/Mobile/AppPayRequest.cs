﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pharos.Api.Retailing.Models.Mobile
{
    /// <summary>
    /// 扫码支付请求参数
    /// </summary>
    public class AppPayRequest : BaseParams
    {
        /// <summary>
        /// 门店
        /// </summary>
        public string StoreId { get; set; }

        /// <summary>
        /// 商户终端设备号
        /// </summary>
        public string Device_id { get; set; }
        /// <summary>
        /// 消费金额
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 会员卡号
        /// </summary>
        public string CardSN { get; set; }
        /// <summary>
        /// 使用积分
        /// </summary>
        public decimal UseIntegral { get; set; }
        /// <summary>
        /// 抵扣金额
        /// </summary>
        public decimal DiscountAmount { get; set; }
        /// <summary>
        /// 实收金额
        /// </summary>
        public decimal Received { get; set; }
        /// <summary>
        /// 获取积分
        /// </summary>
        public decimal GetIntegral { get; set; }
        /// <summary>
        /// 支付二维码字符串
        /// </summary>
        public string PayToken { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public string MemberId { get; set; }
        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; }
        /// <summary>
        /// 当前用户编码
        /// </summary>
        public string UserCode { get; set; }
        /// <summary>
        /// 订单备注
        /// </summary>
        public string Remark { get; set; }
    }
}