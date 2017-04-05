﻿
namespace Pharos.POS.Retailing.Models.ApiParams
{
    public class DeviceRegInfo:BaseApiParams
    {
        /// <summary>
        /// 设备类型（1:大POS机、2:PAD、3:Mobile） 
        /// </summary>
        public short Type { get { return 1; } }
        /// <summary>
        /// 设备编码（全局唯一） 
        /// </summary>
        public string DeviceSN { get; set; }
    }
}