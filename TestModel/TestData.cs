using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestModel
{
    public class TestData
    {
        public string ID;
        public string TestItem;
        public string TestItemName;
        public string UppLimit;
        public string LowLimit;
        public string Unit;
        public string Value;
        public string Result;
        public int beferTime;
        public int AfterTime;
        public string Remark;
        public string Other;
        public bool Check;
        public bool Show;
        public string FillValue;
    }

    public class ConfigData
    {
        public string Title;
        public string VisaPort;
        public string SerialPort;
        public string Sen_TX_Power;
        public string number_of_packets;
        public string Low_Freq;
        public string Mod_Freq;
        public string Hi_Freq;
        public string Low_Loss;
        public string Mod_Loss;
        public string Hi_Loss;
        public string Inquiry_TimeOut;
        public string PeakPower;
        public string AvgPowerLow;
        public string AvgPowerHi;
        public bool OP;
        public bool MI;
        public bool SS;
        public bool CD;
        public bool IC;
        public string PowerPort;
        public string Voltage1;
        public string Voltage2;
        public string Current;
        public bool GPIB_Enable;
        public bool Serial_Enable;
        public bool Power_Enable;
        public string CompareString;
        public int SNLength;
        public string MultimeterPort;
        public bool Multimeter_Select;
        public bool AudioEnable;
        public string AudioPath;
        public bool SerialSelect;
        public bool AutoSNTest;
        public string SNHear;
        public string SNLine;
        public bool AutoFixture;
        public string FixturePort;
        public bool LEDEnable;
        public string LEDPort;
        public bool PlugEnable;
        public string MaxSet;
        public string PlugNumber;
        public string _4010Port;
        public bool _4010Enable;
        public bool MesEnable;
        public string MesStation;
        public bool AutoHALL;
        public string RelayPort;
        public bool RelayEnable;
        public string NowStation;
        public bool AutoSN;
    }

    public class TestRatio
    {
        public double Total;
        public double Pass;
        public double PassRadio;
    }

    public class LogColume
    {
       public string SN;
        public string TestTime;
        public string MAC;
        public string TotalStatus;
    }

    public class LogContent
    {
        public string isPass;
        public string data;
        public string unit;
        public string lowerLimit;
        public string upperLimit;
    }

    public class InitTestItem
    {
        public string TestItem;
        public string Remark;
    }

}
