﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Barcode.Retailing.Dtos
{
    public class ProductCategoryDto
    {
        /// <summary>
        /// 分类编号（全局唯一） 
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int CategorySN { get; set; }
        /// <summary>
        /// 上级分类SN
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int CategoryPSN { get; set; }
        /// <summary>
        /// 分类层级（1:顶级、2：二级、3:三级、4:四级）
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        public short Grade { get; set; }
        /// <summary>
        /// 分类名称
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        public string Title { get; set; }
    }
}