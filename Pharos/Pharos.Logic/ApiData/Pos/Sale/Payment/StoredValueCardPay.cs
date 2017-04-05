﻿using Pharos.Logic.ApiData.Pos.DataAdapter;
using Pharos.Logic.ApiData.Pos.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Sale.Payment
{
    public class StoredValueCardPay : BasePay, IBackgroundPaymentWithoutWait
    {
        public StoredValueCardPay()
            : base(16, PayMode.StoredValueCard)
        {
        }

        public object RequestPay()
        {
            if (string.IsNullOrEmpty(PayDetails.CardNo))
            {
                throw new PosException("储值卡支付必须提供卡号！");

            }
            var dataAdapter = DataAdapterFactory.Factory(MachinesSettings.Mode, StoreId, MachineSn, CompanyId, DeviceSn);

            var card = dataAdapter.GetStoredValueCardInfo(PayDetails.CardNo);

            switch (card.State)
            {
                case 0:
                    throw new PosException(string.Format("储值卡{0}未激活！", PayDetails.CardNo));
                case 2:
                    throw new PosException(string.Format("储值卡{0}已挂失！", PayDetails.CardNo));
                case 3:
                    throw new PosException(string.Format("储值卡{0}已作废！", PayDetails.CardNo));
                case 4:
                    throw new PosException(string.Format("储值卡{0}已退卡！", PayDetails.CardNo));
            }
            var member = "未知持卡人";
            if (card.MemberId != null)
            {
                var info = dataAdapter.GetMemberInfo(string.Empty, string.Empty, card.MemberId);
                if (info != null && !string.IsNullOrEmpty(info.RealName))
                {
                    member = info.RealName;
                }
                if (info != null && !string.IsNullOrEmpty(info.MobilePhone))
                {
                    member += "【" + info.MobilePhone + "】";
                }
            }
            if (!string.IsNullOrEmpty(card.Password) && card.Password != PayDetails.CardPassword)
            {
                throw new PosException("支付密码错误，请重试刷卡并输入支付密码！");
            }
            if (card.Balance < PayDetails.Amount)
            {
                throw new PosException("500", string.Format("卡内余额不足，当前余额{0:N2}元", card.Balance), new { Balance = card.Balance, User = member, PayAmount = 0m });
            }
            var balance = card.Balance - PayDetails.Amount;
            dataAdapter.SaveToStoredValueCard(PayDetails.CardNo, PayDetails.Amount, balance);

            return new { Balance = balance, User = member, PayAmount = PayDetails.Amount };
        }

        public bool ConnectTest()
        {
            return true;
        }
    }
}