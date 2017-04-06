﻿/*----------------------------------------------------------------
 * 功能描述：缓存引擎
 * 创 建 人：蔡少发
 * 创建时间：2015-05-11
//----------------------------------------------------------------*/

using System;
using System.Web;
using System.Web.Caching;
using System.Collections;

namespace Pharos.Utility
{
    /// <summary>
    /// 缓存引擎
    /// </summary>
    public class DataCache
    {
        #region HttpRuntime

        /// <summary>
        /// 获取指定的Cache值 [HttpRuntime]
        /// </summary>
        /// <param name="key">键</param>
        public static object Get(string key)
        {
            Cache objCache = HttpRuntime.Cache;
            return objCache[key];
        }

        /// <summary>
        /// 获取指定的Cache值 [HttpRuntime]
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="key">键</param>
        public static T Get<T>(string key)
        {
            return (T)Get(key);
        }

        /// <summary>
        /// 写入指定的Cache值 [HttpRuntime]
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">缓存值</param>
        /// <param name="minutes">缓存时间（1-255分钟，默认10分钟）</param>
        /// <param name="relative">相对过期</param>
        public static void Set(string key, object value, short minutes,bool relative=false)
        {
            if (value != null)
            {
                Cache objCache = HttpRuntime.Cache;
                if (relative)
                {
                    objCache.Insert(key, value, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, minutes, 0));
                }
                else
                {
                    objCache.Insert(key, value, null, DateTime.Now.AddMinutes(minutes), TimeSpan.Zero, CacheItemPriority.NotRemovable, null);
                }
            }
            else
                Remove(key);
        }

        /// <summary>
        /// 写入指定的Cache值 [HttpRuntime]
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">缓存值</param>
        public static void Set(string key, object value)
        {
            Set(key, value, 10);
        }

        /// <summary>
        /// 移除所有缓存
        /// </summary>
        /// <returns>返回清理缓存数</returns>
        public static int RemoveAll()
        {
            Cache objCache = HttpRuntime.Cache;
            int count = objCache.Count;

            IDictionaryEnumerator ide = objCache.GetEnumerator();

            ArrayList list = new ArrayList();

            while (ide.MoveNext())
            {
                list.Add(ide.Key);
            }

            foreach (string arr in list)
            {
                objCache.Remove(arr);
            }

            return count;
        }

        /// <summary>
        /// 移除指定的Cache [HttpRuntime]
        /// </summary>
        /// <param name="key">键</param>
        public static void Remove(string key)
        {
            Cache objCache = HttpRuntime.Cache;
            objCache.Remove(key);
        }

        /// <summary>
        /// 移除指定的Cache [HttpRuntime]
        /// </summary>
        /// <param name="key">键</param>
        public static void Remove(params string[] key)
        {
            Cache objCache = HttpRuntime.Cache;

            foreach (string s in key)
            {
                objCache.Remove(s);
            }
        }

        /// <summary>
        /// 移除为指定Key开头的Cache [HttpRuntime]
        /// </summary>
        /// <param name="key">键</param>
        public static void RemoveStartsWith(string key)
        {
            Cache objCache = HttpRuntime.Cache;
            IDictionaryEnumerator ide = objCache.GetEnumerator();

            ArrayList list = new ArrayList();

            while (ide.MoveNext())
            {
                if (ide.Key.ToString().StartsWith(key))
                {
                    list.Add(ide.Key);
                }
            }

            foreach (string arr in list)
            {
                objCache.Remove(arr);
            }
        }

        /// <summary>
        /// 移除为指定Key开头的Cache [HttpRuntime]
        /// </summary>
        /// <param name="key">键</param>
        public static void RemoveStartsWith(params string[] key)
        {
            Cache objCache = HttpRuntime.Cache;
            IDictionaryEnumerator ide = objCache.GetEnumerator();

            ArrayList list = new ArrayList();

            while (ide.MoveNext())
            {
                foreach (string s in key)
                {
                    if (ide.Key.ToString().StartsWith(s))
                    {
                        list.Add(ide.Key);
                    }
                }
            }

            foreach (string arr in list)
            {
                objCache.Remove(arr);
            }
        }

        #endregion

        #region Application

        /// <summary>
        /// 写入指定的Cache值 [Application]
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void SetApp(string key, object value)
        {
            HttpContext.Current.Application.Lock();

            if (HttpContext.Current.Application[key] == null)
            {
                HttpContext.Current.Application.Add(key, value);
            }
            else
            {
                HttpContext.Current.Application.Set(key, value);
            }

            HttpContext.Current.Application.UnLock();
        }

        /// <summary>
        /// 获取指定的Cache值 [Application]
        /// </summary>
        /// <param name="key">键</param>
        public static object GetApp(string key)
        {
            if (HttpContext.Current.Application[key] != null)
            {
                return HttpContext.Current.Application[key];
            }

            return null;
        }

        /// <summary>
        /// 获取指定的Cache值 [Application]
        /// </summary>
        /// <param name="key">键</param>
        public static T GetApp<T>(string key)
        {
            return (T)GetApp(key);
        }

        /// <summary>
        /// 移除指定的Cache [Application]
        /// </summary>
        /// <param name="key">键</param>
        public static void RemoveApp(string key)
        {
            HttpContext.Current.Application.Remove(key);
        }

        #endregion
    }
}