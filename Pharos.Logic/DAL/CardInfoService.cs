﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.Entity;
using Pharos.Sys.Entity;
using Pharos.Utility;
using Pharos.Utility.Helpers;

namespace Pharos.Logic.BLL
{
    public class CardInfoService : BaseService<CardInfo>
    {
        /// <summary>
        /// 新建卡类型
        /// </summary>
        /// <returns></returns>
        public OpResult CreateMemberCardType(CardInfo cardinfo)
        {
            return CardInfoService.Add(cardinfo, true);
        }

        /// <summary>
        /// 获取所有的卡信息
        /// </summary>
        /// <returns></returns>
        public object FindMemberCardTypeList(out int count)
        {
            var query = (from a in CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId)
                         join b in BaseService<SysUserInfo>.CurrentRepository.Entities.Where(o => o.CompanyId == CommonService.CompanyId)
                         on a.CreateUID equals b.UID into l
                         from def in l.DefaultIfEmpty()
                         select new
                         {
                             Id = a.Id,
                             CardType = a.CardType,
                             CardName = a.CardName,
                             CardTypeId = a.CardTypeId,
                             Category = a.Category,
                             MinRecharge = a.MinRecharge,
                             // DefaultIntegr = a.DefaultIntegr,
                             DefaultPrice = a.DefaultPrice,
                             Memo = a.Memo,
                             State = a.State,
                             CreateDT = a.CreateDT,
                             CreateUID = l.FirstOrDefault() == null ? "" : l.FirstOrDefault().FullName
                         });
            count = query.Count();
            if (count > 0)
            {
                //return CurrentRepository.FindList(o => o.CompanyId == CommonService.CompanyId).ToPageList();
                var result = query.ToPageList();

                return result;
            }
            else
            {
                return new CardInfo();
            }
        }
        /// <summary>
        /// 更改卡类型状态
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="id">卡类型id</param>
        /// <returns></returns>
        public OpResult UpdateState(short state, string ids)
        {
            try
            {
                var cardInfos = CurrentRepository.QueryEntity.Where(o => ("," + ids + ",").Contains("," + o.Id + ",")).ToList();

                if (!string.IsNullOrEmpty(ids) && cardInfos.Count() == 0)
                    return new OpResult() { Successed = false, Message = "未找到该卡类型！" };

                foreach (var item in cardInfos)
                {
                    if (state != item.State)
                    {
                        if (item.State != state)
                        {
                            item.State = state;
                        }
                        //CurrentRepository.Update(item, false);
                    }
                }
                CurrentRepository.Update(new CardInfo());
                return new OpResult() { Successed = true, Message = "操作成功！" };
            }
            catch (Exception e)
            {
                Log.WriteError(e);
                throw e;
            }


        }


        public List<CardInfo> GetAllMemberCardType()
        {
            return CurrentRepository.Entities.Where(o => o.CompanyId == CommonService.CompanyId && o.State == 0).ToList();
        }
        //
        public OpResult UpdateMemberCardType(CardInfo cardInfo)
        {
            var currentInfo = CurrentRepository.Entities.Where(o => o.CompanyId == CommonService.CompanyId && cardInfo.Id == o.Id).FirstOrDefault();
            if (currentInfo != null)
            {
                currentInfo.CardName = cardInfo.CardName;
                currentInfo.CardType = cardInfo.CardType;
                //currentInfo.CardTypeId = cardInfo.CardTypeId;
                currentInfo.Category = cardInfo.Category;
                //currentInfo.CouponType = cardInfo.CouponType;
                //currentInfo.DefaultIntegr = cardInfo.DefaultIntegr;
                currentInfo.DefaultPrice = cardInfo.DefaultPrice;
                //currentInfo.Discount = cardInfo.Discount;
                //currentInfo.IntegrationType = cardInfo.IntegrationType;
                currentInfo.Memo = cardInfo.Memo;
                currentInfo.MinRecharge = cardInfo.MinRecharge;
                currentInfo.State = cardInfo.State;
                CurrentRepository.Update(currentInfo);
                return new OpResult() { Successed = true, Message = "操作成功！" };

            }
            else
            {
                return new OpResult() { Successed = false, Message = "未找到该数据！" };
            }
        }
    }
}
