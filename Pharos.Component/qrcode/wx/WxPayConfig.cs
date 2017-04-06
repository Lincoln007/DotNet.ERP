﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
namespace Pharos.Component.qrcode.wx
{
    /**
    * 	配置账号信息
    */
    public class WxPayConfig
    {
        //=======【基本信息设置】=====================================
        /* 微信公众号信息配置
        * APPID：绑定支付的APPID（必须配置）
        * MCHID：商户号（必须配置）
        * KEY：商户支付密钥，参考开户邮件设置（必须配置）
        * APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置）
        */
        public static string APPID
        {
            get { return ConfigurationManager.AppSettings["wxappId"]; }
        }
        public static string MCHID
        {
            get { return ConfigurationManager.AppSettings["wxmchId"]; }
        }
        public static string KEY
        {
            get { return ConfigurationManager.AppSettings["wxkey"]; }
        }
        public const string APPSECRET = "51c56b886b5be869567dd389b3e5d1d6";

        //=======【证书路径设置】===================================== 
        /* 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
        */
        public const string SSLCERT_PATH = "Content/wxcerts/apiclient_cert.p12";
        public const string SSLCERT_PASSWORD = "1233410002";



        //=======【支付结果通知url】===================================== 
        /* 支付结果通知回调url，用于商户接收支付结果
        */
        //public static string NOTIFY_URL = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + (HttpContext.Current.Request.ApplicationPath == "/" ? "" : HttpContext.Current.Request.ApplicationPath) + "/Qrcode/ResultNotify";
        public static string NOTIFY_URL
        {
            get { return ConfigurationManager.AppSettings["wxnotify_url"]; }
        }
        //=======【商户系统后台机器IP】===================================== 
        /* 此参数可手动配置也可在程序中自动获取
        */
        public static string IP
        {
            get
            {
                //if (HttpContext.Current != null)
                    //return HttpContext.Current.Request.UserHostAddress;
                return "8.8.8.8";
            }
        }


        //=======【代理服务器设置】===================================
        /* 默认IP和端口号分别为0.0.0.0和0，此时不开启代理（如有需要才设置）
        */
        public const string PROXY_URL = "http://10.152.18.220:8080";

        //=======【上报信息配置】===================================
        /* 测速上报等级，0.关闭上报; 1.仅错误时上报; 2.全量上报
        */
        public const int REPORT_LEVENL = 1;
    }
}