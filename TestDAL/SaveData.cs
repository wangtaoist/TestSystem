using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TestModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestDAL
{
    public class SaveData
    {
        private string initPath;
        public SaveData(string path)
        {
            this.initPath = Path.Combine(path, "TestData");
        }

        public void SaveTestLog(List<TestData> datas, LogColume colume)
        {
            if (!Directory.Exists(initPath))
            {
                Directory.CreateDirectory(initPath);
            }

            string fileName = Path.Combine(initPath, DateTime.Now.ToString("yyyyMMdd") + ".csv");
            if (!File.Exists(fileName))
            {
                //File.Create(fileName);
                using (FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                    {
                        string col = string.Format("SN,TestTime,MAC,TotalStatus,{0}"
                            , string.Join(",", datas.Select(s => s.TestItemName).ToList()));
                        sw.WriteLine(col);

                        string lowLimit = string.Format(",,,,{0}", string.Join(","
                            , datas.Select(s => s.LowLimit).ToList()));
                        sw.WriteLine(lowLimit);

                        string HiLimit = string.Format(",,,,{0}", string.Join(","
                           , datas.Select(s => s.UppLimit).ToList()));
                        sw.WriteLine(HiLimit);

                        string row = string.Format("{0},{1},{2},{3},{4}"
                            , colume.SN, colume.TestTime, colume.MAC, colume.TotalStatus
                            , string.Join(",", datas.Select(s => s.Value).ToList()));
                        sw.WriteLine(row);
                    }
                }
            }
            else
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                    {
                        string row = string.Format("{0},{1},{2},{3},{4}"
                            , colume.SN, colume.TestTime, colume.MAC, colume.TotalStatus
                            , string.Join(",", datas.Select(s => s.Value).ToList()));
                        sw.WriteLine(row);
                    }
                }
            }

            SaveMESData(datas, colume);

        }

        public void SaveMESData(List<TestData> datas, LogColume colume)
        {
            string mesPath = Path.Combine(initPath, "MES");
            if (!Directory.Exists(mesPath))
            {
                Directory.CreateDirectory(mesPath);
            }

            JObject obj = new JObject();
            //string col = 
            obj.Add("SN",colume.SN);
            obj.Add("TestTime", colume.TestTime);
            obj.Add("MAC", colume.MAC);
            obj.Add("TotalStatus", colume.TotalStatus);
            foreach (var item in datas)
            {
                //
                LogContent log = new LogContent();
                log.data = item.Value;
                log.isPass = item.Result;
                log.lowerLimit = item.LowLimit;
                log.upperLimit = item.UppLimit;
                log.unit = item.Unit;
                string json = JsonConvert.SerializeObject(log,Formatting.Indented);
                obj.Add(item.TestItemName, json);
            }
            string cont = JsonConvert.SerializeObject(obj,Formatting.Indented);
            string mesFileName = Path.Combine(mesPath, "result.txt");
            using (FileStream fs = new FileStream(mesFileName, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                {
                    sw.WriteLine(cont);
                }
            }
        }
    }
}
