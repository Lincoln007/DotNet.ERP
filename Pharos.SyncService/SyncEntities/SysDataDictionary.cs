﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SyncService.SyncEntities
{
    [Serializable]
    public class SysDataDictionary : SyncDataObject
    {
        /// <summary>
        /// 编号（该编号全局唯一）
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int DicSN { get; set; }
        /// <summary>
        /// 父编号ID（0：顶级）
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int DicPSN { get; set; }

        /// <summary>
        /// 排序（0:无）
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// 类别名称
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 深度(1:一级、2:二级、3:三级、4:四级、9:具体字典)
        /// [长度：5]
        /// [默认值：((1))]
        /// </summary>
        public short Depth { get; set; }
        /// <summary>
        /// 状态（0:关闭、1:可用）
        /// [长度：1]
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        public bool Status { get; set; }
        public int CompanyId { get; set; }
    }
}