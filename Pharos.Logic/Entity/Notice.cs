// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-07-10
// 描述信息：用于管理本系统的公告信息
// --------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations.Schema;
using Pharos.Utility;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 公告
    /// </summary>
    [Serializable]
    [Excel("公告信息")]
    public class Notice : SyncEntity
    {


        /// <summary>
        /// 公告主题
        /// [长度：100]
        /// [允许为空]
        /// </summary>
        [Excel("公告主题", 1)]
        public string Theme { get; set; }
        /// <summary>
        /// 公告内容
        /// [长度：1000]
        /// [允许为空]
        /// </summary>
        [Excel("公告内容", 2)]
        public string NoticeContent { get; set; }
        [Excel("公告范围", 3)]
        /// <summary>
        /// 公告范围(门店ID 多个ID 以,号间隔）
        /// [长度：100]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public string StoreId { get; set; }
        /// <summary>
        /// 公告状态（ 0:未发布 1:已发布）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        [Excel("公告状态", 4)]
        [Pharos.Utility.Exclude]
        public short State { get; set; }
        /// <summary>
        /// 创建时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// </summary>
        [Excel("创建时间", 5)]
        public DateTime CreateDT { get; set; }
        /// <summary>
        /// 创建人UID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        [Excel("创建人", 6)]
        public string CreateUID { get; set; }
        /// <summary>
        /// 公告截止日期
        /// </summary>
        [Excel("公告截止日期", 7)]
        public DateTime ExpirationDate { get; set; }
        /// <summary>
        /// 公告开始日期
        /// </summary>
        [Excel("公告开始日期", 8)]
        public DateTime BeginDate { get; set; }

        public string Url { get; set; }
        /// <summary>
        /// 类型（1-公告；2-活动）
        /// </summary>
        public short Type { get; set; }

    }
}