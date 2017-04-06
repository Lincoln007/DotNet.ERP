// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-05-22
// 描述信息：用于管理本系统的所有商品促销信息
// --------------------------------------------------

using System;
using System.Runtime.Serialization;
using Pharos.Utility;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 商品促销
    /// </summary>
    [Serializable]

    [Excel("主促销信息")]
    public partial class CommodityPromotion : BasePromotion
    {
    }
}