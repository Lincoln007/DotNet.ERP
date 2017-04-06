// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-06-05
// 描述信息：用于管理本系统的商品折扣信息（主表： CommodityPromotion）
// --------------------------------------------------

using System;
using System.Runtime.Serialization;
using Pharos.Utility;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 单品折扣
    /// </summary>
    [Serializable]
    [Excel("单品折扣")]
    public class CommodityDiscount : SyncEntity
    {

        /// <summary>
        /// 促销 ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        [Pharos.Utility.Exclude]
        [Excel("促销ID", 1)]
        public string CommodityId { get; set; }


        /// <summary>
        /// 单品条码
        /// [长度：30]
        /// </summary>
        [Excel("单品条码", 2)]
        public string Barcode { get; set; }


        /// <summary>
        /// 商品系列 ID
        /// [长度：10]
        /// </summary>
        [Excel("商品系列", 3)]
        public int CategorySN { get; set; }


        /// <summary>
        /// 折扣率（ %）
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((100))]
        /// </summary>
        [Excel("折扣率", 4)]
        public decimal? DiscountRate { get; set; }


        /// <summary>
        /// 折后价
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// </summary>
        [Excel("折后价", 5)]
        public decimal? DiscountPrice { get; set; }


        /// <summary>
        /// 起购量
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        [Excel("起购量", 6)]
        public decimal MinPurchaseNum { get; set; }
        /// <summary>
        /// 折扣方式 1-固定量（叠加）,2-起购量,3-固定量（不叠加）
        /// </summary>
        public short Way { get; set; }

        /// <summary>
        /// 类别层级
        /// </summary>
        public short? CategoryGrade { get; set; }

    }
}