﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 商品组合
    /// </summary>
    public partial class ProductGroup : SyncEntity
    {
        /// <summary>
        /// 对应条码
        /// </summary>
        public string Barcode { get; set; }
        /// <summary>
        /// 组合条码
        /// </summary>
        public string GroupBarcode { get; set; }
        /// <summary>
        /// 组合数量
        /// </summary>
        public decimal Number { get; set; }
        public DateTime CreateDT { get; set; }
        public string CreateUID { get; set; }

    }
}