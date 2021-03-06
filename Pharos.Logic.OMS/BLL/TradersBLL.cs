﻿using Pharos.Utility;
using Pharos.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Pharos.Logic.OMS.DAL;
using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.EntityExtend;
using Pharos.Logic.OMS.Entity.View;

namespace Pharos.Logic.OMS.BLL
{
    public class TradersBLL
    {
        private TradersDAL _dal = new TradersDAL();

        TradersService tradersService
        {
            get
            {
                return new TradersService();
            }
        }

        /// <summary>
        /// 获取客户汇总
        /// </summary>
        /// <param name="nvl"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public DataTable GetList(System.Collections.Specialized.NameValueCollection nvl, out object footer, out int recordCount, out List<ViewSysData> l1, out List<ViewSysData> l2, out List<ViewSysData> l3)
        {
            //省份
            var CurrentProvinceId = (nvl["CurrentProvinceId"] ?? "").Trim();
            //城市
            var CurrentCityId = (nvl["CurrentCityId"] ?? "").Trim();
            //登记日期（开始）
            var CreateDT_begin = (nvl["CreateDT_begin"] ?? "").Trim();
            //登记日期（结束）
            var CreateDT_end = (nvl["CreateDT_end"] ?? "").Trim();
            //跟踪状态
            var TrackStautsId = (nvl["TrackStautsId"] ?? "").Trim();
            //业务员
            var AssignerUID = (nvl["AssignerUID"] ?? "").Trim();
            //客户类型
            var BusinessModeId = (nvl["BusinessModeId"] ?? "").Trim();
            //经营范围
            var BusinessScopeId = (nvl["BusinessScopeId"] ?? "").Trim();

            var pageIndex = 1;
            var pageSize = 50;
            if (!nvl["page"].IsNullOrEmpty())
                pageIndex = int.Parse(nvl["page"]);
            if (!nvl["rows"].IsNullOrEmpty())
                pageSize = int.Parse(nvl["rows"]);
            if (!nvl["noPage"].IsNullOrEmpty())
                pageSize = 10000;

            string strw = "";
            if (!CurrentProvinceId.IsNullOrEmpty() && CurrentProvinceId != "0")
            {
                strw = strw + " and t.CurrentProvinceId=" + CurrentProvinceId;
            }
            if (!CurrentCityId.IsNullOrEmpty() && CurrentCityId != "0")
            {
                strw = strw + " and t.CurrentCityId=" + CurrentCityId;
            }
            if (!CreateDT_begin.IsNullOrEmpty())
            {
                string c = CreateDT_begin + " " + "00:00:00";
                strw = strw + " and t.CreateDT >='" + c + "'";
            }
            if (!CreateDT_end.IsNullOrEmpty())
            {
                var c = CreateDT_end + " " + "23:59:59";
                strw = strw + " and t.CreateDT <='" + c + "'";
            }
            if (!TrackStautsId.IsNullOrEmpty())
            {
                strw = strw + " and t.TrackStautsId=" + TrackStautsId;
            }
            if (!AssignerUID.IsNullOrEmpty())
            {
                string[] aUID = AssignerUID.Split(',');
                string newAUID = "";
                if (aUID.Length > 0)
                {
                    for (int i = 0; i < aUID.Length; i++)
                    {
                        if (newAUID == "")
                        {
                            newAUID = "'" + aUID[i] + "'";
                        }
                        else
                        {
                            newAUID = newAUID + ",'" + aUID[i] + "'";
                        }
                    }
                    strw = strw + " and u.UserId in (" + newAUID + ")";
                }

            }
            else
            {
                //查看我的客户汇总 
                if (CurrentUser.HasPermiss(81) && !CurrentUser.HasPermiss(243) && !CurrentUser.HasPermiss(244))
                {
                    strw = strw + " and u.UserId in ('" + CurrentUser.UID + "')";
                }
                //查看本部门的客户汇总
                if (CurrentUser.HasPermiss(243) && !CurrentUser.HasPermiss(244))
                {
                    var UserId = tradersService.getUserIdByDeptId(CurrentUser.DeptId);
                    string newUserId = "";
                    foreach (var v in UserId)
                    {
                        if (newUserId == "")
                        {
                            newUserId = "'" + v + "'";
                        }
                        else
                        {
                            newUserId = newUserId + ",'" + v + "'";
                        }
                    }
                    strw = strw + " and u.UserId in (" + newUserId + ")";
                }
            }

            if (!BusinessModeId.IsNullOrEmpty())
            {
                strw = strw + " and t.BusinessModeId=" + BusinessModeId;
            }
            if (!BusinessScopeId.IsNullOrEmpty())
            {
                strw = strw + " and t.BusinessScopeId like '%" + BusinessScopeId + "%'";
            }
            DataSet ds = _dal.GetList(strw, pageIndex, pageSize);
            DataTable dt = ds.Tables[0];
            DataTable dt2 = ds.Tables[1];
            DataTable dt3 = ds.Tables[2];
            //客户类型
            DataTable dt4 = ds.Tables[3];
            //跟进状态
            DataTable dt5 = ds.Tables[4];
            //支付方式
            DataTable dt6 = ds.Tables[5];
            int count = Convert.ToInt32(dt3.Rows[0]["c"]);
            recordCount = count;
            footer = dt2;
            l1=Tool.ConvertToList<ViewSysData>(dt4);
            l2 = Tool.ConvertToList<ViewSysData>(dt5);
            l3 = Tool.ConvertToList<ViewSysData>(dt6);
            return dt;
        }


    }
}
