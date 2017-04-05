﻿using Newtonsoft.Json.Linq;
using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.Models;
using QCT.Pay.Common;
using QCT.Pay.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pharos.Utility.Helpers;
using Pharos.Logic.OMS.BLL;
using Newtonsoft.Json;
using QCT.Pay.Common.Helpers;
using Pharos.Logic.OMS;

namespace QCT.Api.Pay.Utils
{
    /// <summary>
    /// 创建订单基础类
    /// </summary>
    /// <typeparam name="ReqModel"></typeparam>
    public class OrderBuilder<TReqModel, TRspModel> : BasePayBuilder
        where TReqModel : BaseTradeRequest
        where TRspModel : BaseTradeResponse
    {
        /// <summary>
        /// 初始化实力对象
        /// </summary>
        /// <returns></returns>
        public static OrderBuilder<TReqModel, TRspModel> Create()
        {
            return new OrderBuilder<TReqModel, TRspModel>();
        }

        /// <summary>
        /// 商户信息
        /// </summary>
        public MerchantChannelModel MerchModel { get; set; }
        /// <summary>
        /// 商户门店信息
        /// </summary>
        public MerchantStoreChannelModel MerchStoreModel { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OutTradeNo { get; set; }
        /// <summary>
        /// 请求参数Model
        /// </summary>
        public TReqModel ReqModel { get; set; }
        /// <summary>
        /// 响应参数Model
        /// </summary>
        public TRspModel RspModel { get; set; }
        /// <summary>
        /// 创建生成订单
        /// </summary>
        /// <param name="reqModel"></param>
        /// <returns></returns>
        public virtual QctPayReturn Build(TReqModel reqModel)
        {
            return QctPayReturn.Fail();
        }
        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="reqModel"></param>
        /// <returns></returns>
        public virtual object Query(TReqModel reqModel)
        {
            return QctPayReturn.Fail();
        }
        /// <summary>
        /// 返回结果处理
        /// </summary>
        /// <param name="rst"></param>
        /// <returns></returns>
        public object Result(QctPayReturn rst)
        {
            if (rst.Successed)
                return rst.Data;
            else
                return rst;
        }
        /// <summary>
        /// 获取商户门店信息
        /// </summary>
        /// <returns></returns>
        public MerchantStoreChannelModel GetMerchStore()
        {
            var storeObj = PaySvc.GetMerchStore(MerchModel, ReqModel.Store_Id);
            WithMerchStoreModel(storeObj);
            return storeObj;
        }
        /// <summary>
        /// 赋值请求参数
        /// </summary>
        /// <param name="reqModel"></param>
        public void WithReqModel(TReqModel reqModel)
        {
            ReqModel = reqModel;
        }
        /// <summary>
        /// 赋值响应参数
        /// </summary>
        /// <param name="rspModel"></param>
        public void WithRspModel(TRspModel rspModel)
        {
            RspModel = rspModel;
        }
        /// <summary>
        /// 赋值商户信息Model
        /// </summary>
        /// <param name="merchModel"></param>
        public void WithMerchModel(object merchModel)
        {
            MerchModel = (MerchantChannelModel)merchModel;
        }
        /// <summary>
        /// 赋值商户门店信息Model
        /// </summary>
        /// <param name="merchStoreModel"></param>
        public void WithMerchStoreModel(MerchantStoreChannelModel merchStoreModel)
        {
            MerchStoreModel = merchStoreModel;
        }
        /// <summary>
        /// 赋值订单号
        /// </summary>
        /// <param name="outTradeNo"></param>
        public void WithOutTradeNo(string outTradeNo)
        {
            OutTradeNo = outTradeNo;
        }
        /// <summary>
        /// 是否可创建交易订单
        /// </summary>
        /// <returns></returns>
        public QctPayReturn CanBuilder()
        {
            var result = CanAccess();
            if (!result.Successed)
                return result;

            result = CanCreateOrder();
            if (!result.Successed)
                return result;

            return QctPayReturn.Success();
        }
        /// <summary>
        /// 是否有访问接口权限
        /// </summary>
        /// <returns></returns>
        public QctPayReturn CanAccess()
        {
            var result = VerifyParams();
            if (!result.Successed)
                return result;

            result = VerifyMerchAccess();
            if (!result.Successed)
                return result;

            var merchStoreModel = GetMerchStore();
            if (merchStoreModel == null)
                return QctPayReturn.Fail("找不到商户门店信息");

            return QctPayReturn.Success();
        }
        /// <summary>
        /// 判断订单号是否重复
        /// </summary>
        /// <returns></returns>
        public QctPayReturn CanCreateOrder()
        {
            var can = PaySvc.CanCreateOrder(ReqModel.Mch_Id, OutTradeNo);
            if (can)
                return QctPayReturn.Success();
            else
                return QctPayReturn.Fail(msg: "订单号已存在，不能重复提交");
        }
        /// <summary>
        /// 验证参数格式
        /// </summary>
        /// <returns></returns>
        public QctPayReturn VerifyParams()
        {
            var errorMsg = ReqModel.TryValidateObject();
            if (errorMsg.IsNullOrEmpty())
                return QctPayReturn.Success();
            else
                return QctPayReturn.Fail(code: PayConst.FAIL_CODE_40001, msg: errorMsg);
        }
        /// <summary>
        /// 验证商户访问权限（1、支付许可权限；2、商家支付通道权限；3、商家接口通道权限；）
        /// </summary>
        /// <returns></returns>
        public QctPayReturn VerifyMerchAccess()
        {
            var result = PaySvc.CheckMerchAccess(ReqModel.Mch_Id, ReqModel.Method, ReqModel.Version.ToType<decimal>());
            WithMerchModel(result.Data);
            return result;
        }
        /// <summary>
        /// 发送请求
        /// </summary>
        /// <returns></returns>
        public QctPayReturn SendPost<TSxfReqModel, TSxfRspModel>(string url, TSxfReqModel sxfReqModel)
            where TSxfReqModel : SxfBaseTradeRequest
            where TSxfRspModel : SxfBaseTradeResponse
        {
            //var resultStr = string.Empty;
            try
            {
                var postResult = PayHelper.SendPost(url, PaySignHelper.ToDicAndSign(sxfReqModel, MerchModel.SecretKey3, "signature"));
                if (postResult.Successed)
                {
                    var resultObj = JsonConvert.DeserializeObject<TSxfRspModel>(postResult.Data.ToString());
                    if (resultObj.IsSuccess())
                    {
                        return QctPayReturn.Success(data: resultObj);
                    }
                    else
                    {
                        //处理返回失败结果
                        LogEngine.WriteError(string.Format("发送交易请求成功但返回交易错误信息：请求参数：{0}，返回参数：{1}]", sxfReqModel.ToJson(), postResult), null, LogModule.支付交易);
                        if (string.IsNullOrWhiteSpace(resultObj.RspMsg))
                        {
                            resultObj.RspMsg = "服务器请求失败";
                        }
                        return QctPayReturn.Fail(PayTradeHelper.TransCodeBySxf(resultObj.RspCod), resultObj.RspMsg);
                    }
                }
                else
                    return postResult;
            }
            catch (Exception ex)
            {
                return ResultFail(msg: "订单请求失败", logMsg: string.Format("发送交易请求异常：服务器异常，请求参数：{0}，异常信息：{1}]", sxfReqModel.ToJson(), ex.Message));
            }
        }
        /// <summary>
        /// 返回错误信息，并记录日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="logMsg"></param>
        /// <returns></returns>
        public QctPayReturn ResultFail(string msg, string logMsg)
        {
            LogEngine.WriteError(logMsg, null, LogModule.支付交易);
            return QctPayReturn.Fail(code: PayConst.FAIL_CODE_40004, msg: msg);
        }
    }
}