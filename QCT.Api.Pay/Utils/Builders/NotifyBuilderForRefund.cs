﻿using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.Models;
using QCT.Pay.Common;
using QCT.Pay.Common.Helpers;
using QCT.Pay.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QCT.Api.Pay.Utils
{
    /// <summary>
    /// 创建生成退款通知类
    /// </summary>
    public class NotifyBuilderForRefund : NotifyBuilder<SxfRefundNotifyRequest>
    {
        /// <summary>
        /// 创建生成退款通知
        /// </summary>
        /// <param name="reqModel"></param>
        /// <returns></returns>
        public override SxfPayReturn Build(SxfRefundNotifyRequest reqModel)
        {
            TradeOrder tradeOrder = null;
            var rfdTradeResult = new TradeResult(reqModel);
            //保存通知结果并更改TradeOrder状态
            var success = PaySvc.SaveMchTradeResult(rfdTradeResult, out tradeOrder);
            if (!success)
                return SxfPayReturn.Fail(msg: "数据接收失败");
            else
            {
                var rfdNotify = new NotifyRefundRequest(tradeOrder, rfdTradeResult);
                var rfdNotifyDic = PaySignHelper.ToASCIIDictionary(rfdNotify);
                return SendPost(PayConst.QCTTRADE_NOTIFY_REFUND, rfdNotifyDic, tradeOrder.CID, tradeOrder.RfdNotifyUrl);
            }
        }
    }
}