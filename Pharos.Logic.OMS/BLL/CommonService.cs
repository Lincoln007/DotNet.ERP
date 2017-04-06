﻿using System.Collections.Generic;
using System.Linq;
using Pharos.Utility.Helpers;
using Pharos.Utility;
using System;
namespace Pharos.Logic.OMS
{
    /// <summary>
    /// 公共类
    /// </summary>
    public class CommonService
    {
        /// <summary>
        /// 生成新的GUID
        /// </summary>
        public static string GUID
        {
            get { return Guid.NewGuid().ToString().Replace("-", ""); }
        }

        /// <summary>
        /// 分解类别条件
        /// </summary>
        /// <param name="category">原传递值</param>
        /// <param name="big">大类</param>
        /// <param name="mid">中类</param>
        /// <param name="sub">子类</param>
        public static void GetCategory(string category, ref int? big, ref int? mid, ref int? sub)
        {
            if (category.IsNullOrEmpty()) return;
            var cates = category.TrimEnd(',').Split(',');
            if (cates.Length == 3)
            {
                big = int.Parse(cates[0]);
                mid = int.Parse(cates[1]);
                sub = int.Parse(cates[2]);
            }
            else if (cates.Length == 2)
            {
                big = int.Parse(cates[0]);
                mid = int.Parse(cates[1]);
            }
            else if (cates.Length == 1
                && !cates[0].IsNullOrEmpty())
            {
                big = int.Parse(cates[0]);
            }
        }
        /// <summary>
        /// 导入提示信息
        /// </summary>
        /// <param name="errls">错误列表</param>
        /// <param name="count">导入总记录数</param>
        /// <returns></returns>
        public static OpResult GenerateImportHtml(List<string> errls,int count)
        {
            var op = new OpResult();
            if (errls!=null && errls.Any())
            {
                op.Descript = "<dl><dt>以下数据导入失败：</dt>{0}</dl>";
                string str = "";
                foreach (var err in errls)
                {
                    str += "<dd>" + err + "</dd>";
                }
                op.Descript = string.Format(op.Descript, str);
                var scount = count - errls.Count;
                var ecount = errls.Count;
                if (errls.Any(o=>o.Contains("异常")))
                {
                    scount = 0;
                    ecount = count;
                    errls.RemoveAll(o => o.Contains("异常"));
                }
                var html = "<ul><li>成功导入{0}条数据,余{1}条导入失败!</li><li><a href=\"javascript:void(0)\" onclick=\"viewErr()\">查看失败记录!</a></li></ul>";
                op.Message = string.Format(html, scount,"<font color='red'>" + ecount + "</font>");
            }
            else
                op.Message = "<ul><li>成功导入" + count + "条数据!</li></ul>";
            op.Message = System.Web.HttpUtility.UrlEncode(op.Message);
            op.Descript = System.Web.HttpUtility.UrlEncode(op.Descript);
            op.Successed = true;
            return op;
        }
    }
}