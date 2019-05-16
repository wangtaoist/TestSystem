using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestModel;
using TestTool;
using System.Data;
using TestDAL;

namespace TestBLL
{
    public class TestLogic
    {
        private Queue<string> testQueue;
        private string initPath;
        private DataBase dataBase;
        private OperateInstrument operate;
        private ConfigData config;
        private CsrOperate csr;
        private OperateBES operateBES;
        public string BTAddress;
        public string PorductSN;
        public string BattarySN;
   
        public TestLogic(Queue<string> queue,string path)
        {
            this.testQueue = queue;
            this.initPath = path;
            dataBase = new DataBase(path);
            config = GetConfigData();
            //csr = new CsrOperate();
            operateBES = new OperateBES(config.SerialPort, testQueue);
            operate = new OperateInstrument(config, testQueue);
        }

        public void InitTestPort(object obj)
        {
            try
            {             
                if (config.GPIB_Enable)
                {
                    testQueue.Enqueue("打开和初始化MT8852");                 
                    operate.InitInstr();
                    testQueue.Enqueue("打开和初始化仪器完成");
                }
                if (config.Power_Enable)
                {
                    testQueue.Enqueue("打开和初始化仪电源供应器");
                    operate.InitPower();
                    testQueue.Enqueue("打开和初始化仪器完成");
                }
                if(config.Multimeter_Select)
                {
                    testQueue.Enqueue("打开和初始化仪万用表");
                    operate.initMultimeter();
                    testQueue.Enqueue("打开和初始化万用表完成");
                }
            }
            catch (Exception ex)
            {
                testQueue.Enqueue(ex.Message);
            }
           
        }

        public TestData TestProcess(TestData data)
        {
            switch (data.TestItem)
            {
                case "CSR_Open_Connection":
                    {
                        data = csr.OpenPort(data);
                        break;
                    }
                case "CSR_Check_PsKey":
                    {
                        break;
                    }
                case "MT8852_Read_BD_Address":
                    {
                       data = operate.GetBTAddress(data);
                        break;
                    }
                case "CSR_Cal_Freq":
                    {
                        data = operate.CalFreq(data);
                        break;
                    }
                case "CSR_Offset_Gain":
                    {
                        data = csr.ReadOffset(data);
                        break;
                    }
                case "CSR_Enter_TestMode":
                    {
                        break;
                    }
                case "1455_Open_Connection":
                    {
                        break;
                    }
                case "CalFreq_Run_8852_Script":
                    {
                        break;
                    }
                case "Open_MT8852":
                    {
                        data = operate.Open_MT8852(data);
                        break;
                    }
                case "Closed_MT8852":
                    {
                        data = operate.Closed_MT8852(data);
                        break;
                    }
                case "Run_MT8852_Script":
                    {
                        data = operate.Run_Script(data);
                        break;
                    }
                case "DUT_Power":
                    {
                       data = operate.GetTXPower(data);
                        break;
                    }
                case "DUT_Sensitivity":
                    {
                        data = operate.GetSingleSensitivity(data);
                        break;
                    }
                case "DUT_InitCarrier":
                    {
                        data = operate.GetInitialcarrier(data);
                        break;
                    }
                case "DUT_ModulationIndex":
                    {
                        data = operate.GetModulationindex(data);
                        break;
                    }
                case "DUT_CarrierDrift":
                    {
                        data = operate.GetCarrierdrift(data);
                        break;
                    }
                case "CSR_Write_PsKey":
                    {
                        break;
                    }
                case "RunExternalApp":
                    {
                        break;
                    }
                case "K2300Series_Open":
                    {
                        data = operate.K2300Series_Open(data);
                        break;
                    }
                case "K2300Series_ChannelOne_OutPut":
                    {
                        data = operate.K2300Series_ChannelOne_OutPut(data);
                        break;
                    }
                case "K2300Series_ChannelTwo_OutPut":
                    {
                        data = operate.K2300Series_ChannelTwo_OutPut(data);
                        break;
                    }
                case "K2300Series_ChannelOne_ReadVoltage":
                    {
                        data = operate.K2300Series_ChannelOne_ReadVoltage(data);
                        break;
                    }
                case "K2300Series_ChannelOne_ReadCurrent":
                    {
                        data = operate.K2300Series_ChannelOne_ReadCurrent(data);
                        break;
                    }
                case "K2300Series_ChannelTwo_ReadVoltage":
                    {
                        data = operate.K2300Series_ChannelTwo_ReadVoltage(data);
                        break;
                    }
                case "K2300Series_ChannelTwo_ReadCurrent":
                    {
                        data = operate.K2300Series_ChannelTwo_ReadCurrent(data);
                        break;
                    }
                case "K2300Series_ChannelOne_StopOut":
                    {
                        data = operate.K2300Series_ChannelOne_StopOut(data);
                        break;
                    }
                case "K2300Series_ChannelTwo_StopOut":
                    {
                        data = operate.K2300Series_ChannelTwo_StopOut(data);
                        break;
                    }
                case "K2300Series_Closed":
                    {
                        data = operate.K2300Series_Closed(data);
                        break;
                    }
                case "HP66319D_Open":
                    {
                        data = operate.K2300Series_Open(data);
                        break;
                    }
                case "HP66319D_ChannelOne_OutPut":
                    {
                        data = operate.K2300Series_ChannelOne_OutPut(data);
                        break;
                    }
                case "HP66319D_ChannelTwo_OutPut":
                    {
                        data = operate.K2300Series_ChannelTwo_OutPut(data);
                        break;
                    }
                case "HP66319D_ChannelOne_ReadVoltage":
                    {
                        data = operate.K2300Series_ChannelOne_ReadVoltage(data);
                        break;
                    }
                case "HP66319D_ChannelOne_ReadCurrent":
                    {
                        data = operate.K2300Series_ChannelOne_ReadCurrent(data);
                        break;
                    }
                case "HP66319D_ChannelTwo_ReadVoltage":
                    {
                        data = operate.K2300Series_ChannelTwo_ReadVoltage(data);
                        break;
                    }
                case "HP66319D_ChannelTwo_ReadCurrent":
                    {
                        data = operate.K2300Series_ChannelTwo_ReadCurrent(data);
                        break;
                    }
                case "HP66319D_ChannelOne_StopOut":
                    {
                        data = operate.K2300Series_ChannelOne_StopOut(data);
                        break;
                    }
                case "HP66319D_ChannelTwo_StopOut":
                    {
                        data = operate.K2300Series_ChannelTwo_StopOut(data);
                        break;
                    }
                case "HP66319D_Closed":
                    {
                        data = operate.K2300Series_Closed(data);
                        break;
                    }
                case "Key34461_Open":
                    {
                        data = operate.Key34461_Open(data);
                        break;
                    }
                case "Key34461_ReadVoltage":
                    {
                        data = operate.Key34461_ReadVoltage(data);
                        break;
                    }
                case "Key34461_ReadCurrent":
                    {
                        data = operate.Key34461_ReadCurrent(data);
                        break;
                    }
                case "Key34461_Closed":
                    {
                        data = operate.Key34461_Closed(data);
                        break;
                    }               
                case "BES_Enter_TestMode":
                    {                    
                        break;
                    }
                case "BES_Enter_DUTMode":
                    {
                        data = operateBES.BES_Enter_DUTMode(data);
                        break;
                    }
                case "BES_SoftVersion":
                    {
                        data = operateBES.ReadSoftVersion(data);
                        break;
                    }
                case "BES_HWVersion":
                    {
                        data = operateBES.ReadHWVersion(data);
                        break;
                    }
                case "BES_WhiteLight":
                    {
                        data = operateBES.BES_WhiteLight(data);
                        break;
                    }
                case "BES_RedLight":
                    {
                        data = operateBES.BES_RedLight(data);
                        break;
                    }
                case "BES_ReadSN":
                    {
                        data = operateBES.BES_ReadSN(data);
                        break;
                    }
                case "BES_CompareSN":
                    {
                        data = operateBES.BES_CompareSN(data);
                        break;
                    }
                case "BES_ReadBTName":
                    {
                        //data = operateBES.BES_ReadBTName(data);
                        break;
                    }
                case "BES_ReadBTAddress":
                    {
                        data = operateBES.BES_ReadBTAddress(data);
                        break;
                    }
                case "BES_ReadBattarySN":
                    {
                        data = operateBES.BES_ReadBattarySN(data);
                        break;
                    }
                case "BES_CompareBattarySN":
                    {
                        data = operateBES.BES_CompareBattarySN(data);
                        break;
                    }
                case "BES_ReadProductColor":
                    {
                        data = operateBES.BES_ReadProductColor(data);
                        break;
                    }
                case "BES_ReadWarningTone":
                    {
                        data = operateBES.BES_ReadWarningTone(data);
                        break;
                    }
                case "BES_HALLTest":
                    {
                        operateBES.BES_HALLTest(data);
                        break;
                    }
                case "BES_Pair":
                    {
                        data = operateBES.BES_Pair(data);
                        break;
                    }
                case "BES_Shutdown":
                    {
                        data = operateBES.BES_Shutdown(data);
                        break;
                    }
                case "BES_Reset":
                    {
                        data = operateBES.BES_Reset(data);
                        break;
                    }
                case "BES_ClearPair":
                    {
                        data = operateBES.BES_ClearPair(data);
                        break;
                    }
                case "BES_ReadWaterProof":
                    {
                        data = operateBES.BES_ReadWaterProof(data);
                        break;
                    }
                case "BES_ReadBattaryVoltage":
                    {
                        data = operateBES.BES_ReadBattaryVoltage(data);
                        break;
                    }
                case "BES_ReadNTC":
                    {
                        data = operateBES.BES_ReadNTC(data);
                        break;
                    }
                case "BES_ReadChargeMode":
                    {
                        data = operateBES.BES_ReadChargeMode(data);
                        break;
                    }
                case "BES_ReadTestStation":
                    {
                        data = operateBES.BES_ReadTestStation(data);
                        break;
                    }
                case "BES_WriteSN":
                    {
                        operateBES.btAddress = BTAddress;
                        data = operateBES.BES_WriteSN(data);
                        break;
                    }
                case "BES_WriteBattarySN":
                    {
                        operateBES.btAddress = BTAddress;
                        data = operateBES.BES_WriteBattarySN(data);
                        break;
                    }
                case "BES_WriteBTAddress":
                    {
                        data = operateBES.BES_WriteBTAddress(data);
                        break;
                    }
                case "BES_WriteProductColor":
                    {
                        data = operateBES.BES_WriteProductColor(data);
                        break;
                    }
                case "BES_WriteWarningTone":
                    {
                        data = operateBES.BES_WriteWarningTone(data);
                        break;
                    }
                case "BES_WriteChargeMode":
                    {
                        data = operateBES.BES_WriteChargeMode(data);
                        break;
                    }
                case "BES_WriteTestStation":
                    {
                        data = operateBES.BES_WriteTestStation(data);
                        break;
                    }
                case "BES_OpenSerialPort":
                    {
                        data = operateBES.OpenSerialPort(data);
                        break;
                    }
                case "BES_ClosedSerialPort":
                    {
                        data = operateBES.ClosedSerialPort(data);
                        break;
                    }
            }
            return data;
        }

        public List<TestData> GetTestItem()
        {
            List<TestData> list = dataBase.GetTestItem();          
            return list;
        }

        public TestRatio GetTestRadio()
        {
            TestRatio list = dataBase.GetTestRadio();
            return list;
        }

        public ConfigData GetConfigData()
        {
            return dataBase.GetConfigData();
        }

        public void ConfigInitInstrument()
        {
 
        }

        public void ClosedInstrument()
        {
            if (operate != null)
            {
                operate.CloesdInstr();
            }
        }

        public void UpdataTestCount(bool status)
        {
            dataBase.UpdataTestCount(status);
        }

        public void ClsRadio()
        {
            dataBase.ClsRadio();
        }

        public void SaveTestLog(List<TestData> datas,LogColume col)
        {
            SaveData saveData = new SaveData(initPath);
            saveData.SaveTestLog(datas, col);
        }

        public void RstPower()
        {
            operate.InitPower();
        }
    }
}
