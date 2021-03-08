using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestModel;
using TestTool;
using System.Data;
using TestDAL;
using System.Threading;
using System.IO.Ports;
using System.Web.Services.Description;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;


namespace TestBLL
{
    public class TestLogic
    {
        private Queue<string> testQueue,statusQueue;
        private string initPath;
        private DataBase dataBase;
        private OperateInstrument operate;
        private ConfigData config;
        private CsrOperate csr;
        private OperateBES operateBES;
        private OperateRelay operateRelay;
        private OperateMicroChip operateMicroChip;
        public string BTAddress;
        public string PorductSN;
        public string BattarySN;
        private AudioOperate Audio;
        private OperateLED operateLED;
        public SerialPort FixPort;
        public string PackSN;
        public OperatrAiroha operatrAiroha;

        public TestLogic(Queue<string> queue, string path,Queue<string> statusQueue)
        {
            this.testQueue = queue;
            this.statusQueue = statusQueue;
            this.initPath = path;
            dataBase = new DataBase(path);
            config = GetConfigData();
            //csr = new CsrOperate();
            operateBES = new OperateBES(config.SerialPort
                , testQueue, config.SerialSelect, statusQueue);
            //if(config.AutoSNTest)
            //{
            operateBES.config = config;
            operateBES.dataBase = dataBase;
            //operateBES.BES_WriteSN(new TestData());
            //}

            operateMicroChip = new OperateMicroChip(config);
            operatrAiroha = new OperatrAiroha(config);
            operate = new OperateInstrument(config, testQueue);
            if(config.AudioEnable)
            {
                queue.Enqueue("加载Audio项目文件");
                Audio = new AudioOperate(config, operateBES, queue);
                queue.Enqueue("加载Audio项目文件完成");
            }
            if(config.LEDEnable)
            {
                queue.Enqueue("打开LED测试仪");
                operateLED = new OperateLED(config);
            }
            if(config.RelayEnable)
            {
                queue.Enqueue("打开继电器");
                operateRelay = new OperateRelay(config);
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
                case "MessageBox":
                    {
                        string[] agrm = data.Other.Split(';');
                       
                        var box = AutoClosingMessageBox.Show(agrm[0], "Message"
                            , int.Parse(agrm[1]), MessageBoxButtons.YesNo);
                        //var box = MessageBox.Show(data.Other, "Message"
                        //    , MessageBoxButtons.YesNo,
                        //    MessageBoxIcon.Question);
                        if(box == DialogResult.Yes)
                        {
                            data.Result = "Pass";
                            data.Value = "Pass";
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = "Fail";
                        }
                        break;
                    }
                case "QCC_Open_Connection":
                    {
                        data = operate.Open_QCC_Port(data);
                        break;
                    }
                case "QCC_ReadBtAdress":
                    {
                        data = operate.QCC_ReadBtAdress(data);
                        break;
                    }
                case "QCC_Write_BtAddress":
                    {
                        operate.BtAddress = BTAddress;
                        data = operate.QCC_Write_BtAddress(data);
                        break;
                    }
                case "QCC_Cal_Freq":
                    {
                        data = operate.QCC_CalFreq(data);
                        break;
                    }
                case "QCC_Offset_Gain":
                    {
                        data = operate.QCC_Read_Trim(data);
                        break;
                    }
                case "QCC_Enter_TestMode":
                    {
                        data = operate.QCC_EnableTestMode(data);
                        break;
                    }
                case "QCC_StartAudioLoop":
                    {
                        data = operate.QCC_StartAudioLoop(data);
                        break;
                    }
                case "QCC_StopAudioLoop":
                    {
                        data = operate.QCC_StopAudioLoop(data);
                        break;
                    }
                case "QCC_Cloesd_Port":
                    {
                        data = operate.QCC_Closed_Port(data);
                        break;
                    }
                case "OpenCsrDev":
                    {
                        data = operate.OpenCsrDev(data);
                        break;
                    }
                case "ReadCsrBDAddress":
                    {
                        data = operate.ReadCsrBDAddress(data);
                        break;
                    }
                case "CsrEnableTestMode":
                    {
                        data = operate.CsrEnableTestMode(data);
                        break;
                    }
                case "CsrClosedPort":
                    {
                        data = operate.CsrClosedPort(data);
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
                case "Run_MT8852_CalcFreq":
                    {
                        data = operate.Run_MT8852_CalcFreq(data);
                        break;
                    }
                case "Run_MT8852_CsrCalcFreq":
                    {
                        data = operate.Run_MT8852_CsrCalcFreq(data);
                        break;
                    }
                case "Run_MT8852_CalcFreqScript":
                    {
                        byte sampTrim = 0x44;
                        TestData calData = null;
                        operateBES.BES_WriteTrim(sampTrim);
                        for (int i = 0; i < 5; i++)
                        {
                            if (operate.Run_MT8852_CalcFreqScript(data).Result == "Pass")
                            {
                                calData = operate.GetInitialcarrier(data);
                                testQueue.Enqueue("频率偏移:" + calData.Value);
                                if (calData.Result == "Pass")
                                {
                                    data.Result = "Pass";
                                    data.Value = calData.Value;
                                    operate.InitInstr();
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
                                if (i == 4)
                                {
                                    data.Result = "Fail";
                                    data.Value = calData.Value;
                                }
                            }
                            else
                            {
                                data.Result = "Fail";
                                data.Value = "Fail";
                                break;
                            }
                        }
                        //data = operate.Run_MT8852_CalcFreqScript(data);
                        break;
                    }

                case "Run_MT8852_Lchse_CalcFreqScript":
                    {
                        byte sampTrim = 0x3f;
                        TestData calData = null;
                        operateBES.BES_Lchse_WriteTrim(sampTrim);
                        for (int i = 0; i < 10; i++)
                        {
                            if (operate.Run_MT8852_CalcFreqScript(data).Result == "Pass")
                            {
                                calData = operate.GetInitialcarrier(data);
                                testQueue.Enqueue("频率偏移:" + calData.Value);
                                if (calData.Result == "Pass")
                                {
                                    data.Result = "Pass";
                                    data.Value = calData.Value;
                                    operate.InitInstr();
                                    break;
                                }
                                else
                                {
                                    int freqOff = (int)double.Parse(calData.Value) * 2;
                                   // int tr = operateBES.BES_Lchse_ReadTrim();
                                    byte trim = (byte)(sampTrim -
                                        Convert.ToByte(Math.Abs(freqOff)));
                                    testQueue.Enqueue("写入Trim:" + trim);
                                    //Thread.Sleep(500);
                                    operateBES.BES_Lchse_WriteTrim(trim);
                                    sampTrim = trim;
                                }
                                Thread.Sleep(1000);
                                if (i == 4)
                                {
                                    data.Result = "Fail";
                                    data.Value = calData.Value;
                                }
                            }
                            else
                            {
                                data.Result = "Fail";
                                data.Value = "Fail";
                                break;
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
                case "MT8852_Read_BD_Name":
                    {
                        data = operate.GetBTName(data);
                        break;
                    }
                case "MT8852_Compare_BD_Address":
                    {
                        data = operate.CompareBTAddress(data);
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
                case "DUT_Sensitivity_FER":
                    {
                        data = operate.GetSingleSensitivity_FER(data);
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

                case "K2303Series_Open":
                    {
                        data = operate.K2303Series_Open(data);
                        break;
                    }
                case "K2303Series_ChannelOne_OutPut":
                    {
                        data = operate.K2303Series_ChannelOne_OutPut(data);
                        break;
                    }
                case "K2303Series_ChannelOne_OverVoltage":
                    {
                        data = operate.K2303Series_ChannelOne_OverVoltage(data);
                        break;
                    }
                case "K2303Series_ChannelOne_ReadVoltage":
                    {
                        data = operate.K2303Series_ChannelOne_ReadVoltage(data);
                        break;
                    }
                case "K2303Series_ChannelOne_ReadCurrent":
                    {
                        data = operate.K2303Series_ChannelOne_ReadCurrent(data);
                        break;
                    }
                case "K2303Series_ChannelOne_StopOut":
                    {
                        data = operate.K2303Series_ChannelOne_StopOut(data);
                        break;
                    }
                case "K2303Series_Closed":
                    {
                        data = operate.K2303Series_Closed(data);
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
                        if(PackSN != null && PackSN.Length == 20)
                        {
                            operateBES.PackSN = PackSN;
                        }
                        
                        data = operateBES.BES_ReadSN(data);
                        break;
                    }
                case "BES_CompareSN":
                    {
                        if (PackSN != null && PackSN.Length == 20)
                        {
                            operateBES.PackSN = PackSN;
                        }
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
                        operateBES.PackSN = PackSN;
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
                        if(config.AutoHALL)
                        {
                            operateBES.FixPort = this.FixPort;
                        }
                        operateBES.BES_HALLTest(data);
                        break;
                    }
                case "BES_PowerKeyTest":
                    {
                        operateBES.BES_PowerKeyTest(data);
                        break;
                    }
                case "BES_HALLClosedTest":
                    {
                        operateBES.BES_HALLClosedTest(data);
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
                case "BES_ReadPackSN":
                    {
                        operateBES.PackSN = PackSN;
                        data = operateBES.BES_ReadPackSN(data);
                        break;
                    }
                case "BES_WritePackSN":
                    {
                        operateBES.PackSN = PackSN;
                        data = operateBES.BES_WritePackSN(data);
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
                        operateBES.btAddress = BTAddress;
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
                case "BES_BurnIN":
                    {
                        data = operateBES.BES_BurnIN(data);
                        break;
                    }
                case "BES_ReadBurnIN":
                    {
                        data = operateBES.BES_ReadBurnIN(data);
                        break;
                    }
                case "BES_Read_NormalBurnIN_HALL":
                    {
                        data = operateBES.BES_Read_NormalBurnIN_HALL(data);
                        break;
                    }
                case "BES_Read_NormalBurnIN_Power":
                    {
                        data = operateBES.BES_Read_NormalBurnIN_Power(data);
                        break;
                    }
                case "BES_Read_NormalBurnIN_Charge":
                    {
                        data = operateBES.BES_Read_NormalBurnIN_Charge(data);
                        break;
                    }
                case "BES_Read_MaxPowerBurnIN_HALL":
                    {
                        data = operateBES.BES_Read_MaxPowerBurnIN_HALL(data);
                        break;
                    }
                case "BES_Read_MaxPowerBurnIN_Power":
                    {
                        data = operateBES.BES_Read_MaxPowerBurnIN_Power(data);
                        break;
                    }
                case "BES_Read_MaxPowerBurnIN_Charge":
                    {
                        data = operateBES.BES_Read_MaxPowerBurnIN_Charge(data);
                        break;
                    }
                case "BES_Read_SleepBurnIN_HALL":
                    {
                        data = operateBES.BES_Read_SleepBurnIN_HALL(data);
                        break;
                    }
                case "BES_Read_SleepBurnIN_Power":
                    {
                        data = operateBES.BES_Read_SleepBurnIN_Power(data);
                        break;
                    }
                case "BES_Read_SleepBurnIN_Charge":
                    {
                        data = operateBES.BES_Read_SleepBurnIN_Charge(data);
                        break;
                    }
                case "BES_Read_BurnIN_Memory":
                    {
                        data = operateBES.BES_Read_BurnIN_Memory(data);
                        break;
                    }
                case "BES_Read_BurnIN_LowBattery":
                    {
                        data = operateBES.BES_Read_BurnIN_LowBattery(data);
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
                case "BES_Semi_Product_CheckMacAddress":
                    {
                        data = operateBES.BES_Semi_Product_CheckMacAddress(data);
                        break;
                    }
                case "BES_Semi_Product_CheckProductSN":
                    {
                        data = operateBES.BES_Semi_Product_CheckProductSN(data);
                        break;
                    }
                case "BES_Pack_CheckMacAddress":
                    {
                        data = operateBES.BES_Pack_CheckMacAddress(data);
                        break;
                    }
                case "BES_Pack_CheckProductSN":
                    {
                        data = operateBES.BES_Pack_CheckProductSN(data);
                        break;
                    }
                case "BES_Semi_Product_CheckBatterySN":
                    {
                        data = operateBES.BES_Semi_Product_CheckBatterySN(data);
                        break;
                    }
                case "BES_Assy_CRCBatterySN":
                    {
                        data = operateBES.BES_Assy_CRCBatterySN(data);
                        break;
                    }
                case "BES_Pack_CRCBatterySN":
                    {
                        data = operateBES.BES_Pack_CRCBatterySN(data);
                        break;
                    }
                case "BES_Assy_CRCCheckMacAddress":
                    {
                        data = operateBES.BES_Assy_CRCCheckMacAddress(data);
                        break;
                    }
                case "BES_Assy_CRCCheckProductSN":
                    {
                        data = operateBES.BES_Assy_CRCCheckProductSN(data);
                        break;
                    }
                case "BES_Pack_CheckPackSN":
                    {
                        operateBES.PackSN = PackSN;
                        data = operateBES.BES_Pack_CheckPackSN(data);
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
                case "BES_ExitCDC":
                    {
                        data = operateBES.BES_ExitCDC(data);
                        break;
                    }
                case "BES_Enter_UsbAudio":
                    {
                        data = operateBES.BES_Enter_UsbAudio(data);
                        break;
                    }
                case "BES_ReadTouchData":
                    {
                        data = operateBES.BES_ReadTouchData(data);
                        break;
                    }
                case "BES_ReadWearData":
                    {
                        data = operateBES.BES_ReadWearData(data);
                        break;
                    }
                case "BES_TWS_TXMode":
                    {
                        data = operateBES.BES_TWS_TXMode(data);
                        break;
                    }
                case "BES_TWS_DUTMode":
                    {
                        data = operateBES.BES_TWS_DUTMode(data);
                        break;
                    }
                case "BES_TWS_ReadVersion":
                    {
                        data = operateBES.BES_TWS_ReadVersion(data);
                        break;
                    }
                case "BES_TWS_ReadMACAddress":
                    {
                        data = operateBES.BES_TWS_ReadMACAddress(data);
                        break;
                    }
                case "BES_TWS_MainMic":
                    {
                        data = operateBES.BES_TWS_MainMic(data);
                        break;
                    }
                case "BES_TWS_FFMic":
                    {
                        data = operateBES.BES_TWS_FFMic(data);
                        break;
                    }
                case "BES_TWS_ClearAll":
                    {
                        data = operateBES.BES_TWS_ClearAll(data);
                        break;
                    }
                case "BES_TWS_AodioLoop":
                    {
                        data = operateBES.BES_TWS_AodioLoop(data);
                        break;
                    }
                case "BES_TWS_ComparePairName":
                    {
                        data = operateBES.BES_TWS_ComparePairName(data);
                        break;
                    }

                case "BES_Lchse_TWS_EnterDUT":
                    {
                        data = operateBES.BES_Lchse_TWS_EnterDUT(data);
                        break;
                    }
                case "BES_Lchse_TWS_ReadSoftVersion":
                    {
                        data = operateBES.BES_Lchse_TWS_ReadSoftVersion(data);
                        break;
                    }
                case "BES_Lchse_TWS_ReadHWVersion":
                    {
                        data = operateBES.BES_Lchse_TWS_ReadHWVersion(data);
                        break;
                    }
                case "BES_Lchse_TWS_ReadVoltage":
                    {
                        data = operateBES.BES_Lchse_TWS_ReadVoltage(data);
                        break;
                    }
                case "BES_Lchse_TWS_ReadElectricity":
                    {
                        data = operateBES.BES_Lchse_TWS_ReadElectricity(data);
                        break;
                    }
                case "BES_Lchse_TWS_ReadBTAddress":
                    {
                        data = operateBES.BES_Lchse_TWS_ReadBTAddress(data);
                        break;
                    }
                case "BES_Lchse_TWS_WriteBTAddress":
                    {
                        operateBES.btAddress = BTAddress;
                        data = operateBES.BES_Lchse_TWS_WriteBTAddress(data);
                        break;
                    }
                case "BES_Lchse_TWS_WriteBTName":
                    {
                        //operateBES.btAddress = BTAddress;
                        data = operateBES.BES_Lchse_TWS_WriteBTName(data);
                        break;
                    }
                case "BES_Lchse_TWS_ReadBTName":
                    {
                        data = operateBES.BES_Lchse_TWS_ReadBTName(data);
                        break;
                    }
                case "BES_Lchse_TWS_ShutDown":
                    {
                        data = operateBES.BES_Lchse_TWS_ShutDown(data);
                        break;
                    }
                case "BES_Lchse_TWS_Reset":
                    {
                        data = operateBES.BES_Lchse_TWS_Reset(data);
                        break;
                    }
                case "BES_Lchse_TWS_CloseLog":
                    {
                        data = operateBES.BES_Lchse_TWS_CloseLog(data);
                        break;
                    }
                case "BES_Lchse_TWS_04cCharge":
                    {
                        data = operateBES.BES_Lchse_TWS_04cCharge(data);
                        break;
                    }
                case "BES_Lchse_TWS_1cCharge":
                    {
                        data = operateBES.BES_Lchse_TWS_1cCharge(data);
                        break;
                    }
                case "BES_Lchse_TWS_TalkMic":
                    {
                        data = operateBES.BES_Lchse_TWS_TalkMic(data);
                        break;
                    }
                case "BES_Lchse_TWS_FFMIC":
                    {
                        data = operateBES.BES_Lchse_TWS_FFMIC(data);
                        break;
                    }
                case "BES_Lchse_TWS_FBMIC":
                    {
                        data = operateBES.BES_Lchse_TWS_FBMIC(data);
                        break;
                    }
                case "OpenMicroChipPort":
                    {
                        data = operateMicroChip.OpenMicroChipPort(data);
                        break;
                    }
                case "ClosedMicroChipPort":
                    {
                        data = operateMicroChip.ClosedMicroChipPort(data);
                        break;
                    }
                case "MicroChipReadBTAddress":
                    {
                        data = operateMicroChip.MicroChipReadBTAddress(data);
                        break;
                    }
                case "MicroChipReadNTC":
                    {
                        data = operateMicroChip.MicroChipReadNTC(data);
                        break;
                    }
                case "MicroChipReadVoltage":
                    {
                        data = operateMicroChip.MicroChipReadVoltage(data);
                        break;
                    }
                case "MicroChipReadTone":
                    {
                        data = operateMicroChip.MicroChipReadTone(data);
                        break;
                    }
                case "MicroChipReadSoftVersion":
                    {
                        data = operateMicroChip.MicroChipReadSoftVersion(data);
                        break;
                    }
                case "MicroChipEntenDUT":
                    {
                        data = operateMicroChip.MicroChipEntenDUT(data);
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
                case "CSR_EarPair":
                    {
                        Audio.btAddress = BTAddress;
                        data = Audio.CSR_EarPair(data);
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
                case "OpenRelay":
                    {
                        data = operateRelay.OpenRelay(data);
                        break;
                    }
                case "OpenChannel":
                    {
                        data = operateRelay.OpenChannel(data);
                        break;
                    }
                case "ClosedChannel":
                    {
                        data = operateRelay.ClosedChannel(data);
                        break;
                    }
                case "OpenAllChannel":
                    {
                        data = operateRelay.OpenAllChannel(data);
                        break;
                    }
                case "ClosedAllChannel":
                    {
                        data = operateRelay.ClosedAllChannel(data);
                        break;
                    }
                case "ClosedRelay":
                    {
                        data = operateRelay.ClosedRelay(data);
                        break;
                    }

                case "OpenAirohaPort":
                    {
                        data = operatrAiroha.OpenAirohaPort(data);
                        break;
                    }
                case "AirohaClosedLog":
                    {
                        data = operatrAiroha.AirohaClosedLog(data);
                        break;
                    }
                case "AirohaReadBtAddress":
                    {
                        data = operatrAiroha.AirohaReadBtAddress(data);
                        break;
                    }
                case "AirohaReadBtName":
                    {
                        data = operatrAiroha.AirohaReadBtName(data);
                        break;
                    }
                case "AirohaReadSoftVersion":
                    {
                        data = operatrAiroha.AirohaReadSoftVersion(data);
                        break;
                    }
                case "AirohaEnterDUT":
                    {
                        data = operatrAiroha.AirohaEnterDUT(data);
                        break;
                    }
                case "AirohaMainMic":
                    {
                        data = operatrAiroha.AirohaMainMic(data);
                        break;
                    }
                case "AirohaPowerOff":
                    {
                        data = operatrAiroha.AirohaPowerOff(data);
                        break;
                    }
                case "AirohaFFMic":
                    {
                        data = operatrAiroha.AirohaFFMic(data);
                        break;
                    }
                case "AirohaClosePort":
                    {
                        data = operatrAiroha.AirohaClosePort(data);
                        break;
                    }
                case "TouchTest":
                    {
                        List<double> list = GetConsoleData();
                        data.Value = string.Join(";", list);
                        for (int i = 0; i < list.Count; i++)
                        {
                            if(list[i] >= double.Parse(data.LowLimit) 
                                && list[i] <= double.Parse(data.UppLimit))
                            {
                                data.Result = "Pass";
                            }
                            else
                            {
                                data.Result = "Fail";
                                break;
                            }
                        }

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
            if(csr != null)
            {
                csr.CsrClosedPort(new TestData());
                csr.QCCClosedDev();
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

        public List<double> GetConsoleData()
        {
            List<double> list = new List<double>();
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.WorkingDirectory = Path.Combine(Environment.CurrentDirectory, "Touch");
            p.Start();
            p.StandardInput.WriteLine("TouchTest.exe");
            p.StandardInput.WriteLine("exit");
            p.StandardInput.AutoFlush = true;
            p.WaitForExit();//等待程序执行完退出进程
            string[] data = p.StandardOutput.ReadToEnd().Split(new char[] { '\r','\n'}
            ,StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < data.Length; i++)
            {
                if(data[i].Contains(",") && char.IsNumber(data[i][1]))
                {
                    list = Array.ConvertAll(data[i].Split(','), double.Parse).ToList();
                }
            }
            p.Close();
            return list;
        }
    }
}
