﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Pharos.Utility;
using Pharos.Sys.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharos.Logic.Entity
{
    //[DataContract(IsReference = false)]
    public abstract class BaseProduct : SyncEntity
    {


        ///// <summary>
        ///// 记录ID
        ///// [主键：√]
        ///// [长度：10]
        ///// [不允许为空]
        ///// </summary>
        //[OperationLog("ID", false)]
        //public int Id { get; set; }


        [Excel("货号", 1)]
        /// <summary>
        /// 货号（全局唯一）
        /// [长度：20]
        /// [不允许为空]
        /// </summary>
        [OperationLog("货号", false)]
        public string ProductCode { get; set; }

        [Excel("Barcode", 2)]
        /// <summary>
        /// 条形码（全局唯一）
        /// [长度：30]
        /// [不允许为空]
        /// </summary>
        [OperationLog("条形码", false)]
        public string Barcode { get; set; }

        [Excel("品名", 3)]
        /// <summary>
        /// 品名
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        [OperationLog("品名", false)]
        public string Title { get; set; }

        [Excel("规格", 4)]
        /// <summary>
        /// 规格
        /// [长度：50]
        /// </summary>
        [OperationLog("规格", false)]
        public string Size { get; set; }

        [Excel("品牌", 5)]
        /// <summary>
        /// 品牌SN
        /// [长度：10]
        /// [默认值：((-1))]
        /// </summary>
        [OperationLog("品牌", false)]
        public int? BrandSN { get; set; }

        /// <summary>
        /// 主供应商ID
        /// </summary>
        [OperationLog("主供应商", false)]
        public string SupplierId { get; set; }

        /// <summary>
        /// 产地ID（来自城市ID） 
        /// [长度：10]
        /// [默认值：((-1))]
        /// </summary>
        [OperationLog("产地", false)]
        public int CityId { get; set; }

        /// <summary>
        /// cSN（大类）
        /// [长度：10]
        /// [默认值：((-1))]
        /// </summary>
        [Excel("类别", 6)]
        [OperationLog("品类", false)]
        public int CategorySN { get; set; }
        /// <summary>
        /// 计量大单位ID（来自数据字典表）
        /// [长度：10]
        /// [默认值：((-1))]
        /// </summary>
        [Excel("计量大单位", 9)]
        [OperationLog("计量大单位", true)]
        public int BigUnitId { get; set; }

        /// <summary>
        /// 计量小单位ID（来自数据字典表） 
        /// [长度：10]
        /// [默认值：((-1))]
        /// </summary>
        [Excel("计量小单位", 10)]
        [OperationLog("计量小单位", true)]
        public int SubUnitId { get; set; }

        /// <summary>
        /// 进价
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        [Excel("进价", 11)]
        [OperationLog("进价", false)]
        public decimal BuyPrice { get; set; }

        /// <summary>
        /// 系统售价
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        [Excel("系统售价", 12)]
        [OperationLog("系统售价", false)]
        public decimal SysPrice { get; set; }

        /// <summary>
        /// 产品性质（0:单品、1:组合、2:拆分） 
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        [Excel("产品性质", 13)]
        [OperationLog("产品性质", "0:单品", "1:组合", "2:拆分")]
        public short Nature { get; set; }

        /// <summary>
        /// 对应条码
        /// [长度：30]
        /// </summary>
        [Excel("对应条码", 14)]
        [OperationLog("对应条码", false)]
        public string OldBarcode { get; set; }
        /// <summary>
        /// 可售数量
        /// </summary>
        [OperationLog("可售数量", false)]
        public decimal? SaleNum { get; set; }

        /// <summary>
        /// 物价员（UID）
        /// [长度：40]
        /// [默认值：((-1))]
        /// </summary>
        [OperationLog("物价员", false)]
        public string RaterUID { get; set; }

        /// <summary>
        /// 产品状态（0:已下架、1:已上架）
        /// [长度：5]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        [Excel("产品状态", 15)]
        [OperationLog("产品状态", "0:已下架", "1:已上架")]
        public short State { get; set; }
        /// <summary>
        /// 箱规
        /// </summary>
        [OperationLog("箱规", false)]
        public string BoxBoard { get; set; }
        /// <summary>
        /// 前台优惠（1:允许调价、0:不允许调价)
        /// </summary>
        [Excel("前台优惠", 16)]
        [OperationLog("前台优惠", "1:允许调价", "0:不允许调价")]
        public short? Favorable { get; set; }
        /// <summary>
        /// 保质期（0:不限）
        /// </summary>
        [OperationLog("保质期", false)]
        public short Expiry { get; set; }
        /// <summary>
        /// 保质期单位（1:天、2:月、3:年）
        /// </summary>
        [OperationLog("保质期单位", "1:天", "2:月", "3:年")]
        public short? ExpiryUnit { get; set; }
        /// <summary>
        /// 生产厂家
        /// </summary>
        [OperationLog("生产厂家", false)]
        public string Factory { get; set; }
        /// <summary>
        /// 计价方式（1:计件、2:称重）
        /// </summary>
        [Excel("计价方式", 17)]
        [OperationLog("计价方式", "1:计件", "2:称重")]
        public short ValuationType { get; set; }
        /// <summary>
        /// 退货标志（1:允许、0:不允许）
        /// </summary>
        [Excel("退货标志", 18)]
        [OperationLog("退货标志", "1:允许", "0:不允许")]
        public short? IsReturnSale { get; set; }
        /// <summary>
        /// 订货标志（1:允许、0:不允许）
        /// </summary>
        [OperationLog("订货标志", "1:允许", "0:不允许")]
        public short? IsAcceptOrder { get; set; }
        /// <summary>
        /// 库存预警（数量）
        /// </summary>
        [OperationLog("库存预警下限", false)]
        public short? InventoryWarning { get; set; }
        /// <summary>
        /// 保质期预警（天）
        /// </summary>
        [OperationLog("保质期预警", false)]
        public short? ValidityWarning { get; set; }
        /// <summary>
        /// 库存预警（数量）
        /// </summary>
        [OperationLog("库存预警上限", false)]
        public short? InventoryWarningMax { get; set; }
        /// <summary>
        /// 批发价
        /// </summary>
        [OperationLog("批发价", false)]
        public decimal TradePrice { get; set; }
        /// <summary>
        /// 加盟价
        /// </summary>
        [OperationLog("加盟价", false)]
        public decimal JoinPrice { get; set; }
        /// <summary>
        /// 一品多码
        /// </summary>
        [OperationLog("一品多码", false)]
        public string Barcodes { get; set; }
        /// <summary>
        /// 多条码串
        /// </summary>
        [OperationLog("多条码串", false)]
        public string BarcodeMult { get; set; }
        /// <summary>
        /// 进项税率
        /// </summary>
        [OperationLog("进项税率", false)]
        public decimal? StockRate { get; set; }
        /// <summary>
        /// 销售税率
        /// </summary>
        [OperationLog("销售税率", false)]
        public decimal? SaleRate { get; set; }
        /// <summary>
        /// 是否发生业务关系(0:否,1:是)
        /// </summary>
        [OperationLog("是否发生业务关系", "0:否", "1:是")]
        public bool IsRelationship { get; set; }

        public DateTime? CreateDT { get; set; }

        /// <summary>
        /// 名称扩展描述（用于自动完成）
        /// </summary>
        [NotMapped]
        public string TitleExt { get { return _TitleExt??Title; } set { _TitleExt = value; } }
        string _TitleExt = null;
    }
}