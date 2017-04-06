﻿using Pharos.Logic.ApiData.Pos.Sale.AfterSale;
using Pharos.Logic.ApiData.Pos.ValueObject;
using System.Collections.Generic;

namespace Pharos.Api.Retailing.Models.Pos
{
    public class RefundRequest : BaseApiParams
    {
        public int Reason { get; set; }
        public string PaySn { get; set; }
        public decimal Amount { get; set; }

        public AfterSaleMode Mode { get; set; }
    }
}