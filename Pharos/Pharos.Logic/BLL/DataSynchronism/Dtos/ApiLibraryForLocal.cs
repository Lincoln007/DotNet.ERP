﻿using Pharos.Utility;
using System;

namespace Pharos.Logic.BLL.DataSynchronism.Dtos
{
    /// <summary>
    /// 集成接口库
    /// </summary>
    [Excel("接口信息")]

    public class ApiLibraryForLocal
    {
        [Excel("接口类型", 1)]
        /// <summary>
        /// 接口类型（ 1:支付接口）
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        public short ApiType { get; set; }

        [Excel("接口名称", 2)]
        /// <summary>
        /// 接口名称
        /// [长度：20]
        /// [不允许为空]
        /// </summary>
        public string Title { get; set; }

        [Excel("接口代码", 3)]
        /// <summary>
        /// 接口代码（全局唯一）
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int ApiCode { get; set; }

        [Excel("接口地址", 4)]
        /// <summary>
        /// 接口地址
        /// [长度：500]
        /// [不允许为空]
        /// </summary>
        public string ApiUrl { get; set; }

        [Excel("接口ICON", 5)]
        /// <summary>
        /// 接口ICON
        /// [长度：200]
        /// [允许为空]
        /// </summary>
        public string ApiIcon { get; set; }

        [Excel("接口顺序", 6)]
        /// <summary>
        /// 接口顺序
        /// [长度：10]
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        public int ApiOrder { get; set; }

        [Excel("备注", 7)]
        /// <summary>
        /// 备注
        /// [长度：200]
        /// [允许为空]
        /// </summary>
        public string Memo { get; set; }

        [Excel("请求方式", 8)]
        /// <summary>
        /// 请求方式[1:post、2:get]
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        public short ReqMode { get; set; }

        [Excel("状态", 9)]
        /// <summary>
        /// 状态（ 0:禁用、 1:可用）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((1))]
        /// </summary>
        public short State { get; set; }

        [Excel("Token", 10)]
        /// <summary>
        /// Token
        /// [长度：100]
        /// [允许为空]
        /// </summary>
        public string ApiToken { get; set; }

        [Excel("账号", 11)]
        /// <summary>
        /// 账号
        /// [长度：50]
        /// [允许为空]
        /// </summary>
        public string ApiAccount { get; set; }

        [Excel("密码", 12)]
        /// <summary>
        /// 密码
        /// [长度：50]
        /// [允许为空]
        /// </summary>
        public string ApiPwd { get; set; }
    }
}