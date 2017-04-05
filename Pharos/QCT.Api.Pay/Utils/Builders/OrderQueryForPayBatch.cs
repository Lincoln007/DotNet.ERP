﻿using Newtonsoft.Json.Linq;
using Pharos.Logic.OMS;
using Pharos.Logic.OMS.Models;
using QCT.Pay.Common.Helpers;
using QCT.Pay.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pharos.Utility.Helpers;
using QCT.Pay.Common;

namespace QCT.Api.Pay.Utils
{
    /// <summary>
    /// 支付订单分页查询
    /// </summary>
    public class OrderQueryForPayBatch : OrderBuilder<PayBatchQueryRequest, PayBatchQueryResponse>
    {
        /// <summary>
        /// 支付订单分页查询
        /// </summary>
        /// <param name="reqModel"></param>
        /// <returns></returns>
        public override object Query(PayBatchQueryRequest reqModel)
        {
            try
            {
                var canObj = CanAccess();
                if (!canObj.Successed)
                    return canObj;
                var sxfReq = new SxfPayBatchQueryRequest(reqModel, MerchStoreModel);
                //sxf签名并请求
                var sxfResult = PayHelper.SendPost(MerchStoreModel.ApiUrl, PaySignHelper.ToDicAndSign(sxfReq, MerchModel.SecretKey3, "signature"));
                if (sxfResult.Successed)
                {
                    //处理返回结果
                    var sxfJObj = JObject.Parse(HttpUtility.UrlDecode(sxfResult.Data.ToString()));
                    var sxfResultRsp = sxfJObj.ToObject<SxfPayBatchQueryResponse>();

                    if (sxfResultRsp.IsSuccess())
                    {
                        var result = sxfResultRsp.ToPayBatchQueryRsp(MerchStoreModel);
                        //Qct签名
                        var resultDic = PaySignHelper.ToDicAndSign(result, MerchModel.SecretKey, "sign");
                        return resultDic;
                    }
                    else
                    {
                        LogEngine.WriteError(string.Format("支付订单分页查询请求错误：请求参数：{0}，返回参数：{1}", sxfResultRsp.ToJson(), sxfResult.ToJson()), null, LogModule.支付交易);
                        var rst = QctPayReturn.Fail(PayConst.FAIL_CODE_40004, sxfResultRsp.RspMsg);
                        return rst;
                    }
                }
                else
                    return sxfResult;
            }
            catch (Exception ex)
            {
                LogEngine.WriteError(string.Format("支付订单分页查询请求异常：{0}，请求参数：{1}", ex.Message, reqModel.ToJson()), null, LogModule.支付交易);
                var rst = QctPayReturn.Fail();
                return rst;
            }
        }
    }
}