﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Sys.DAL;
using Pharos.Sys.Entity;
using Pharos.Utility;
using Pharos.Sys.EntityExtend;
using System.Data;

namespace Pharos.Sys.BLL
{
    public class SysPaymentSettingBLL 
    {
        private SysPaymentSettingDAL _dal = new SysPaymentSettingDAL();
        /// <summary>
        /// 获取支付配置信息列表
        /// </summary>
        /// <returns></returns>       
        public DataTable GetList(Paging paging, int PayType, string StoreId, int State)
        {
            return _dal.GetList(paging, PayType, StoreId, State);
        }

        /// <summary>
        /// 根据Id获取支付配置信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SysPaymentSetting GetModel(int id)
        {
            var data = _dal.GetById(id);
            if (data == null)
            {
                data = new SysPaymentSetting();
                data.State = 1;
            }
            return data;
        }

        /// <summary>
        /// 保存支付配置信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public OpResult SaveModel(SysPaymentSetting model)
        {
            var result = OpResult.Fail("数据保存失败!");
            try
            {
                if (_dal.ExistsById(model.Id))
                {
                    var re = _dal.Update(model);
                    if (re) { result = OpResult.Success("数据保存成功"); }
                }
                else
                {
                    var re = _dal.Insert(model);
                    if (re > 0) { result = OpResult.Success("数据保存成功"); }
                }
            }
            catch (Exception ex)
            {
                result = OpResult.Fail("数据保存失败!" + ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public OpResult SetState(string Ids, short state)
        {
            var result = OpResult.Fail("状态变更失败");
            try
            {
                var ids = Ids.Split(',');
                for (int i = 0; i < ids.Count(); i++)
                {
                    var id = int.Parse(ids[i]);
                    _dal.SetState(id, state);
                }
                result = OpResult.Success("数据保存成功");
            }
            catch (Exception e)
            {
                result = OpResult.Fail("状态变更失败" + e.Message);
            }
            return result;
        }

        /// <summary>
        /// 判断门店是否已有支付方式
        /// </summary>
        /// <param name="PayType"></param>
        /// <param name="StoreId"></param>
        /// <returns></returns>
        public string IsExitStore(int PayType, string StoreId, bool IsUpdate, int Id)
        {
            string isExitStores = _dal.IsExitStore(PayType, StoreId, IsUpdate, Id);
            return isExitStores;
        }
        /// <summary>
        /// 获取最新配置信息
        /// </summary>
        /// <param name="payType">（1：支付宝，2：微信)</param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public SysPaymentSetting GetPaymentSettingBystoreId(int payType, string storeId,int? companyId)
        {
            if (string.IsNullOrWhiteSpace(storeId))
                throw new ArgumentNullException("门店编号不能为空");
            if (!companyId.HasValue)
                throw new ArgumentNullException("企业编号不能为空");
            var setting = _dal.GetPaymentSettingBystoreId(payType, storeId, companyId.Value);
            if (setting == null) throw new Exception("该门店未配置支付信息");
            return setting;
        }
    }
}