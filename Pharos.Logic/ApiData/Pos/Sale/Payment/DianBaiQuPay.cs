﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Sale.Payment
{
    public class DianBaiQuPay : BasePay
    {
        public DianBaiQuPay()
            : base(22, PayMode.DianBaiQuPay)
        {
        }
    }
}