using Pharos.Utility;
using System;

namespace Pharos.Logic.BLL.DataSynchronism.Dtos
{
    /// <summary>
    /// 商品促销
    /// </summary>
    [Excel("主促销信息")]
    public partial class CommodityPromotionForLocal
    {
        [Excel("商品促销Id", 1)]
        public string Id { get; set; }
        /// <summary>
        /// 门店 ID（多个 ID 以,号间隔）
        /// [长度：2000]
        /// [不允许为空]
        /// </summary>
        [Excel("门店", 2)]
        public string StoreId { get; set; }

        /// <summary>
        /// 适用客户群（ 0:不限、 1:内部、2:VIP）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        [Excel("适用客户群", 3)]
        public short CustomerObj { get; set; }

        /// <summary>
        /// 活动起始日期
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        [Excel("活动起始日期", 4)]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 活动结束日期
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        [Excel("活动结束日期", 5)]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 活动时效（ 0:不限、 1:指定时效）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        [Excel("活动时效", 6)]
        public short Timeliness { get; set; }

        /// <summary>
        /// 时效 1（开始）
        /// [长度：5]
        /// </summary>
        [Excel("时效1开始", 7)]
        public string StartAging1 { get; set; }

        /// <summary>
        /// 时效 1（结束）
        /// [长度：5]
        /// </summary>
        [Excel("时效1结束", 8)]
        public string EndAging1 { get; set; }

        /// <summary>
        /// 时效 2（开始）
        /// [长度：5]
        /// </summary>
        [Excel("时效2开始", 9)]
        public string StartAging2 { get; set; }

        /// <summary>
        /// 时效 2（结束）
        /// [长度：5]
        /// </summary>
        [Excel("时效2结束", 10)]
        public string EndAging2 { get; set; }

        /// <summary>
        /// 时效 3（开始）
        /// [长度：5]
        /// </summary>
        [Excel("时效3开始", 11)]
        public string StartAging3 { get; set; }

        /// <summary>
        /// 时效 3（结束）
        /// [长度：5]
        /// </summary>
        [Excel("时效3结束", 12)]
        public string EndAging3 { get; set; }

        /// <summary>
        /// 促销方式（ 1:单品折扣、 2:捆绑促销、 3:组合促销、4:买赠促销、 5:满元促销）
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        [Pharos.Utility.Exclude]
        [Excel("促销方式", 13)]
        public short PromotionType { get; set; }

        /// <summary>
        /// 每天限购次数（ 0:不限）
        /// [长度：5]
        /// [默认值：((0))]
        /// </summary>
        [Excel("每天限购次数", 14)]
        public short RestrictionBuyNum { get; set; }

        /// <summary>
        /// 活动状态（ 0:未开始、 1:活动中、 2:已过期）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        [Pharos.Utility.Exclude]
        [Excel("活动状态", 15)]
        public short State { get; set; }

        /// <summary>
        /// 创建时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// [默认值：(getdate())]
        /// </summary>
        [Pharos.Utility.Exclude]
        [Excel("创建时间", 16)]
        public DateTime CreateDT { get; set; }

        /// <summary>
        /// 创建人 UID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        [Pharos.Utility.Exclude]
        [Excel("创建人", 17)]
        public string CreateUID { get; set; }


    }
}