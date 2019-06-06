using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using TestModel;

namespace TestDAL
{
   public  class ConfigRead
    {
        public static List<InitTestItem> GetInitTestItem()
        {
            List<InitTestItem> list = new List<InitTestItem>();
            //list.Add("CSR_Open_Connection");
            //list.Add("CSR_Check_PsKey");
            //list.Add("CSR_Cal_Freq");
            //list.Add("CSR_Offset_Gain");
            //list.Add("CSR_Enter_TestMode");
            //list.Add("1455_Open_Connection");


            //list.Add("Enter_DUT_Mode");
            list.Add(new InitTestItem()
            {
                TestItem = "Open_MT8852",
                Remark = "打开MT8852 GPIB端口"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "MT8852_Read_BD_Address",
                Remark = "通过MT8852获取耳机蓝牙地址"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "Run_MT8852_Script",
                Remark = "运行MT8852脚本，进行测试"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "DUT_Power",
                Remark = "获取耳机相应频道功率，设置其他参数栏位:Freq:\"相应的频率\""
            });
            list.Add(new InitTestItem()
            {
                TestItem = "DUT_Sensitivity",
                Remark = "获取耳机相应频道BER，设置其他参数栏位:Freq:\"相应的频率\""
            });
            list.Add(new InitTestItem()
            {
                TestItem = "DUT_InitCarrier",
                Remark = "获取耳机初始载波"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "DUT_ModulationIndex",
                Remark = "获取耳机调制指数"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "DUT_CarrierDrift",
                Remark = "获取耳机载波偏移"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "Closed_MT8852",
                Remark = "关闭释放MT8852资源"
            });
            //list.Add("MT8852_Read_BD_Address");
            //list.Add("Run_MT8852_Script");
            //list.Add("DUT_Power");
            //list.Add("DUT_Sensitivity");
            //list.Add("DUT_InitCarrier");
            // list.Add("DUT_ModulationIndex");
            //list.Add("DUT_CarrierDrift");
            //list.Add("Closed_MT8852");
            //list.Add("CSR_Write_PsKey");
            //list.Add("RunExternalApp");
            //list.Add("CSR_Offset_Gain");

            list.Add(new InitTestItem()
            {
                TestItem = "K2300Series_Open",
                Remark = "打开K2300系列电源端口"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "K2300Series_ChannelOne_OutPut",
                Remark = "K2300系列电源通道1输出"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "K2300Series_ChannelTwo_OutPut",
                Remark = "K2300系列电源通道2输出"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "K2300Series_ChannelOne_ReadVoltage",
                Remark = "读取K2300系列电源通道1电压"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "K2300Series_ChannelOne_ReadCurrent",
                Remark = "读取K2300系列电源通道1电流"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "K2300Series_ChannelTwo_ReadVoltage",
                Remark = "读取K2300系列电源通道2电压"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "K2300Series_ChannelTwo_ReadCurrent",
                Remark = "读取K2300系列电源通道2电流"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "K2300Series_ChannelOne_StopOut",
                Remark = "K2300系列电源通道1输出关闭"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "K2300Series_ChannelTwo_StopOut",
                Remark = "K2300系列电源通道2输出关闭"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "K2300Series_Closed",
                Remark = "关闭释放K2300系列电源资源"
            });
            //list.Add("K2300Series_Open");
            //list.Add("K2300Series_ChannelOne_OutPut");
            //list.Add("K2300Series_ChannelTwo_OutPut");
            //list.Add("K2300Series_ChannelOne_ReadVoltage");
            //list.Add("K2300Series_ChannelOne_ReadCurrent");
            //list.Add("K2300Series_ChannelTwo_ReadVoltage");
            //list.Add("K2300Series_ChannelTwo_ReadCurrent");
            //list.Add("K2300Series_ChannelOne_StopOut");
            //list.Add("K2300Series_ChannelTwo_StopOut");
            //list.Add("K2300Series_Closed");

            list.Add(new InitTestItem()
            {
                TestItem = "HP66319D_Open",
                Remark = "打开HP66319D电源端口"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "HP66319D_ChannelOne_OutPut",
                Remark = "HP66319D系列电源通道1输出"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "HP66319D_ChannelTwo_OutPut",
                Remark = "HP66319D系列电源通道2输出"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "HP66319D_ChannelOne_ReadVoltage",
                Remark = "读取HP66319D系列电源通道1电压"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "HP66319D_ChannelOne_ReadCurrent",
                Remark = "读取HP66319D系列电源通道1电流"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "HP66319D_ChannelTwo_ReadVoltage",
                Remark = "读取HP66319D系列电源通道2电压"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "HP66319D_ChannelTwo_ReadCurrent",
                Remark = "读取HP66319D系列电源通道2电流"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "HP66319D_ChannelOne_StopOut",
                Remark = "HP66319D系列电源通道1输出关闭"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "HP66319D_ChannelTwo_StopOut",
                Remark = "HP66319D系列电源通道2输出关闭"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "HP66319D_Closed",
                Remark = "关闭释放HP66319D系列电源资源"
            });

            //list.Add("HP66319D_Open");
            //list.Add("HP66319D_ChannelOne_OutPut");
            //list.Add("HP66319D_ChannelTwo_OutPut");
            //list.Add("HP66319D_ChannelOne_ReadVoltage");
            //list.Add("HP66319D_ChannelOne_ReadCurrent");
            //list.Add("HP66319D_ChannelTwo_ReadVoltage");
            //list.Add("HP66319D_ChannelTwo_ReadCurrent");
            //list.Add("HP66319D_ChannelOne_StopOut");
            //list.Add("HP66319D_ChannelTwo_StopOut");
            //list.Add("HP66319D_Closed");

            list.Add(new InitTestItem()
            {
                TestItem = "Key34461_Open",
                Remark = "打开Key34461电源端口"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "Key34461_ReadVoltage",
                Remark = "Key34461读取电压"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "Key34461_ReadCurrent",
                Remark = "Key34461读取电流"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "Key34461_Closed",
                Remark = "关闭释放Key34461系列电源资源"
            });
            //list.Add("Key34461_Open");
            //list.Add("Key34461_ReadVoltage");
            //list.Add("Key34461_ReadCurrent");
            //list.Add("Key34461_Closed");

            list.Add(new InitTestItem()
            {
                TestItem = "BES_OpenSerialPort",
                Remark = "打开BES系列耳机串口"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Enter_TestMode",
                Remark = "BES系列耳机进入测试模式"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Enter_DUTMode",
                Remark = "BES系列耳机DUT模式"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_SoftVersion",
                Remark = "读取BES系列耳机软体版本"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_HWVersion",
                Remark = "读取BES系列耳机固体版本"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_HALLTest",
                Remark = "BES系列耳机HALL测试，要特别注意测试时未吸合状态"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_WhiteLight",
                Remark = "BES系列耳机白灯测试"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_RedLight",
                Remark = "BES系列耳机红灯测试"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_ReadBTAddress",
                Remark = "读取BES系列耳机蓝牙地址"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_ReadSN",
                Remark = "读取BES系列耳机SN"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_ReadBattarySN",
                Remark = "读取BES系列耳机电池SN"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_ReadProductColor",
                Remark = "读取BES系列耳机产品颜色，在下限中输入，" +
                "包括华为橙色/ 华为黑色/华为绿色/华为银色/华为紫色/华为橘红色" +
                "和荣耀灰色/荣耀蓝色/荣耀红色/荣耀营销色"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Pair",
                Remark = "BES系列耳机配对"
            });

            list.Add(new InitTestItem()
            {
                TestItem = "BES_SetVolume",
                Remark = "BES系列耳机到最大音量"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Shutdown",
                Remark = "BES系列耳机关机"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Reset",
                Remark = "BES系列耳机重启"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_ClearPair",
                Remark = "BES系列耳机清除配对"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_ReadBattaryVoltage",
                Remark = "读取BES系列耳机电池电压"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_ReadNTC",
                Remark = "读取BES系列耳机NTC值"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_ReadWaterProof",
                Remark = "读取BES系列耳机防水值"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_ReadWarningTone",
                Remark = "读取BES系列耳机提示音，在下限中输入，包括：华为中文/英文和荣耀中文/英文"
            });
            //list.Add("BES_OpenSerialPort");
            //list.Add("BES_Enter_TestMode");
            //list.Add("BES_Enter_DUTMode");
            //list.Add("BES_SoftVersion");
            //list.Add("BES_HWVersion");
            //list.Add("BES_HALLTest");
            //list.Add("BES_WhiteLight");
            //list.Add("BES_RedLight");
            //list.Add("BES_ReadSN");
            //list.Add("BES_ReadBTName");
            //list.Add("BES_ReadBTAddress");
            //list.Add("BES_ReadBattarySN");
            //list.Add("BES_ReadProductColor");
            //list.Add("BES_Pair");
            //list.Add("BES_Shutdown");
            //list.Add("BES_Reset");
            //list.Add("BES_ClearPair");
            //list.Add("BES_ReadBattaryVoltage");
            //list.Add("BES_ReadNTC");
            //list.Add("BES_ReadWaterProof");
            //list.Add("BES_ReadWarningTone");

            list.Add(new InitTestItem()
            {
                TestItem = "BES_ReadChargeMode",
                Remark = "读取BES系列耳机充电模式，设置测试项目时下限请设置工厂模式/用户模式"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_ReadTestStation",
                Remark = "读取BES系列耳机工站信息，设置测试项目时下限请设置0-F"
            });
            //list.Add("BES_ReadChargeMode");
            //list.Add("BES_ReadTestStation");

            list.Add(new InitTestItem()
            {
                TestItem = "BES_CompareSN",
                Remark = "比对BES系列耳机SN，该项目会和输入的耳机SN进行比对，该项目不能和电池SN比对一起测试"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_CompareBattarySN",
                Remark = "比对BES系列耳机电池SN，该项目会和输入的电池SN进行比对，该项目不能和耳机SN比对一起测试"
            });
            //list.Add("BES_CompareSN");
            //list.Add("BES_CompareBattarySN");
            list.Add(new InitTestItem()
            {
                TestItem = "BES_WriteSN",
                Remark = "写入BES系列耳机SN，不能和电池SN一起写入"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_WriteBattarySN",
                Remark = "写入BES系列耳机电池SN，不能和耳机SN一起写入"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_WriteBTAddress",
                Remark = "写入BES系列耳机蓝牙地址，不能和耳机SN/电池SN一起写入"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_WriteProductColor",
                Remark = "写入BES系列耳机产品颜色，在下限中输入，" +
                "包括华为橙色/ 华为黑色/华为绿色/华为银色/华为紫色/华为橘红色" +
                "和荣耀灰色/荣耀蓝色/荣耀红色/荣耀营销色"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_WriteWarningTone",
                Remark = "写入BES系列耳机蓝牙提示音，在下限中输入，包括：华为中文/英文和荣耀中文/英文"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_WriteChargeMode",
                Remark = "写入BES系列耳机蓝牙充电模式，设置测试项目时下限请设置工厂模式/用户模式"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_WriteTestStation",
                Remark = "写入BES系列耳机蓝牙工站信息，设置测试项目时下限请设置0-F"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_WriteHWVersion",
                Remark = "写入BES系列耳机蓝牙硬件版本"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_ClosedSerialPort",
                Remark = "关闭和释放BES系列耳机串口"
            });
            //list.Add("BES_WriteSN");
            //list.Add("BES_WriteBattarySN");
            //list.Add("BES_WriteBTAddress");
            //list.Add("BES_WriteProductColor");
            //list.Add("BES_WriteWarningTone");
            //list.Add("BES_WriteChargeMode");
            //list.Add("BES_WriteTestStation");
            //list.Add("BES_ClosedSerialPort");
            //list.Add("4010");
            //list.Add("BES_ChargeMode");
            //list.Add("BES_ChargeMode");
            //list.Add("BES_ChargeMode");
            //list.Add("BES_ChargeMode");

            //list.Add("CSR_Open_Connection");
            //list.Add("CSR_Open_Connection");
            //list.Add("CSR_Open_Connection");

            list.Add(new InitTestItem()
            {
                TestItem = "SwitchToA2dp",
                Remark = "控制耳机进入A2DP模式"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "SwitchToHfp",
                Remark = "控制耳机进入HFP模式"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "EarPair",
                Remark = "A2和耳机配对"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "SpeakerLevel_Left",
                Remark = "测试左SPK电平"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "SpeakerLevel_Right",
                Remark = "测试右SPK电平"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "SpeakerTHD_Left",
                Remark = "测试左SPK THD+N"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "SpeakerTHD_Right",
                Remark = "测试右SPK THD+N"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "SpeakerSNR_Left",
                Remark = "测试左SPK信噪比"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "SpeakerSNR_Right",
                Remark = "测试右SPK信噪比"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "SpeakerCrosstalk_Left",
                Remark = "测试左SPK Crosstalk"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "SpeakerCrosstalk_Right",
                Remark = "测试右SPK Crosstalk"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "MicphoneLevel",
                Remark = "测试Mic电平"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "MicphoneTHD",
                Remark = "测试Mic THD+N"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "MicphoneSNR",
                Remark = "关闭和释放BES系列耳机串口"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "DisConnect",
                Remark = "断开A2与耳机的连接"
            });

            return list;
        }
    }
}
