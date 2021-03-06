﻿using Pharos.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Pharos.Logic.DAL
{
    /// <summary>
    /// 销售订单 数据访问
    /// </summary>
    internal class SaleOrderDAL : BaseDAL
    {
        public DataTable QuerySaleOrdersPageList(System.Collections.Specialized.NameValueCollection nvl)
        {
            var startDate = nvl["date"];
            var endDate = nvl["date2"];
            var cashier = nvl["cashier"];
            var saler = nvl["saler"];
            var store = nvl["store"];
            var apiCodes = nvl["apiCodes"]??"";
            var searchText = nvl["searchText"]??"";
            var ispage = nvl["ispage"].IsNullOrEmpty() ? 1 : int.Parse(nvl["ispage"]);
            var type = nvl["Type"] ?? "";
            var sortField = nvl["sortField"] ?? "";
            var pageIndex = 1;
            var pageSize = 30;
            if (!nvl["page"].IsNullOrEmpty())
                pageIndex = int.Parse(nvl["page"]);
            if (!nvl["rows"].IsNullOrEmpty())
                pageSize = int.Parse(nvl["rows"]);
            var pmrs = new SqlParameter[] { 
                new SqlParameter("@companyId",Sys.SysCommonRules.CompanyId),
                new SqlParameter("@startDate",startDate),
                new SqlParameter("@endDate",endDate),
                new SqlParameter("@storeIds",store),
                new SqlParameter("@title",searchText),
                new SqlParameter("@cashiers",cashier),
                new SqlParameter("@salers",saler),
                new SqlParameter("@apicodes",apiCodes),
                new SqlParameter("@type",type),
                new SqlParameter("@sortField",sortField),
                new SqlParameter("@CurrentPage",pageIndex),
                new SqlParameter("@PageSize",pageSize),
                new SqlParameter("@ispage",ispage)
            };
            
            return _db.DataTable("Rpt_BeforeProductSaleDetails", pmrs,120);
        }
        public List<string> GetDataForSearch(string searchText, string searchField,int companyId)
        {
            var sql = @"SELECT DISTINCT top 20 [" + searchField + @"] FROM  dbo.SaleOrders a
                INNER JOIN dbo.SaleDetail b ON a.PaySN = b.PaySN
                LEFT JOIN dbo.Members d ON a.MemberId = d.MemberId 
                LEFT JOIN dbo.ConsumptionPayment e ON a.PaySN = e.PaySN
                WHERE a.CompanyId=" + companyId;
            sql +=" and "+searchField+" like '%"+searchText+"%'";
            using (var db = new EFDbContext())
            {
                return db.Database.SqlQuery<string>(sql).ToList();
            }
        }
        public Dictionary<string,int> GetMaxNumByDate(List<string> dates,int companyId)
        {
            var sql =string.Format(@"SELECT CAST(MAX(sort) AS INT) maxnum,t.prefix FROM(
				 SELECT SUBSTRING(CustomOrderSn,11,4) sort,SUBSTRING(CustomOrderSn,0,11) prefix FROM dbo.SaleOrders WHERE CompanyId={0} AND MachineSN='00' and CONVERT(VARCHAR(10),CreateDT,120) IN({1})
				) t GROUP BY prefix", companyId, string.Join(",", dates.Select(o => "'" + o + "'")));
            var dt = _db.DataTableText(sql, null);
            var dicts= dt.AsEnumerable().ToDictionary(o => o["prefix"].ToString(), o => Convert.ToInt32(o["maxnum"]));
            return dicts;
        }
    }
}
