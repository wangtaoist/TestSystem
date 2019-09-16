﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestModel;
using TestTool;
using System.Data;
using TestDAL;
using System.Threading;

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
        private AudioOperate Audio;
        private OperateLED operateLED;

        public TestLogic(Queue<string> queue, string path)
        {
            this.testQueue = queue;
            this.initPath = path;
            dataBase = new DataBase(path);
            config = GetConfigData();
            //csr = new CsrOperate();
            operateBES = new OperateBES(config.SerialPort
                , testQueue, config.SerialSelect);
            //if(config.AutoSNTest)
            //{
            operateBES.config = config;
            operateBES.dataBase = dataBase;
            //operateBES.BES_WriteSN(new TestData());
            //}
             
            operate = new OperateInstrument(config, testQueue);
            if(config.AudioEnable)
            {
                queue.Enqueue("加载Audio项目文件");
                Audio = new AudioOperate(config, operateBES);
                queue.Enqueue("加载Audio项目文件完成");
            }
            if(config.LEDEnable)
            {
                queue.Enqueue("打开LED测试仪");
                operateLED = new OperateLED(config);
            }
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
                if(config._4010Enable)
                {
                    testQueue.Enqueue("打开和初始化仪4010");
                    operate.init4010();
                    testQueue.Enqueue("打开和初始化4010完成");
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
                        data = operate.Open_CSR_Port(data);
                        break;
                    }
                case "CSR_Cal_Freq":
                    {
                        data = operate.CSR_CalFreq(data);
                        break;
                    }
                case "CSR_Offset_Gain":
                    {
                        data = operate.CSR_Read_Trim(data);
                        break;
                    }
                case "CSR_Enter_TestMode":
                    {
                        data = operate.CSR_EnableTestMode(data);
                        break;
                    }
                case "CSR_Cloesd_Port":
                    {
                        data = operate.CSR_Read_Trim(data);
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
                case "Run_MT8852_CalcFreqScript":
                    {
                        byte sampTrim = 0x44;
                        TestData calData = null;
                        operateBES.BES_WriteTrim(sampTrim);
                        for (int i = 0; i < 3; i++)
                        {
                            operate.Run_MT8852_CalcFreqScript(data);
                            calData = operate.GetInitialcarrier(data);
                            testQueue.Enqueue("频率偏移:" + calData.Value);
                            if (calData.Result == "Pass")
                            {
                                data.Result = "Pass";
                                data.Value = calData.Value;
                                break;
                            }
                            else
                            {
                                int freqOff = (int)double.Parse(calData.Value) * 2;
                                byte trim = (byte)(sampTrim - Convert.ToByte(Math.Abs(freqOff)));
                                testQueue.Enqueue("写入Trim:" + trim);
                                Thread.Sleep(1000);
                                operateBES.BES_WriteTrim(trim);
                            }
                            Thread.Sleep(200);
                            if (i == 2)
                            {
                                data.Result = "Fail";
                                data.Value = calData.Value;
                            }
                        }
                        //data = operate.Run_MT8852_CalcFreqScript(data);
                        break;
                    }
                case "MT8852_Read_BD_Address":
                    {
                        data = operate.GetBTAddress(data);
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

                case "Open_4010":
                    {
                        data = operate.Open_4010(data);
                        break;
                    }
                case "Run_4010Script":
                    {
                        data = operate.Run_4010Script(data);
                        break;
                    }
                case "Agilent4010_CalcFreq":
                    {
                        byte sampTrim = 0x44;
                        operateBES.BES_WriteTrim(sampTrim);
                        //operateBES.BES_Enter_DUTMode(new TestData());
                        for (int i = 0; i < 5; i++)
                        {
                            operate.Agilent4010_CalcFreq();
                            TestData calData = operate.Get4010Initialcarrier(data);
                            if(calData.Result == "Pass")
                            {
                                data.Result = "Pass";
                                data.Value = calData.Value;
                                break;
                            }
                            else
                            {
                                int freqOff = (int)double.Parse(calData.Value) * 2;
                               byte trim =(byte) (sampTrim - Convert.ToByte(Math.Abs(freqOff)));
                                Thread.Sleep(1000);
                                operateBES.BES_WriteTrim(trim);
                            }
                            Thread.Sleep(200);
                            if(i == 4 )
                            {
                                data.Result = "Fail";
                                data.Value = "Fail";
                            }
                        }
                       
                        break;
                    }
                case "Run_4010CalcFreqAfterScript":
                    {
                        data = operate.Run_4010CalcFreqAfterScript(data);
                        break;
                    }
                case "Get4010TXPower":
                    {
                        data = operate.Get4010TXPower(data);
                        break;
                    }
                case "Get4010SingleSensitivity":
                    {
                        data = operate.Get4010SingleSensitivity(data);
                        break;
                    }
                case "Get4010Modulationindex":
                    {
                        data = operate.Get4010Modulationindex(data);
                        break;
                    }
                case "Get4010Initialcarrier":
                    {
                        data = operate.Get4010Initialcarrier(data);
                        break;
                    }
                case "Get4010Carrierdrift":
                    {
                        data = operate.Get4010Carrierdrift(data);
                        break;
                    }
                case "Get4010BTAddress":
                    {
                        data = operate.Get4010BTAddress(data);
                        break;
                    }

                case "Closed_4010":
                    {
                        data = operate.Closed_4010(data);
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
                case "K2300Series_ChannelOne_OverVoltage":
                    {
                        data = operate.K2300Series_ChannelOne_OverVoltage(data);
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
                case "K2300Series_ChannelTwo_OverVoltage":
                    {
                        data = operate.K2300Series_ChannelTwo_OverVoltage(data);
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
                        data = operate.HP66319D_Open(data);
                        break;
                    }
                case "HP66319D_ChannelOne_OutPut":
                    {
                        data = operate.HP66319D_ChannelOne_OutPut(data);
                        break;
                    }
                case "HP66319D_ChannelTwo_OutPut":
                    {
                        data = operate.HP66319D_ChannelTwo_OutPut(data);
                        break;
                    }
                case "HP66319D_ChannelOne_OverVoltage":
                    {
                        data = operate.HP66319D_ChannelOne_OverVoltage(data);
                        break;
                    }
                case "HP66319D_ChannelTwo_OverVoltage":
                    {
                        data = operate.HP66319D_ChannelTwo_OverVoltage(data);
                        break;
                    }
                case "HP66319D_ChannelOne_ReadVoltage":
                    {
                        data = operate.HP66319D_ChannelOne_ReadVoltage(data);
                        break;
                    }
                case "HP66319D_ChannelOne_ReadCurrent":
                    {
                        data = operate.HP66319D_ChannelOne_ReadCurrent(data);
                        break;
                    }

                case "HP66319D_ChannelTwo_ReadVoltage":
                    {
                        data = operate.HP66319D_ChannelTwo_ReadVoltage(data);
                        break;
                    }
                case "HP66319D_ChannelTwo_ReadCurrent":
                    {
                        data = operate.HP66319D_ChannelTwo_ReadCurrent(data);
                        break;
                    }
                case "HP66319D_ChannelOne_StopOut":
                    {
                        data = operate.HP66319D_ChannelOne_StopOut(data);
                        break;
                    }
                case "HP66319D_ChannelTwo_StopOut":
                    {
                        data = operate.HP66319D_ChannelTwo_StopOut(data);
                        break;
                    }
                case "HP66319D_Closed":
                    {
                        data = operate.HP66319D_Closed(data);
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
                case "Key34461_ReadACVoltage":
                    {
                        data = operate.Key34461_ReadACVoltage(data);
                        break;
                    }
                case "Key34461_ReadCurrent":
                    {
                        data = operate.Key34461_ReadCurrent(data);
                        break;
                    }
                case "Key34461_ReadResistance":
                    {
                        data = operate.Key34461_ReadResistance(data);
                        break;
                    }
                case "Key34461_ReadContinuity":
                    {
                        data = operate.Key34461_ReadContinuity(data);
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
                case "BES_ReadTrim":
                    {
                        data = operateBES.BES_ReadTrim(data);
                        break;
                    }
                case "BES_HWVersion_ASCII":
                    {
                        data = operateBES.ReadHWVersion_ASCII(data);
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
                case "BES_PowerKeyTest":
                    {
                        operateBES.BES_PowerKeyTest(data);
                        break;
                    }
                case "BES_Pair":
                    {
                        data = operateBES.BES_Pair(data);
                        break;
                    }
                case "BES_SetVolume":
                    {
                        data = operateBES.BES_SetVolume(data);
                        break;
                    }
                case "BES_Shutdown":
                    {
                        data = operateBES.BES_Shutdown(data);
                        break;
                    }
                case "BES_ShippingMode":
                    {
                        data = operateBES.BES_ShippingMode(data);
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
                case "BES_WriteHWVersion":
                    {
                        data = operateBES.BES_WriteHWVersion(data);
                        break;
                    }
                case "BES_WriteHWVersion_ASCII":
                    {
                        data = operateBES.BES_WriteHWVersion_ASCII(data);
                        break;
                    }
                case "BES_ControlMic1":
                    {
                        data = operateBES.BES_ControlMic1(data);
                        break;
                    }
                case "BES_ControlMic2":
                    {
                        data = operateBES.BES_ControlMic2(data);
                        break;
                    }
                case "BES_ControlMic3":
                    {
                        data = operateBES.BES_ControlMic3(data);
                        break;
                    }
                case "BES_ControlMicAllOpen":
                    {
                        data = operateBES.BES_ControlMicAllOpen(data);
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
                case "SwitchToA2dp":
                    {
                        data = Audio.SwitchToA2dp(data);
                        break;
                    }
                case "OpenA2":
                    {
                        data = Audio.OpenA2(data);
                        break;
                    }
                case "SwitchToHfp":
                    {
                        data = Audio.SwitchToHfp(data);
                        break;
                    }
                case "EarPair":
                    {
                        data = Audio.EarPair(data);
                        break;
                    }
                case "SpeakerLevel_Left":
                    {
                        data = Audio.SpeakerLevel_Left(data);
                        break;
                    }
                case "SpeakerLevel_Right":
                    {
                        data = Audio.SpeakerLevel_Right(data);
                        break;
                    }
                case "SpeakerTHD_Left":
                    {
                        data = Audio.SpeakerTHD_Left(data);
                        break;
                    }
                case "SpeakerTHD_Right":
                    {
                        data = Audio.SpeakerTHD_Right(data);
                        break;
                    }
                case "SpeakerSNR_Left":
                    {
                        data = Audio.SpeakerSNR_Left(data);
                        break;
                    }
                case "SpeakerSNR_Right":
                    {
                        data = Audio.SpeakerSNR_Right(data);
                        break;
                    }
                case "SpeakerCrosstalk_Left":
                    {
                        data = Audio.SpeakerCrosstalk_Left(data);
                        break;
                    }
                case "SpeakerCrosstalk_Right":
                    {
                        data = Audio.SpeakerCrosstalk_Right(data);
                        break;
                    }
                case "SpeakerNoise_Left":
                    {
                        data = Audio.SpeakerNoise_Left(data);
                        break;
                    }
                case "SpeakerNoise_Right":
                    {
                        data = Audio.SpeakerNoise_Right(data);
                        break;
                    }
                case "MicphoneLevel":
                    {
                        data = Audio.MicphoneLevel(data);
                        break;
                    }
                case "MicphoneTHD":
                    {
                        data = Audio.MicphoneTHD(data);
                        break;
                    }
                case "MicphoneSNR":
                    {
                        data = Audio.MicphoneSNR(data);
                        break;
                    }
                case "DisConnect":
                    {
                        data = Audio.DisConnect(data);
                        break;
                    }
                case "OpenLedPort":
                    {
                        data = operateLED.OpenLedPort(data);
                        break;
                    }
                case "LED1Test":
                    {
                        data = operateLED.LED1Test(data);
                        break;
                    }
                case "ClosedLEDPort":
                    {
                        data = operateLED.ClosedLEDPort(data);
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

        public List<TestData> GetFailItem()
        {
            List<TestData> list = dataBase.GetFailItem();
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
            if(config.AudioEnable)
            {
                Audio.ExitA2();
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

        public int GetPlugNumber()
        {
            return dataBase.GetPlugNumber();
        }

        public void SetPlugNumber(int number)
        {
            dataBase.UpdataPlugNumber(number);
        }
    }
}
