﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Sale.Suspend
{
    /// <summary>
    /// 挂单列表
    /// </summary>
    public class SuspendList : List<SuspendDetails>
    {
        static readonly string FileFormat = "SuspendList{0}_{1}_{2}.JSON";
        public SuspendList()
        {
        }
        public SuspendList(string storeId, string machineSn)
        {
            StoreId = storeId;
            MachineSn = machineSn;
        }
        public string StoreId { get; set; }
        public string MachineSn { get; set; }
        public static SuspendList Factory(string storeId, string machineSn, int companyId, string path)
        {
            SuspendList result = null;
            try
            {
                var fileName = string.Format(FileFormat, storeId, machineSn, companyId);
                var filePath = Path.Combine(path, fileName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (File.Exists(filePath))
                {
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        JsonReader reader = new JsonTextReader(sr);
                        result = serializer.Deserialize<SuspendList>(reader);
                    }
                }
            }
            catch (Exception)
            {

            }
            if (result == null)
            {
                result = new SuspendList(storeId, machineSn);
            }
            return result;
        }

        public void Save(string path, string storeId, string machineSn, int companyId)
        {
            var fileName = string.Format(FileFormat, storeId, machineSn, companyId);
            var filePath = Path.Combine(path, fileName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                JsonSerializer serializer = new JsonSerializer();
                JsonWriter writer = new JsonTextWriter(sw);
                serializer.Serialize(writer, this);
            }
        }
    }
}