﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Pharos.Utility.Helpers;
namespace Pharos.Sys.DAL
{
    internal class OMSCompanyAuthrizeDAL:BaseSysEntityDAL<Entity.OMS_CompanyAuthorize>
    {
        public List<Entity.OMS_CompanyAuthorize> FindPageList(NameValueCollection parms, out int count)
        {
            string sql = "select * from "+TableName+" where 1=1 ";
            var text = parms["searchText"];
            var state = parms["state"];
            if (!text.IsNullOrEmpty())
                sql +=string.Format(" and Title like '%{0}%' or FullTitle like '%{0}%' or Code like '%{0}%'",text);
            if (!state.IsNullOrEmpty())
                sql += " and state="+state;
            return base.ExceuteSqlForPage(sql, out count);
        }
        public bool SetState(string ids,short state)
        {
            string sql = "update " + TableName + " set state=" + state + " {0} where id in("+ids+")";
            string able = "";
            if (state == 2)
                able = ",Useable='N'";
            sql = string.Format(sql, able);
            return base.DbHelper.ExecuteNonQueryText(sql,null)>0;
        }
    }
}