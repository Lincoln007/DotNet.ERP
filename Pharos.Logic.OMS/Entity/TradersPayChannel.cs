// --------------------------------------------------
// Copyright (C) 2017 版权所有
// 创 建 人：
// 创建时间：
// 描述信息：
// --------------------------------------------------

using System;

namespace Pharos.Logic.OMS.Entity
{
    /// <summary>
    /// 用于管理本系统商家支付通道信息
    /// </summary>
    [Serializable]
    public class TradersPayChannel
    {
        /// <summary>
        /// 记录ID
        /// [主键：√]
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        private int _Id;

        /// <summary>
        /// 商家支付通道ID（GUID）
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string TPayChannelId
        {
            get { return _TPayChannelId; }
            set { _TPayChannelId = value; }
        }
        private string _TPayChannelId;

        /// <summary>
        /// 商家支付密钥ID（TradersPaySecretKey表TPaySecrectId）
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string TPaySecrectId
        {
            get { return _TPaySecrectId; }
            set { _TPaySecrectId = value; }
        }
        private string _TPaySecrectId;

        /// <summary>
        /// 支付方式（PayChannelDetails表ChannelPayMode）
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        public short ChannelPayMode
        {
            get { return _ChannelPayMode; }
            set { _ChannelPayMode = value; }
        }
        private short _ChannelPayMode;

        /// <summary>
        /// 支付结果通知URL（给平台回调）
        /// [长度：200]
        /// </summary>
        public string PayNotifyUrl
        {
            get { return _PayNotifyUrl; }
            set { _PayNotifyUrl = value; }
        }
        private string _PayNotifyUrl;

        /// <summary>
        /// 退款结果通知URL（给平台回调）
        /// [长度：200]
        /// </summary>
        public string RfdNotifyUrl
        {
            get { return _RfdNotifyUrl; }
            set { _RfdNotifyUrl = value; }
        }
        private string _RfdNotifyUrl;

        /// <summary>
        /// 通道状态（0：未启用，1：已启用）
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        public short State
        {
            get { return _State; }
            set { _State = value; }
        }
        private short _State;

        /// <summary>
        /// 创建人UID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string CreateUID
        {
            get { return _CreateUID; }
            set { _CreateUID = value; }
        }
        private string _CreateUID;

        /// <summary>
        /// 创建时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// </summary>
        public DateTime CreateDT
        {
            get { return _CreateDT; }
            set { _CreateDT = value; }
        }
        private DateTime _CreateDT;

        /// <summary>
        /// 修改人UID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string ModifyUID
        {
            get { return _ModifyUID; }
            set { _ModifyUID = value; }
        }
        private string _ModifyUID;

        /// <summary>
        /// 修改时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// </summary>
        public DateTime ModifyDT
        {
            get { return _ModifyDT; }
            set { _ModifyDT = value; }
        }
        private DateTime _ModifyDT;
    }
}