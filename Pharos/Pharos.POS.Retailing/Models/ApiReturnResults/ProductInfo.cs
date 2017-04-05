﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.POS.Retailing.Models.ApiReturnResults
{
    public class ProductInfo
    {
        /// <summary>
        /// 档案主条码
        /// </summary>
        public string MainBarcode { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public string Brand { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 商品条码
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 是否允许修改销售数量
        /// </summary>
        public bool EnableEditNum { get; set; }
        /// <summary>
        /// 是否允许修改售价
        /// </summary>
        public bool EnableEditPrice { get; set; }
        /// <summary>
        /// 系统售价
        /// </summary>
        public decimal SystemPrice { get; set; }
        /// <summary>
        /// 进价
        /// </summary>
        public decimal BuyPrice { get; set; }
        public int ProductType { get; set; }


        public string MultiCodes { get; set; }
        public IEnumerable<string> MultiCode { get; set; }
    }
}