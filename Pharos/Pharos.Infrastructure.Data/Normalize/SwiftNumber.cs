﻿using Pharos.Infrastructure.Data.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharos.Infrastructure.Data.Normalize
{
    public class SwiftNumber
    {
        public static object LockObject = new object();
        const string FILENAMEFORMAT = "{0}_{1}.SwiftNumber";
        /// <summary>
        /// 全局流水变更事件
        /// </summary>
        public static EventHandler<SwiftNumberChangeEventArgs> SwiftNumberChanged;

        public SwiftNumber(string name)
            : this(name, SwiftNumberMode.Day)
        {

        }
        public SwiftNumber(string name, SwiftNumberMode mode)
        {
            TempFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "PharosPosCache");
            SwiftNumberMode = mode;
            Name = name;
            switch (mode)
            {
                case Normalize.SwiftNumberMode.Day:
                    DateFormat = "yyyyMMdd";
                    break;
                case Normalize.SwiftNumberMode.Month:
                    DateFormat = "yyyyMM";
                    break;
                case Normalize.SwiftNumberMode.Year:
                    DateFormat = "yyyy";
                    break;
            }
        }
        public string Name { get; set; }
        private string TempFilePath { get; set; }
        public SwiftNumberMode SwiftNumberMode { get; set; }
        private string DateFormat { get; set; }
        public void Next()
        {
            var num = GetNumber() + 1;
            SaveToFile(num);
        }

        public void Reset(int num)
        {
            SaveToFile(num);
        }
        public int GetNumber()
        {
            string fileName = string.Empty;
            if (SwiftNumberMode != SwiftNumberMode.Normal)
            {
                fileName = string.Format(FILENAMEFORMAT, Name, DateTime.Now.ToString(DateFormat));
            }
            else
            {
                fileName = string.Format(FILENAMEFORMAT, Name, string.Empty);
            }
            fileName = Path.Combine(TempFilePath, fileName);

            if (!File.Exists(fileName))
            {
                SaveToFile(1);
                return 1;
            }
            return FileReadWrite.Read(fileName, FileMode.Open, (fs) =>
            {
                byte[] buffer = new byte[4];
                fs.Read(buffer, 0, 4);
                return BitConverter.ToInt32(buffer, 0);
            });
        }
        public int[] GetNumberRanges(int count, out int resetNum)
        {
            string fileName = string.Empty;
            if (SwiftNumberMode != SwiftNumberMode.Normal)
            {
                fileName = string.Format(FILENAMEFORMAT, Name, DateTime.Now.ToString(DateFormat));
            }
            else
            {
                fileName = string.Format(FILENAMEFORMAT, Name, string.Empty);
            }
            fileName = Path.Combine(TempFilePath, fileName);
            var start = 0;
            if (!File.Exists(fileName))
            {
                start = 1;
                SaveToFile(1);
            }
            else
            {
                start = FileReadWrite.Read(fileName, FileMode.Open, (fs) =>
                {
                    byte[] buffer = new byte[4];
                    fs.Read(buffer, 0, 4);
                    return BitConverter.ToInt32(buffer, 0);
                });
            }
            List<int> numbers = new List<int>();
            for (var i = 0; i < count; i++)
            {
                numbers.Add(start + i);
            }
            resetNum = numbers.LastOrDefault() + 1;
            return numbers.ToArray();
        }

        private void SaveToFile(int serialNumber)
        {
            try
            {
                string fileName = string.Empty;
                //当天文件名，记录当前流水
                if (SwiftNumberMode != SwiftNumberMode.Normal)
                {
                    fileName = string.Format(FILENAMEFORMAT, Name, DateTime.Now.ToString(DateFormat));
                }
                else
                {
                    fileName = string.Format(FILENAMEFORMAT, Name, string.Empty);
                }
                fileName = Path.Combine(TempFilePath, fileName);
                FileReadWrite.Write(fileName, FileMode.Create, (fs) =>
                {
                    var buffer = BitConverter.GetBytes(serialNumber);
                    fs.Write(buffer, 0, buffer.Length);
                });
                var tempEvent = SwiftNumberChanged;
                if (tempEvent != null)
                    tempEvent(this, new SwiftNumberChangeEventArgs() { Name = Name });

                if (serialNumber == 1 && SwiftNumberMode != SwiftNumberMode.Normal)
                {
                    //清除昨天的流水文件
                    Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            var delfileName = string.Format(FILENAMEFORMAT, Name, DateTime.Now.AddDays(-1).ToString(DateFormat));
                            delfileName = Path.Combine(TempFilePath, delfileName);
                            if (File.Exists(delfileName))
                            {
                                File.Delete(delfileName);
                            }
                        }
                        catch { }
                    });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
    public class SwiftNumberDto
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public SwiftNumberMode SwiftNumberMode { get; set; }

    }
    public enum SwiftNumberMode
    {
        /// <summary>
        /// 按年，流水号按顺序递增
        /// </summary>
        Year,
        /// <summary>
        /// 按月份，流水号按顺序递增
        /// </summary>
        Month,
        /// <summary>
        /// 按日期，流水号按顺序递增
        /// </summary>
        Day,
        /// <summary>
        /// 不按时间，流水号按顺序递增
        /// </summary>
        Normal
    }

    public class SwiftNumberChangeEventArgs : EventArgs
    {
        public string Name { get; set; }
    }
}