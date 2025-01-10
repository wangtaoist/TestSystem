using System.Collections.Generic;
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
                TestItem = "MessageBox",
                Remark = "弹出信息框,在其他配置中写入要提示的内容,格式:提示框内容:界面宽:界面高;" +
               "提示框超时时间,设置为0时就是取消超时,默认宽450高200例:请拔出USB线;2000;500;300"
            });

            list.Add(new InitTestItem()
            {
                TestItem = "CmdCommand",
                Remark = "通过CMD指令运行其他程序并获取返回值"
            });

            list.Add(new InitTestItem()
            {
                TestItem = "QCC_Open_Connection",
                Remark = "打开QCC端口，使用方式：USBDBG;SPI,TRB，" +
                "在Other中设置端口及端口号，例：USBDBG:100;1,SPI:23675;1,TRB:23456;1"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "QCC_ReadBtAdress",
                Remark = "读取QCC蓝牙地址"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "QCC_Write_BtAddress",
                Remark = "写QCC蓝牙地址"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "QCC_Cal_Freq",
                Remark = "QCC校正频率，校正时需要一直处于开机状态"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "QCC_Offset_Gain",
                Remark = "读取QCC校正Trim值"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "QCC_Enter_TestMode",
                Remark = "QCC进入测试模式"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "QCC_StartAudioLoop",
                Remark = "QCC进入AudioLoopBack模式"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "QCC_StopAudioLoop",
                Remark = "QCC退出AudioLoopBack模式"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "QCC_Cloesd_Port",
                Remark = "关闭QCC端口"
            });

            list.Add(new InitTestItem()
            {
                TestItem = "OpenCsrDev",
                Remark = "打开CSR端口,使用方式：USB;SPI;UART;USB-SPI," +
                "在Other中设置端口及端口号，例：USB-SPI:0"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "ReadCsrBDAddress",
                Remark = "读取CSR芯片蓝牙地址"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "CsrEnableTestMode",
                Remark = "读取CSR芯片进入DUT"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "CsrClosedPort",
                Remark = "关闭CSR端口"
            });

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
                TestItem = "MT8852_Compare_BD_Address",
                Remark = "通过MT8852获取并比对耳机蓝牙地址前6位，在下限中填入"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "MT8852_Read_BD_Name",
                Remark = "通过MT8852获取耳机蓝牙地址并进行比对"
            });

            list.Add(new InitTestItem()
            {
                TestItem = "Run_MT8852_Script",
                Remark = "运行MT8852脚本，进行测试"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "Run_MT8852_CalcFreqScript",
                Remark = "运行MT8852校准频率脚本，进行测试，适用于BES芯片"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "Run_MT8852_Lchse_CalcFreqScript",
                Remark = "运行MT8852校准频率脚本，进行测试，适用于联创自研BES芯片"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "Run_MT8852_CalcFreq",
                Remark = "运行MT8852校准频率脚本，进行测试，适用于QCC芯片"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "Run_MT8852_CsrCalcFreq",
                Remark = "运行MT8852校准频率脚本，进行测试，适用于CSR芯片"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "Run_MT8852_BLE",
                Remark = "运行MT8852脚本，进行BLE测试"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "Set8852LossOne",
                Remark = "设置8852第一组Loss，设置时两组Loss中间用;分开，例如：-10;-10"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "Set8852LossTwo",
                Remark = "设置8852第二组Loss，设置时两组Loss中间用;分开，例如：-10;-10"
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
                TestItem = "DUT_Sensitivity_FER",
                Remark = "获取耳机相应频道FER，设置其他参数栏位:Freq:\"相应的频率\""
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
                TestItem = "DUT_CarrierModRMS2M",
                Remark = "获取耳机ECM(EVM)均方根2M数据，该项目要先测试，设置其他参数栏位:Freq:\"相应的频率\""
            });
            list.Add(new InitTestItem()
            {
                TestItem = "DUT_GetCarrierModRMS3M",
                Remark = "获取耳机ECM(EVM)均方根3M数据，设置其他参数栏位:Freq:\"相应的频率\""
            });
            list.Add(new InitTestItem()
            {
                TestItem = "DUT_GetCarrierModPeak2M",
                Remark = "获取耳机ECM(EVM)峰值2M数据，设置其他参数栏位:Freq:\"相应的频率\""
            });
            list.Add(new InitTestItem()
            {
                TestItem = "DUT_GetCarrierModPeak3M",
                Remark = "获取耳机ECM(EVM)峰值3M数据，设置其他参数栏位:Freq:\"相应的频率\""
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
                TestItem = "Agilent4010_CalcFreq",
                Remark = "使用Agilent4010对耳机进行频率校准，" +
                "通过测试指令写入trim值，适用于BES芯片"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "Open_4010",
                Remark = "打开Agilent4010端口"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "Closed_4010",
                Remark = "关闭Agilent4010端口"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "Run_4010CalcFreqAfterScript",
                Remark = "运行Agilent4010校正频率之后测试Script"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "Run_4010Script",
                Remark = "运行Agilent4010测试Script"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "Get4010TXPower",
                Remark = "测试Agilent4010发送功率"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "Get4010SingleSensitivity",
                Remark = "测试Agilent4010BER"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "Get4010Modulationindex",
                Remark = "测试Agilent4010调制指数"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "Get4010Initialcarrier",
                Remark = "测试Agilent4010初始载波"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "Get4010Carrierdrift",
                Remark = "测试Agilent4010载波漂移"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "Get4010BTAddress",
                Remark = "获取Agilent测试耳机蓝牙地址"
            });
            //通用电源控制
            list.Add(new InitTestItem()
            {
                TestItem = "PowerSupplyCommon",
                Remark = "检测电源型号，2303/2306/66319"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "PowerSupplyCommonOpen",
                Remark = "通用电源打开"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "PowerSupplyCommonChannelOneOut",
                Remark = "通用电源通道一输出"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "PowerSupplyCommonChannelTwoOut",
                Remark = "通用电源通道二输出"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "PowerSupplyCommonChannelOneOverVoltage",
                Remark = "通用电源通道1输出过压电压，电压在下限设置"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "PowerSupplyCommonChannelTwoOverVoltage",
                Remark = "通用电源通道2输出过压电压，电压在下限设置"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "PowerSupplyCommonChannelOneReadCurrent",
                Remark = "通用电源读取通道1电流"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "PowerSupplyCommonChannelOneReadVoltage",
                Remark = "通用电源读取通道1电压"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "PowerSupplyCommonChannelTwoReadCurrent",
                Remark = "通用电源读取通道2电流"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "PowerSupplyCommonChannelTwoReadVoltage",
                Remark = "通用电源读取通道2电压"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "PowerSupplyCommonChannelOneStopOut",
                Remark = "通用电源读取通道1关闭输出"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "PowerSupplyCommonChannelTwoStopOut",
                Remark = "通用电源读取通道2关闭输出"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "PowerSupplyCommonClose",
                Remark = "通用电源关闭"
            });

            list.Add(new InitTestItem()
            {
                TestItem = "K2300Series_Open",
                Remark = "K2300系列电源打开"
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
                TestItem = "K2300Series_ChannelOne_OverVoltage",
                Remark = "K2300系列电源通道1输出过压电压，电压在下限设置"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "K2300Series_ChannelTwo_ChargeOutPut",
                Remark = "K2300系列电源通道2输出充电电压，电压在下限设置"
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
                TestItem = "K2300Series_ChannelTwo_OverVoltage",
                Remark = "K2300系列电源通道2输出过压电压，电压在下限设置"
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

            list.Add(new InitTestItem()
            {
                TestItem = "K2303Series_Open",
                Remark = "打开K2303电源资源"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "K2303Series_ChannelOne_OutPut",
                Remark = "K2303电源通道输出"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "K2303Series_ChannelOne_OverVoltage",
                Remark = "K2303电源过压输出"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "K2303Series_ChannelOne_ReadVoltage",
                Remark = "K2303电源读取电压"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "K2303Series_ChannelOne_ReadCurrent",
                Remark = "K2303电源读取电流"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "K2303Series_ChannelOne_StopOut",
                Remark = "K2303关闭输出"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "K2303Series_Closed",
                Remark = "关闭释放K2303系列电源资源"
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
                TestItem = "HP66319D_ChannelOne_OverVoltage",
                Remark = "HP66319D系列电源通道1输出过压电压，电压在下限设置"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "HP66319D_ChannelTwo_ChargeOutPut",
                Remark = "HP66319D系列电源通道2输出充电电压，电压在下限设置"
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
                TestItem = "HP66319D_ChannelTwo_OverVoltage",
                Remark = "HP66319D系列电源通道2输出过压电压，电压在下限设置"
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
                Remark = "Key34461读取直流电压"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "Key34461_ReadACVoltage",
                Remark = "Key34461读取交流电压"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "Key34461_ReadResistance",
                Remark = "Key34461读取电阻，单位：Ω，KΩ，MΩ"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "Key34461_ReadContinuity",
                Remark = "Key34461读取通断"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "Key34461_ReadCurrent",
                Remark = "Key34461读取电流，单位：mA,uA"
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
                Remark = "打开BES系列通讯串口，在其他配置中设置波特率" 
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_OpenOppoSerialPort",
                Remark = "打开BES系列OPPO耳机串口"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_OpenOneLineSerialPort",
                Remark = "打开BES系列耳机单线通讯串口"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "PublicByteSerialPortCommand",
                Remark = "通用串口指令(byte数据类型)，数据格式:指令;跳过几个byte;第几位byte是数据长度,偏移位数;编码方式：ASCII/16/10/bt/voltage;是否有返回;命令格式：ASCII/BYTE" +
                "例如：55 AA FF 17 00 A5;5;3,2;16;true;BYTE"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "PublicAsciiSerialPortCommand",
                Remark = "通用串口指令(Aecii数据类型)，数据格式:指令;匹配字符;分割符;是否返回;读取延迟时间;是否写入;写入位置;写入数据" +
                "例如：$L^PHYNUM?;^PHYNUM:;\r;true;300,true,0,0"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "PublicWriteSerialNumber",
                Remark = "通用写入产品SN，数据格式:指令;是否需要结尾指令;结尾指令;跳过几个byte;第几位byte是数据长度,偏移位数;编码方式：ASCII/16/10" +
                "例如：4C 43 14 00 20;true;AA;3;1;16"

            });
            list.Add(new InitTestItem()
            {
                TestItem = "PublicReadAndCompareSerialNumber",
                Remark = "通用读取和比对SN，数据格式:指令;跳过几个byte;第几位byte是数据长度,偏移位数;编码方式：ASCII/16/10" +
              "例如：4C 43 14 01 AA ;4;16;16"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "CompareTestValue",
                Remark = "通用串口指令(byte数据类型)，数据格式:测试界面那个项目和那个项目数据进行比对；如果比对4个地址，" +
                "需要其中3个地址为一致，1和2位tws地址，3和4位耳机本地地址例如：2;3"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "EnterProductLineTestMode",
                Remark = "X5i项目充电盒进入产线测试模式"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "Readx5sBattaryLevel",
                Remark = "X5i项目充电盒读取电量"
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
            //list.Add(new InitTestItem()
            //{
            //    TestItem = "BES_HWVersion",
            //    Remark = "读取BES系列耳机固体版本"
            //});
            list.Add(new InitTestItem()
            {
                TestItem = "BES_ReadTrim",
                Remark = "读取BES系列耳机校正频率Trim值"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_HWVersion_ASCII",
                Remark = "读取BES系列耳机固体版本_ASCII"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_HALLTest",
                Remark = "BES系列耳机HALL测试，要特别注意测试时未吸合状态"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_HALLClosedTest",
                Remark = "BES系列耳机HALL测试，只测试霍尔在吸合状态"
            });
            //list.Add(new InitTestItem()
            //{
            //    TestItem = "BES_PowerKeyTest",
            //    Remark = "BES系列耳机开机键测试(侧按键)"
            //});
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
                "和荣耀灰色/荣耀蓝色/荣耀红色/荣耀营销色/荣耀黑色/荣耀绿色/荣耀橙色"
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
                TestItem = "BES_ShippingMode",
                Remark = "BES系列耳机进入ShippingMode"
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
                TestItem = "BES_BurnIN",
                Remark = "BES老化测试"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_ReadBurnIN",
                Remark = "BES读取老化测试数据"
            });

            list.Add(new InitTestItem()
            {
                TestItem = "BES_Read_NormalBurnIN_HALL",
                Remark = "BES读取老化正常情况下HALL测试数据"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Read_NormalBurnIN_Power",
                Remark = "BES读取老化正常情况下电量计测试数据"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Read_NormalBurnIN_Charge",
                Remark = "BES读取老化正常情况下充电测试数据"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Read_MaxPowerBurnIN_HALL",
                Remark = "BES读取老化最大功率情况下HALL测试数据"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Read_MaxPowerBurnIN_Power",
                Remark = "BES读取老化最大功率情况下电量计测试数据"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Read_MaxPowerBurnIN_Charge",
                Remark = "BES读取老化最大功率情况下充电测试数据"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Read_SleepBurnIN_HALL",
                Remark = "BES读取老化休眠情况下HALL测试数据"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Read_SleepBurnIN_Power",
                Remark = "BES读取老化休眠情况下电量计测试数据"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Read_SleepBurnIN_Charge",
                Remark = "BES读取老化休眠情况下充电测试数据"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Read_BurnIN_Memory",
                Remark = "BES读取老化读取写入内存数据"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Read_BurnIN_LowBattery",
                Remark = "BES读取老化低电压关机数据"
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
            list.Add(new InitTestItem()
            {
                TestItem = "BES_ReadPackSN",
                Remark = "读取并比对扫描枪输入的20位包装整机SN"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_WritePackSN",
                Remark = "写入扫描枪输入的20位包装整机SN"
            });
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
                "和荣耀灰色/荣耀蓝色/荣耀红色/荣耀营销色/荣耀黑色/荣耀绿色/荣耀橙色"
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
            //BES_WriteHWVersion_ASCII
            list.Add(new InitTestItem()
            {
                TestItem = "BES_WriteHWVersion_ASCII",
                Remark = "写入BES系列耳机蓝牙硬件版本_ASCII"
            });

            list.Add(new InitTestItem()
            {
                TestItem = "BES_Semi_Product_CheckMacAddress",
                Remark = "半成品工站检查蓝牙地址是否重复"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Semi_Product_CheckProductSN",
                Remark = "半成品工站检查产品SN是否重复"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Pack_CheckMacAddress",
                Remark = "包装段检查蓝牙地址是否重复"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Pack_CheckProductSN",
                Remark = "包装段检查产品SN是否重复"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Pack_CheckPackSN",
                Remark = "包装段检查包装20位SN是否重复"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Semi_Product_CheckBatterySN",
                Remark = "电池信息写入工站检查电池SN是否重复"
            });

            list.Add(new InitTestItem()
            {
                TestItem = "BES_Assy_CRCBatterySN",
                Remark = "组装段CRC工站检查电池SN是否重复"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Pack_CRCBatterySN",
                Remark = "包装段CRC工站检查电池SN是否重复"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Assy_CRCCheckMacAddress",
                Remark = "组装段CRC检查蓝牙地址是否重复"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Assy_CRCCheckProductSN",
                Remark = "组装段CRC检查蓝牙地址是否重复"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_ControlMic1",
                Remark = "控制NTG主板主Mic打开"
            });
          
            list.Add(new InitTestItem()
            {
                TestItem = "BES_ControlMic2",
                Remark = "控制NTG主板副Mic1打开"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_ControlMic3",
                Remark = "控制NTG主板副Mic2打开"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_ControlMicAllOpen",
                Remark = "控制NTG主板Mic全部打开"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_ExitCDC",
                Remark = "永久退出CDC模式，适用于包装前一工站"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Enter_UsbAudio",
                Remark = "仅退出一次 CDC，可重复进入"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_ReadTouchData",
                Remark = "读取触摸板实时触摸时的值"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_ReadWearData",
                Remark = "读取佩戴触发值，要在远近距离测试各一次"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_BD02CompareFirstPairRecord",
                Remark = "BD02项目读取第一个蓝牙地址"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_BD02CompareSecondPairRecord",
                Remark = "BD02项目读取第二个蓝牙地址"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_BD02ComparePairRecord",
                Remark = "BD02项目读取二个蓝牙地址进行比对，要先读取两个蓝牙地址后再进行比对"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_BD02LeftCompareSoftVersion",
                Remark = "BD02项目读取左耳并比对软件版本"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_BD02RightCompareSoftVersion",
                Remark = "BD02项目读取右耳并比对软件版本"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_BD02ReadLeftPairMessage",
                Remark = "BD02项目读取左耳组队信息"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_BD02ReadRightPairMessage",
                Remark = "BD02项目读取右耳组队信息"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_BD02LeftRightCompare",
                Remark = "BD02项目左右耳组队信息比对"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_T1207ReadMac",
                Remark = "T1207项目读取蓝牙地址"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_T1207SoftVersion",
                Remark = "T1207项目读取软件版本"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_T1207EnterDUT",
                Remark = "T1207项目进入DUT模式"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_T1207Reset",
                Remark = "T1207项目写入蓝牙地址后重启"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_T1207WriteBtAddress",
                Remark = "T1207项目写蓝牙地址，需要扫描输入"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_T1207ClsPair",
                Remark = "T1207项目清除耳机和手机的配对记录"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_T1207ReadLeftPairMessage",
                Remark = "T1207项目读取左耳TWS组队信息"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_T1207ReadRightPairMessage",
                Remark = "T1207项目读取右耳TWS组队信息"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_T1207LeftRightCompare",
                Remark = "T1207项目比对左右耳TWS组队信息"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_T1207ReadLeftElectricity",
                Remark = "T1207项目读取左耳电量"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_T1207ReadRightElectricity",
                Remark = "T1207项目读取右耳电量"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_TW18LeftRightCompare",
                Remark = "TW18项目比对左右耳TWS组队信息"
            });

            list.Add(new InitTestItem()
            {
                TestItem = "BES_JT51ReadLeftPairMessage",
                Remark = "JT51项目读取左耳组队信息"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_JT51ReadRightPairMessage",
                Remark = "JT51项目读取右耳组队信息"
            });

            list.Add(new InitTestItem()
            {
                TestItem = "BES_TWS_TXMode",
                Remark = "BES TWS耳机进入TX模式"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_TWS_DUTMode",
                Remark = "BES TWS耳机进入DUT模式"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_TWS_ReadVersion",
                Remark = "BES TWS耳机读取软件版本并比对版本"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_TWS_ReadMACAddress",
                Remark = "BES TWS耳机读取耳机蓝牙地址"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_TWS_MainMic",
                Remark = "BES TWS耳机切换为主Mic"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_TWS_FFMic",
                Remark = "BES TWS耳机FF Mic"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_TWS_ClearAll",
                Remark = "BES TWS耳机清除所有配对记录"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_TWS_AudioLoop",
                Remark = "BES TWS耳机进入AudioLoop模式"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_TWS_ComparePairName",
                Remark = "BES TWS耳机比对配对名称"
            });
           
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Lchse_TWS_EnterDUT",
                Remark = "BES  Lchse TWS耳机进入DUT模式"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Lchse_TWS_ReadSoftVersion",
                Remark = "BES  Lchse TWS耳机读取软件版本"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Lchse_TWS_ReadHWVersion",
                Remark = "BES Lchse TWS耳机读取硬件版本"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Lchse_TWS_ReadVoltage",
                Remark = "BES Lchse TWS耳机读取电池电压"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Lchse_TWS_ReadElectricity",
                Remark = "BES Lchse TWS耳机读取电池电量"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Lchse_TWS_ReadBTAddress",
                Remark = "BES Lchse TWS耳机读取蓝牙地址"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Lchse_TWS_WriteBTAddress",
                Remark = "BES Lchse TWS耳机写蓝牙地址"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Lchse_TWS_ReadBTName",
                Remark = "BES Lchse TWS耳机读取蓝牙名称"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Lchse_TWS_WriteBTName",
                Remark = "BES Lchse TWS耳机写入蓝牙名称，在设置下限参数时设置蓝牙名称"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Lchse_TWS_ShutDown",
                Remark = "BES Lchse TWS耳机关机"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Lchse_TWS_Reset",
                Remark = "BES Lchse TWS耳机复位"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Lchse_TWS_CloseLog",
                Remark = "BES Lchse TWS关闭Log功能"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Lchse_TWS_04cCharge",
                Remark = "BES Lchse TWS 0.4c充电"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Lchse_TWS_1cCharge",
                Remark = "BES Lchse TWS 1c充电"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Lchse_TWS_TalkMic",
                Remark = "BES Lchse TWS 打开通话Mic，测试LoopBack"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Lchse_TWS_FFMIC",
                Remark = "BES Lchse TWS 打开通话FFMic，测试LoopBack"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_Lchse_TWS_FBMIC",
                Remark = "BES Lchse TWS 打开通话FBMic，测试LoopBack"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_XE25_TWS_EnterDUT",
                Remark = "BES XE25 TWS 进入DUT模式"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_XE25_TWS_CloseLog",
                Remark = "BES XE25 TWS 关闭Log打印"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_XE25_TWS_ReadVersion",
                Remark = "BES XE25 TWS 读取版本"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_XE25_TWS_ReadMac",
                Remark = "BES XE25 TWS 读取Mac地址"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_XE25_TWS_ReadName",
                Remark = "BES XE25 TWS 读取名称"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_XE25_TWS_ClsRepair",
                Remark = "BES XE25 TWS 清除配对记录"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_XE25_TWS_LoopBackOn",
                Remark = "BES XE25 TWS 进入LoopBack模式"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_XE25_TWS_LoopBackOff",
                Remark = "BES XE25 TWS 关闭LoopBack模式"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_OppoReadSoftVersion",
                Remark = "BES 读取OPPO耳机软件版本"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_OppoReadHardVersion",
                Remark = "BES 读取OPPO耳机硬件版本"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_OppoReadElectricity",
                Remark = "BES 读取OPPO耳机电量"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_OppoReadButton",
                Remark = "BES 读取OPPO耳机按键，01为暂停，02为音量+，04为音量-"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_OppoHALLTest",
                Remark = "BES OPPO耳机霍尔测试"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_OppoReadNTC",
                Remark = "BES 读取OPPO耳机NTC值，返回值2：0°-17°，" +
                "19：17°-45°，47为45-53度， 55：53°-70°，55：53°-70°，71：>70°"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_OppoReadVoltage",
                Remark = "BES 读取OPPO耳机电压，包装线要等待10秒以上才准确"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_OppoEnterDUT",
                Remark = "BES OPPO耳机进入DUT"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_OppoReset",
                Remark = "BES OPPO耳机复位"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_OppoPowerOff",
                Remark = "BES OPPO耳机关机"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_OppoFactorySettings",
                Remark = "BES OPPO耳机出厂设置，清除配对记录"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_OppoChargeManage",
                Remark = "BES 设置OPPO耳机充电模式，00为退出，01为进入"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_OppoReadChargeManage",
                Remark = "BES 读取OPPO耳机充电模式，00为退出，01为进入"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_OppoShipping",
                Remark = "BES OPPO耳机进入船运模式"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_OppoReadBtName",
                Remark = "BES 读取OPPO耳机名称并进行判断"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_OppoReadBtAddress",
                Remark = "BES 读取OPPO耳机蓝牙地址"
            });
           
            list.Add(new InitTestItem()
            {
                TestItem = "BES_OppoReadBattarySN",
                Remark = "BES 读取OPPO耳机电池SN"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_OppoCompareBattarySN",
                Remark = "BES 读取并比对OPPO耳机电池SN"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_OppoWriteBattarySN",
                Remark = "BES 写入OPPO耳机电池SN"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_OppoReadTelecomSN",
                Remark = "BES 读取OPPO耳机电信码"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_OppoCompareTelecomSN",
                Remark = "BES 读取并比对OPPO耳机电信码"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_OppoWriteTelecomSN",
                Remark = "BES 写入OPPO耳机电信码"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_OppoReadEQ",
                Remark = "BES 读取OPPO耳机EQ，01：国内  02：国外  其他值默认国内，中英文提示音切换"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_OppoWriteEQ",
                Remark = "BES 写入OPPO耳机EQ，01：国内  02：国外  其他值默认国内，中英文提示音切换"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_OppoReadENC",
                Remark = "BES 读取OPPO耳机ENC bypass，0为ENC有效，1为无效，可以测试Mic"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_OppoWriteENC",
                Remark = "BES 写入OPPO耳机ENC bypass，0为ENC有效，1为无效，可以测试Mic"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_OppoClsElectricity",
                Remark = "BES 清除OPPO耳机电量"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_OppoReadColor",
                Remark = "BES 读取OPPO耳机颜色"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_OppoWriteColor",
                Remark = "BES 写入OPPO耳机颜色"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "BES_OppoReadEncLicense",
                Remark = "BES 读取OPPO耳机校验ENC 许可证，0为未进行校验，1为ENC校验成功"
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
                TestItem = "OpenMicroChipPort",
                Remark = "打开MicroChip串口"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "MicroChipReadBTAddress",
                Remark = "MicroChip读取蓝牙地址"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "MicroChipReadNTC",
                Remark = "MicroChip读取NTC值"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "MicroChipReadVoltage",
                Remark = "MicroChip读取电池电压"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "MicroChipReadTone",
                Remark = "MicroChip读取提示音，在下限中输入，如:中文;英文"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "MicroChipReadSoftVersion",
                Remark = "MicroChip读取软件版本"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "MicroChipEntenDUT",
                Remark = "MicroChip进入DUT模式，测试RF"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "ClosedMicroChipPort",
                Remark = "关闭MicroChip串口"
            });

            list.Add(new InitTestItem()
            {
                TestItem = "OpenA2",
                Remark = "打开与配置A2"
            });
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
                Remark = "A2和BES芯片耳机配对"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "CSR_EarPair",
                Remark = "A2和高通耳机通过搜索进行配对,需要扫蓝牙地址才可以测试"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "OppoEarPair",
                Remark = "A2和BES芯片OPPO耳机配对"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "ScanPair",
                Remark = "A2搜索进行配对,需要输入耳机名称"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "XE25_EarPair",
                Remark = "XE25项目读取蓝牙地址配对"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "SpeakerLevel_Left",
                Remark = "测试左SPK电平，单位：Vrmas"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "SpeakerLevel_Right",
                Remark = "测试右SPK电平，单位：Vrmas"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "SpeakerTHD_Left",
                Remark = "测试左SPK THD+N Radio，单位：%"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "SpeakerTHD_Right",
                Remark = "测试右SPK THD+N Radio，单位：%"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "SpeakerSNR_Left",
                Remark = "测试左SPK信噪比，单位dB"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "SpeakerSNR_Right",
                Remark = "测试右SPK信噪比，单位dB"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "SpeakerNoise_Left",
                Remark = "测试左SPK低噪音电平，单位：Vrms"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "SpeakerNoise_Right",
                Remark = "测试右SPK低噪音电平，单位：Vrms"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "SpeakerCrosstalk_Left",
                Remark = "测试左SPK Crosstalk，单位dB"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "SpeakerCrosstalk_Right",
                Remark = "测试右SPK Crosstalk，单位dB"
            });

            list.Add(new InitTestItem()
            {
                TestItem = "SpeakerDynamicRange_Left",
                Remark = "测试左SPK 动态范围，单位dB"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "SpeakerDynamicRange_Right",
                Remark = "测试右SPK 动态范围，单位dB"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "MicphoneLevel",
                Remark = "测试Mic电平，单位是Vrms"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "MicphoneTHD",
                Remark = "测试Mic THD+N Radio，单位是%"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "MicphoneSNR",
                Remark = "测试Mic 信噪比，单位是dB"
            });

            list.Add(new InitTestItem()
            {
                TestItem = "SetMaxVolume",
                Remark = "设置待测试设备的喇叭最大音量"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "DisConnect",
                Remark = "断开A2与耳机的连接"
            });

            list.Add(new InitTestItem()
            {
                TestItem = "OpenLedPort",
                Remark = "打开LED测试仪串口"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "LED1Test",
                Remark = "测试LED1，上下限参数为：R,G,B,亮度"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "ClosedLEDPort",
                Remark = "关闭LED测试仪串口"
            });

            list.Add(new InitTestItem()
            {
                TestItem = "OpenRelay",
                Remark = "打开继电器串口"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "OpenChannel",
                Remark = "断开某一通道，通道在下限设置"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "ClosedChannel",
                Remark = "吸合某一通道，通道在下限设置"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "OpenAllChannel",
                Remark = "打开所有通道"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "ClosedAllChannel",
                Remark = "吸合所有通道"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "ClosedRelay",
                Remark = "关闭继电器串口"
            });
            //list.Add(new InitTestItem()
            //{
            //    TestItem = "TouchTest",
            //    Remark = "测试NTG触摸板的补偿值"
            //});
            list.Add(new InitTestItem()
            {
                TestItem = "OpenAirohaPort",
                Remark = "打开络达串口"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "AirohaClosedLog",
                Remark = "关闭络达Log"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "AirohaReadBtAddress",
                Remark = "读取络达蓝牙地址"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "AirohaReadBtName",
                Remark = "读取络达蓝牙名称"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "AirohaReadSoftVersion",
                Remark = "读取络达软件版本"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "AirohaEnterDUT",
                Remark = "络达进入DUT和配对模式"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "AirohaPowerOff",
                Remark = "络达关机"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "AirohaPowerReset",
                Remark = "络达重启"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "AirohaMainMic",
                Remark = "络达打开主MIC"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "AirohaFFMic",
                Remark = "络达打开FF MIC"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "AirohaLightOut",
                Remark = "络达光感出耳校准"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "AirohaLightInput",
                Remark = "络达光感入耳校准"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "AirohaLightValue",
                Remark = "络达光感值"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "AirohaClosePort",
                Remark = "关闭络达串口"
            });

            list.Add(new InitTestItem()
            {
                TestItem = "OpenXlink",
                Remark = "打开中科蓝汛Xlink端口"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "XlinkShutDown",
                Remark = "关闭中科蓝汛Xlink端口"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "XlinkReadBtAddress",
                Remark = "读取中科蓝汛蓝牙地址"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "XlinkReadSoftVersion",
                Remark = "读取中科蓝汛软件版本"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "XlinkEnterDUT",
                Remark = "中科蓝汛进入DUT模式"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "XlinkClsPair",
                Remark = "中科蓝汛清除配对记录"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "XlinkLoopBackON",
                Remark = "中科蓝汛打开LoopBack"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "XlinkLoopBackOFF",
                Remark = "中科蓝汛关闭LoopBack"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "ClosedXlink",
                Remark = "关闭中科蓝汛串口"
            });

            list.Add(new InitTestItem()
            {
                TestItem = "CRY574Initialize",
                Remark = "初始化CRY574 Dongle"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "CRY574Release",
                Remark = "释放CRY574资源"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "CRY574ScanAndConnectMac",
                Remark = "搜索蓝牙地址和连接蓝牙及SPP"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "CRY574GetSppData",
                Remark = "通用串口指令(byte数据类型)，指令;跳过几个byte;第几位byte是数据长度,偏移位数;编码方式：ASCII/16/10/bt;是否有返回;返回值识别数组" +
                "例如：55 AA FF 17 00 A5;5;3,2;16;true;05 5b"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "CRY574GetSppAnyData",
                Remark = "通用串口指令(byte数据类型)，指令;跳过几个byte;第几位byte是数据长度,偏移位数;编码方式：ASCII/16/10/bt;是否有返回;返回值识别数组" +
               "例如：55 AA FF 17 00 A5;5;3,2;16;true;05 5b"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "CRY574GetSppATData",
                Remark = "通用串口指令(AT指令)，指令;分割符合;获取数据位数;返回值识别数组" +
              "例如：at+name;:;1;4f 4b"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "PDT40FarDiff",
                Remark = "比对远空光感值与空域光感值的差"
            });
            list.Add(new InitTestItem()
            {
                TestItem = "PDT40NearDiff",
                Remark = "比对近空光感值与空域光感值的差"
            });
            return list;
        }
    }
}
