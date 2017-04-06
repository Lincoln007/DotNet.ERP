﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Utility.Helpers;
using Pharos.Logic.Entity;
using Pharos.Utility;
using System.Net;
using System.IO;
using Pharos.Logic.DAL;
using Pharos.Sys;
using System.Web.Mvc;
namespace Pharos.Logic.BLL
{
    /// <summary>
    /// 公共类
    /// </summary>
    public class CommonService
    {
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
        /// 适用门店名称
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public static string GetStoreTitleList(string storeId) 
        {
            string storeTitleList = "",storeTitle = "";
            string[] arr = storeId.Split(',');
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[0] == "")
                {
                    storeTitleList = "全部";
                    break;
                }
                else
                {
                    if (!string.IsNullOrEmpty(arr[i]))
                    {
                        var sId = arr[i];
                        storeTitle = WarehouseService.Find(o =>o.CompanyId==CommonService.CompanyId && o.StoreId == sId).Title;
                    }
                    storeTitleList += i > 0 ? ("、" + storeTitle) : storeTitle;
                }
            }
            return storeTitleList;     
        }
        public static int CompanyId
        {
            get
            {
                return Pharos.Sys.SysCommonRules.CompanyId;
            }
        }
        /// <summary>
        /// 导入提示信息
        /// </summary>
        /// <param name="errls">错误列表</param>
        /// <param name="count">导入总记录数</param>
        /// <param name="data">返回客户端数据</param>
        /// <param name="isSuccess">是否成功提示</param>
        /// <returns></returns>
        public static OpResult GenerateImportHtml(List<string> errls,int count,object data=null,bool isSuccess=true)
        {
            var op = new OpResult();
            if (errls != null && errls.Any())
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
                if (errls.Any(o => o.Contains("异常")))
                {
                    scount = 0;
                    ecount = count;
                    errls.RemoveAll(o => o.Contains("异常"));
                }
                var html = "<ul><li>{2}导入{0}条数据,余{1}条导入失败!</li><li><a href=\"javascript:void(0)\" onclick=\"viewErr()\">查看失败记录!</a></li></ul>";
                op.Message = string.Format(html, scount, "<font color='red'>" + ecount + "</font>",isSuccess?"成功":"可");
                op.Successed = false;
            }
            else
            {
                op.Message = "<ul><li>成功导入" + count + "条数据!</li></ul>";
                op.Successed = true;
            }
            op.Message = System.Web.HttpUtility.UrlEncode(op.Message);
            op.Descript = System.Web.HttpUtility.UrlEncode(op.Descript);
            op.Data = data;
            return op;
        }
        /// <summary>
        /// 调用APP刷新接口
        /// </summary>
        /// <param name="parms">json串</param>
        /// <returns></returns>
        public static OpResult AppRefresh(string parms)
        {
            var op = new OpResult();
            try
            {
                var url = System.Configuration.ConfigurationManager.AppSettings["AppUrl"] + "MemoryRefresh";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "text/json";
                byte[] data = Encoding.Default.GetBytes(parms);
                request.ContentLength = data.Length;
                var reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();

                var response = (HttpWebResponse)request.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
                var rt = sr.ReadToEnd().Trim();
                sr.Close();
                op = rt.ToObject<OpResult>();
                op.Successed = op.Code == "200";
            }
            catch (Exception ex)
            {
                op.Message = ex.Message;
            }
            return op;
        }
        public static OpResult ExecuteTranList(params BatchTranEntity[] entities)
        {
            var dal = new CommonDAL();
            try
            {
                dal.ExecuteTranList(entities);
                return OpResult.Success();
            }catch(Exception ex)
            {
                new LogEngine().WriteError(ex);
                return OpResult.Fail(ex.Message);
            }
        }

        public static IList<SelectListItem> EnumToSelect(Type type, string emptyTitle = null, byte? selectValue = null)
        {
            IList<SelectListItem> list = new List<SelectListItem>();
            if (emptyTitle != null) list.Insert(0, new SelectListItem() { Text = emptyTitle, Value = "", Selected = true });
            string[] t = Enum.GetNames(type);
            foreach (string f in t)
            {
                var text = f;
                var obj = Enum.Parse(type, f);
                var field = type.GetField(f);
                object[] objs = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if (objs != null && objs.Length > 0)
                {
                    System.ComponentModel.DescriptionAttribute da = (System.ComponentModel.DescriptionAttribute)objs[0];
                    text = da.Description;
                }
                var d = Convert.ToInt32(Convert.ChangeType(obj, type));
                var item = new SelectListItem() { Text = text, Value = d.ToString() };
                if (selectValue.HasValue)
                    item.Selected = d == selectValue.Value;
                list.Add(item);
            }
            return list;
        }
    }
}