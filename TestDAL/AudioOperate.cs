using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ATCAPI;
using TestModel;
using System.Threading;
using System.IO;
using TestTool;

namespace TestDAL
{
    public class AudioOperate
    {
        private ATC ATc;
        private ConfigData config;
        private double[] SpkLevels, SpkTHD, SpkSNR, SpkCrossralk, LowNoise, DynamicRange;
        private OperateBES Operate;
        public string btAddress;
        private bool btStatus;
        private Queue<string> queue;

        public AudioOperate(ConfigData config, OperateBES operate,Queue<string> queue)
        {
            try
            {
                this.config = config;
                this.queue = queue;
                btAddress = string.Empty;
                
              
                if (File.Exists(config.AudioPath))
                {
                    queue.Enqueue("加载A2项目文件");
                    ATc = new ATC();
                    ATc.Visible = false;
                    string name = ATc.ProjectFileName;
                    if ((!config.AudioPath.Contains(name)) || name == "")
                    {
                        ATc.OpenProject(config.AudioPath);
                    }
                    queue.Enqueue("加载A2项目文件完成");
                }
                else
                {
                    queue.Enqueue("A2项目文件路径设置错误，请检查");
                }
            
                this.Operate = operate;
                btStatus = true;
            }
            catch (Exception ex)
            {
                queue.Enqueue("ex;" + ex.Message);
            }
            
        }

        public TestData OpenA2(TestData data)
        {
            try
            {
                //ATc.BtsimSettings.CallCancel();
                OutputConnectorType type = ATc.SignalPathSetup.OutputConnector.Type;
                
                InputConnectorType ty = ATc.SignalPathSetup.InputConnector.Type;
                if (type == OutputConnectorType.Btsim || ty == InputConnectorType.Btsim)
                {
                    ATc.BtsimSettings.Reset();
                    data.Result = "Pass";
                    data.Value = "Pass";
                    btStatus = true;
                }
                else
                {
                    btStatus = false;

                    //ATc.SignalPathSetup.OutputConnector.Type = OutputConnectorType.Bluetooth;
                    //ATc.BluetoothSettings.ProfileSet = BluetoothProfileSet.A2dpSourceHfpGatewayAvrcp;

                    ATc.BluetoothSettings.InquiryTimeout = 5;
                    ATc.BluetoothSettings.ClearDeviceList();
                    data.Result = "Pass";
                    data.Value = "Pass";
                }
                //ATc.SignalPathSetup.OutputConnector.Type = OutputConnectorType.Btsim;
                //ATc.SignalPathSetup.InputConnector.Type = InputConnectorType.AnalogUnbalanced;
                //ATc.BtsimSettings.Reset();
                //string name = ATc.BtsimSettings.BtsimName;

                //ATc.BluetoothSettings.ClearDeviceList();
                //ATc.BluetoothSettings.ProfileSet = BluetoothProfileSet.A2dpSourceHfpGatewayAvrcp;
                //ATc.BluetoothSettings.InquiryTimeout = 5;
               
            }
            catch (Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            return data;
        }

        public TestData OpenLookBackA2(TestData data)
        {
            return data;
        }

        public TestData SwitchToA2dp(TestData data)
        {
            try
            {
                if (btStatus)
                {
                    //ATc.SignalPathSetup.OutputConnector.Type = OutputConnectorType.Btsim;
                    //ATc.SignalPathSetup.InputConnector.Type = InputConnectorType.AnalogBalanced;
                    ATc.BtsimSettings.SwitchToA2dp();

                    data.Value = "Pass";
                    data.Result = "Pass";
                }
                else
                {
                    ATc.BluetoothSettings.ConnectA2dp(btAddress);
                    if (ATc.BluetoothSettings.AcceptA2dpConnections)
                    {
                        data.Value = "Pass";
                        data.Result = "Pass";
                    }
                    else
                    {
                        data.Value = "Fail";
                        data.Result = "Fail";
                    }
                }
                //ATc.SignalPathSetup.OutputConnector.Type = OutputConnectorType.Bluetooth;
                //ATc.SignalPathSetup.InputConnector.Type = InputConnectorType.AnalogBalanced;

                //ATc.BluetoothSettings.ConnectA2dp(btAddress);

            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData SwitchToHfp(TestData data)
        {
            try
            {
                if (btStatus)
                {
                    //ATc.BtsimSettings.SwitchToHfp();
                    //ATc.SignalPathSetup.OutputConnector.Type = OutputConnectorType.AnalogUnbalanced;
                    //ATc.SignalPathSetup.InputConnector.Type = InputConnectorType.Btsim;
                    //Thread.Sleep(2000);
                    ATc.BtsimSettings.SwitchToHfp();
                }
                else
                {
                    //ATc.SignalPathSetup.InputConnector.Type = InputConnectorType.Bluetooth;
                    //ATc.SignalPathSetup.OutputConnector.Type = OutputConnectorType.AnalogBalanced;
                    ATc.BluetoothSettings.ConnectHfp(btAddress);
                    ATc.BluetoothSettings.HfpAudioGatewayCommand(BluetoothHfpAgCommand.OpenSco);
                }
                //ATc.SignalPathSetup.OutputConnector.Type = OutputConnectorType.AnalogUnbalanced;
                //ATc.SignalPathSetup.InputConnector.Type = InputConnectorType.Bluetooth;
                //ATc.BluetoothSettings.ConnectHfp(btAddress);
                //ATc.BluetoothSettings.HfpAudioGatewayCommand(BluetoothHfpAgCommand.OpenSco);
                data.Value = "Pass";
                data.Result = "Pass";
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData EarPair(TestData data)
        {
            btAddress = Operate.BES_ReadBTAddress();
            for (int i = 0; i < 3; i++)
            {
                //ATc.BtsimSettings.Reset();

                if (btStatus)
                {
                    try
                    {
                        ATc.BtsimSettings.Reset();
                        ATc.BtsimSettings.ConnectToDevice(btAddress, "1", 10);
                        if (ATc.BtsimSettings.AppState == BtsimAppState.Connected)
                        //if(conn)
                        {
                            data.Result = "Pass";
                            data.Value = "Pass";
                            break;
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = "Fail";
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                    }
                }
                else
                {
                    try
                    {
                        bool conn = ATc.BluetoothSettings.PairWithDevice(btAddress);
                        if (conn)
                        {
                            data.Result = "Pass";
                            data.Value = "Pass";
                            break;
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = "Fail";
                        }

                    }
                    //bool conn = ATc.BluetoothSettings.PairWithDevice(btAddress);
                    catch (Exception ex)
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                    }
                }
            }
            return data;
        }

        public TestData OppoEarPair(TestData data)
        {
            btAddress = Operate.BES_OppoReadBtAddress();
            for (int i = 0; i < 3; i++)
            {
                //ATc.BtsimSettings.Reset();

                if (btStatus)
                {
                    try
                    {
                        ATc.BtsimSettings.Reset();
                        ATc.BtsimSettings.ConnectToDevice(btAddress, "1", 10);
                        if (ATc.BtsimSettings.AppState == BtsimAppState.Connected)
                        //if(conn)
                        {
                            data.Result = "Pass";
                            data.Value = "Pass";
                            break;
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = "Fail";
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                    }
                }
                else
                {
                    try
                    {
                        bool conn = ATc.BluetoothSettings.PairWithDevice(btAddress);
                        if (conn)
                        {
                            data.Result = "Pass";
                            data.Value = "Pass";
                            break;
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = "Fail";
                        }

                    }
                    //bool conn = ATc.BluetoothSettings.PairWithDevice(btAddress);
                    catch (Exception ex)
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                    }
                }
            }
            return data;
        }

        public TestData XE25_EarPair(TestData data)
        {
            btAddress = Operate.BES_XE25_TWS_ReadMac();
            for (int i = 0; i < 3; i++)
            {
                //ATc.BtsimSettings.Reset();

                if (btStatus)
                {
                    try
                    {
                        ATc.BtsimSettings.Reset();
                        ATc.BtsimSettings.ConnectToDevice(btAddress, "1", 10);
                        if (ATc.BtsimSettings.AppState == BtsimAppState.Connected)
                        //if(conn)
                        {
                            data.Result = "Pass";
                            data.Value = "Pass";
                            break;
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = "Fail";
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                    }
                }
                else
                {
                    try
                    {
                        bool conn = ATc.BluetoothSettings.PairWithDevice(btAddress);
                        if (conn)
                        {
                            data.Result = "Pass";
                            data.Value = "Pass";
                            break;
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = "Fail";
                        }

                    }
                    //bool conn = ATc.BluetoothSettings.PairWithDevice(btAddress);
                    catch (Exception ex)
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                    }
                }
            }
            return data;
        }

        public TestData CSR_EarPair(TestData data)
        {

            for (int i = 0; i < 3; i++)
            {
                //ATc.BtsimSettings.Reset();
                //btAddress = Operate.BES_ReadBTAddress();
                if (btStatus)
                {
                    //IBtsimDeviceCollection lists = ATc.BtsimSettings.ScanForDevices(5);

                    //Dictionary<string, double> dic = new Dictionary<string, double>();
                    //for (int i = 0; i < lists.Count; i++)
                    //{
                    //    dic.Add(lists[i].Address, double.Parse(lists[i].Rssi));
                    //}
                    //dic
                    try
                    {
                        //ATc.BtsimSettings.Reset();
                        ATc.BtsimSettings.ConnectToDevice(btAddress, "1", 10);
                        if (ATc.BtsimSettings.AppState == BtsimAppState.Connected)
                        //if(conn)
                        {
                            data.Result = "Pass";
                            data.Value = "Pass";
                            break;
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = "Fail";
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                        break;
                    }
                }
                else
                {
                    //IBluetoothDeviceCollection lists = ATc.BluetoothSettings.ScanForDevices(5);
                    //Dictionary<string, double> dic = new Dictionary<string, double>();
                    //for (int i = 0; i < lists.Count; i++)
                    //{
                    //    dic.Add(lists[i].Address, double.Parse(lists[i].Rssi));
                    //}
                    try
                    {
                        bool conn = ATc.BluetoothSettings.PairWithDevice(btAddress);
                        if (conn)
                        {
                            data.Result = "Pass";
                            data.Value = "Pass";
                            break;
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = "Fail";
                        }

                    }
                    catch (Exception ex)
                    {
                        data.Result = "Fail";
                        data.Value = "Fail";
                    }
                    //bool conn = ATc.BluetoothSettings.PairWithDevice(btAddress);
                }
            }

            return data;
        }

        public TestData ScanPair(TestData data)
        {
            try
            {
                if (btStatus)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        var lists = ATc.BtsimSettings.ScanForDevices(5);
                        for (int i = 0; i < lists.Count; i++)
                        {
                            string name = lists[i].FriendlyName;
                            string btaddress = lists[i].Address;
                            if (name.Contains(data.LowLimit))
                            {
                                ATc.BtsimSettings.ConnectToDevice(btaddress, "1", 10);
                                if (ATc.BtsimSettings.AppState == BtsimAppState.Connected)
                                {
                                    data.Result = "Pass";
                                    data.Value = "Pass";
                                    break;
                                }
                            }
                        }
                    }
                }

                else
                {
                    ATc.BluetoothSettings.ClearDeviceList();
                    var lists = ATc.BluetoothSettings.ScanForDevices(5);
                    for (int i = 0; i < lists.Count; i++)
                    {
                        string name = lists[i].FriendlyName;
                        string address = lists[i].Address;
                        if (name.Contains(data.LowLimit))
                        {
                            bool status = ATc.BluetoothSettings.PairWithDevice(address);
                            if (status)
                            {
                                data.Value = "Pass";
                                data.Result = "Pass";
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {
                data.Result = "Fail";
                data.Value = "Fail";

            }
            return data;
        }

        public TestData SpeakerLevel_Left(TestData data)
        {
            try
            {
                //Operate.BES_SetVolume(data);
              
                for (int i = 0; i < 3; i++)
                {
                    //Operate.SetVolume();
                    ATc.Sequence["Speaker"]["Level and Gain"].Run();
                    //ATc.LevelAndGain.Level.Axis.Unit = data.Unit;
                    if (ATc.Sequence["Speaker"]["Level and Gain"].HasSequenceResults)
                    {
                        ISequenceResultCollection results = ATc.Sequence["Speaker"]["Level and Gain"]
                            .SequenceResults;

                        SpkLevels = results[0].GetMeterValues();

                        if (SpkLevels[0] + double.Parse(data.FillValue)
                            <= double.Parse(data.UppLimit)
                            && SpkLevels[0] + double.Parse(data.FillValue)
                            >= double.Parse(data.LowLimit))
                        {
                            data.Result = "Pass";
                            data.Value = Math.Round(SpkLevels[0] 
                                + double.Parse(data.FillValue), 3).ToString();
                            break;
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = Math.Round(SpkLevels[0] 
                                + double.Parse(data.FillValue), 3).ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData SpeakerLevel_Right(TestData data)
        {
            try
            {
                if (SpkLevels[1] + double.Parse(data.FillValue)
                    <= double.Parse(data.UppLimit)
                        && SpkLevels[1] + double.Parse(data.FillValue) 
                        >= double.Parse(data.LowLimit))
                {
                    data.Result = "Pass";
                    data.Value = Math.Round(SpkLevels[1] 
                        + double.Parse(data.FillValue), 3).ToString();
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = Math.Round(SpkLevels[1], 3).ToString();
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData SpeakerTHD_Left(TestData data)
        {
            try
            {
                
                for (int i = 0; i < 3; i++)
                {
                    ATc.Sequence["Speaker"]["THD+N"].Run();
                    //ATc.ThdN.ThdRatio.Axis.Unit = data.Unit;
                    
                    if (ATc.Sequence["Speaker"]["THD+N"].HasSequenceResults)
                    {
                        ISequenceResultCollection results = ATc.Sequence["Speaker"]["THD+N"]
                            .SequenceResults;
                        SpkTHD = results.Count > 2 
                            ? results[2].GetMeterValues() : results[0].GetMeterValues();
                        if (SpkTHD[0] + double.Parse(data.FillValue)
                            <= double.Parse(data.UppLimit)
                             && SpkTHD[0] + double.Parse(data.FillValue) 
                             >= double.Parse(data.LowLimit))
                        {
                            data.Result = "Pass";
                            data.Value = Math.Round(SpkTHD[0] 
                                + double.Parse(data.FillValue), 3).ToString();
                            break;
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = Math.Round(SpkTHD[0] 
                                + double.Parse(data.FillValue), 3).ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData SpeakerTHD_Right(TestData data)
        {
            try
            {
                if (SpkTHD[1] + double.Parse(data.FillValue)
                    <= double.Parse(data.UppLimit)
                        && SpkTHD[1] + double.Parse(data.FillValue)
                        >= double.Parse(data.LowLimit))
                {
                    data.Result = "Pass";
                    data.Value = Math.Round(SpkTHD[1]
                         + double.Parse(data.FillValue), 3).ToString();
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = Math.Round(SpkTHD[1]
                         + double.Parse(data.FillValue), 3).ToString();
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData SpeakerSNR_Left(TestData data)
        {
            try
            {
                //ATc.SignalToNoiseRatio.SignalToNoiseRatio.Axis.Unit = data.Unit;
                for (int i = 0; i < 3; i++)
                {
                    ATc.Sequence["Speaker"]["Signal to Noise Ratio"].Run();
                    //ATc.SignalToNoiseRatio.SignalToNoiseRatio.Axis.Unit = data.Unit;
                    if (ATc.Sequence["Speaker"]["Signal to Noise Ratio"].HasSequenceResults)
                    {
                        ISequenceResultCollection results = ATc.Sequence["Speaker"]["Signal to Noise Ratio"]
                            .SequenceResults;
                        SpkSNR = results[0].GetMeterValues();
                        if (SpkSNR[0] + double.Parse(data.FillValue)
                            <= double.Parse(data.UppLimit)
                             && SpkSNR[0] + double.Parse(data.FillValue)
                             >= double.Parse(data.LowLimit))
                        {
                            data.Result = "Pass";
                            data.Value = Math.Round(SpkSNR[0]
                                 + double.Parse(data.FillValue), 3).ToString();
                            break;
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = Math.Round(SpkSNR[0]
                                 + double.Parse(data.FillValue), 3).ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData SpeakerSNR_Right(TestData data)
        {
            try
            {
                if (SpkSNR[1] + double.Parse(data.FillValue)
                    <= double.Parse(data.UppLimit)
                        && SpkSNR[1] + double.Parse(data.FillValue)
                        >= double.Parse(data.LowLimit))
                {
                    data.Result = "Pass";
                    data.Value = Math.Round(SpkSNR[1]
                         + double.Parse(data.FillValue), 3).ToString();
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = Math.Round(SpkSNR[1]
                         + double.Parse(data.FillValue), 3).ToString();
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData SpeakerNoise_Left(TestData data)
        {
            try
            {
               
                for (int i = 0; i < 3; i++)
                {
                    ATc.Sequence["Speaker"]["THD+N"].Run();
                    if (ATc.Sequence["Speaker"]["THD+N"].HasSequenceResults)
                    {
                        ISequenceResultCollection results = ATc.Sequence["Speaker"]["THD+N"]
                            .SequenceResults;
                        LowNoise = results[7].GetMeterValues();
                        if (LowNoise[0] + double.Parse(data.FillValue)
                            <= double.Parse(data.UppLimit)
                             && LowNoise[0] + double.Parse(data.FillValue)
                             >= double.Parse(data.LowLimit))
                        {
                            data.Result = "Pass";
                            data.Value = Math.Round(LowNoise[0] 
                                + double.Parse(data.FillValue), 3).ToString();
                            break;
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = Math.Round(LowNoise[0]
                                 + double.Parse(data.FillValue), 3).ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData SpeakerNoise_Right(TestData data)
        {
            try
            {
                if (LowNoise[1] + double.Parse(data.FillValue)
                    <= double.Parse(data.UppLimit)
                        && LowNoise[1] + double.Parse(data.FillValue)
                        >= double.Parse(data.LowLimit))
                {
                    data.Result = "Pass";
                    data.Value = Math.Round(LowNoise[1] 
                        + double.Parse(data.FillValue), 3).ToString();
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = Math.Round(LowNoise[1]
                         + double.Parse(data.FillValue), 3).ToString();
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData SpeakerCrosstalk_Left(TestData data)
        {
            try
            {
               
                for (int i = 0; i < 3; i++)
                {
                    ATc.Sequence["Speaker"]["Crosstalk, One Channel Undriven"].Run();
                    //ATc.CrosstalkOneChannelUndriven.Crosstalk.Axis.Unit = data.Unit;
                    if (ATc.Sequence["Speaker"]["Crosstalk, One Channel Undriven"].HasSequenceResults)
                    {
                        ISequenceResultCollection results = ATc.Sequence["Speaker"]
                            ["Crosstalk, One Channel Undriven"].SequenceResults;
                        SpkCrossralk = results[0].GetMeterValues();
                        if (SpkCrossralk[0] + double.Parse(data.FillValue)
                            <= double.Parse(data.UppLimit)
                             && SpkCrossralk[0] + double.Parse(data.FillValue)
                             >= double.Parse(data.LowLimit))
                        {
                            data.Result = "Pass";
                            data.Value = Math.Round(SpkCrossralk[0]
                                + double.Parse(data.FillValue), 3).ToString();
                            break;
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = Math.Round(SpkCrossralk[0]
                                 + double.Parse(data.FillValue), 3).ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData SpeakerCrosstalk_Right(TestData data)
        {
            try
            {
                if (SpkCrossralk[1] + double.Parse(data.FillValue)
                    <= double.Parse(data.UppLimit)
                        && SpkCrossralk[1] + double.Parse(data.FillValue)
                        >= double.Parse(data.LowLimit))
                {
                    data.Result = "Pass";
                    data.Value = Math.Round(SpkCrossralk[1]
                         + double.Parse(data.FillValue), 3).ToString();
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = Math.Round(SpkCrossralk[1]
                         + double.Parse(data.FillValue), 3).ToString();
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData SpeakerDynamicRange_Left(TestData data)
        {
            //Dynamic Range - AES17
            try
            {

                for (int i = 0; i < 3; i++)
                {
                    ATc.Sequence["Speaker"]["Dynamic Range - AES17"].Run();
                    //ATc.CrosstalkOneChannelUndriven.Crosstalk.Axis.Unit = data.Unit;
                    if (ATc.Sequence["Speaker"]["Dynamic Range - AES17"].HasSequenceResults)
                    {
                        ISequenceResultCollection results = ATc.Sequence["Speaker"]
                            ["Dynamic Range - AES17"].SequenceResults;
                        DynamicRange = results[0].GetMeterValues();
                        if (DynamicRange[0] + double.Parse(data.FillValue)
                            <= double.Parse(data.UppLimit)
                             && DynamicRange[0] + double.Parse(data.FillValue)
                             >= double.Parse(data.LowLimit))
                        {
                            data.Result = "Pass";
                            data.Value = Math.Round(DynamicRange[0]
                                + double.Parse(data.FillValue), 3).ToString();
                            break;
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = Math.Round(DynamicRange[0]
                                 + double.Parse(data.FillValue), 3).ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData SpeakerDynamicRange_Right(TestData data)
        {
            try
            {
                if (DynamicRange[1] + double.Parse(data.FillValue)
                    <= double.Parse(data.UppLimit)
                        && DynamicRange[1] + double.Parse(data.FillValue)
                        >= double.Parse(data.LowLimit))
                {
                    data.Result = "Pass";
                    data.Value = Math.Round(DynamicRange[1]
                         + double.Parse(data.FillValue), 3).ToString();
                }
                else
                {
                    data.Result = "Fail";
                    data.Value = Math.Round(DynamicRange[1]
                         + double.Parse(data.FillValue), 3).ToString();
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        //Micphone
        public TestData MicphoneLevel(TestData data)
        {
            try
            {
                for (int i = 0; i < 3; i++)
                {
                    ATc.Sequence["Micphone"]["Level and Gain"].Run();
                    //ATc.LevelAndGain.Level.Axis.Unit = data.Unit;
                    if (ATc.Sequence["Micphone"]["Level and Gain"].HasSequenceResults)
                    {
                        ISequenceResultCollection results = ATc.Sequence["Micphone"]
                            ["Level and Gain"].SequenceResults;

                        double[] MicLevels = results[0].GetMeterValues();

                        if (MicLevels[0] + double.Parse(data.FillValue)
                            <= double.Parse(data.UppLimit)
                            && MicLevels[0] + double.Parse(data.FillValue)
                            >= double.Parse(data.LowLimit))
                        {
                            data.Result = "Pass";
                            data.Value = Math.Round(MicLevels[0]
                                 + double.Parse(data.FillValue), 3).ToString();
                            break;
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = Math.Round(MicLevels[0]
                                 + double.Parse(data.FillValue), 3).ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData MicphoneTHD(TestData data)
        {
            try
            {
                for (int i = 0; i < 3; i++)
                {
                    ATc.Sequence["Micphone"]["THD+N"].Run();
                    //ATc.ThdN.ThdRatio.Axis.Unit = data.Unit;
                    if (ATc.Sequence["Micphone"]["THD+N"].HasSequenceResults)
                    {
                        ISequenceResultCollection results = ATc.Sequence["Micphone"]["THD+N"]
                            .SequenceResults;
                        double[] MicTHD = results.Count > 2
                            ? results[2].GetMeterValues() : results[0].GetMeterValues();
                        if (MicTHD[0] + double.Parse(data.FillValue)
                            <= double.Parse(data.UppLimit)
                             && MicTHD[0] + double.Parse(data.FillValue)
                             >= double.Parse(data.LowLimit))
                        {
                            data.Result = "Pass";
                            data.Value = Math.Round(MicTHD[0]
                                 + double.Parse(data.FillValue), 3).ToString();
                            break;
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = Math.Round(MicTHD[0]
                                 + double.Parse(data.FillValue), 3).ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData MicphoneSNR(TestData data)
        {
            try
            {
                //ATc.SignalToNoiseRatio.SignalToNoiseRatio.Axis.Unit = data.Unit;
                for (int i = 0; i < 3; i++)
                {
                    ATc.Sequence["Micphone"]["Signal to Noise Ratio"].Run();
                    //ATc.SignalToNoiseRatio.SignalToNoiseRatio.Axis.Unit = data.Unit;
                    if (ATc.Sequence["Micphone"]["Signal to Noise Ratio"].HasSequenceResults)
                    {
                        ISequenceResultCollection results = ATc.Sequence["Micphone"]["Signal to Noise Ratio"]
                            .SequenceResults;
                        double[] MicSNR = results[0].GetMeterValues();
                        if (MicSNR[0] + double.Parse(data.FillValue)
                            <= double.Parse(data.UppLimit)
                             && MicSNR[0] + double.Parse(data.FillValue)
                             >= double.Parse(data.LowLimit))
                        {
                            data.Result = "Pass";
                            data.Value = Math.Round(MicSNR[0] 
                                + double.Parse(data.FillValue), 3).ToString();
                            break;
                        }
                        else
                        {
                            data.Result = "Fail";
                            data.Value = Math.Round(MicSNR[0]
                                 + double.Parse(data.FillValue), 3).ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                data.Value = "Fail";
                data.Result = "Fail";
            }
            return data;
        }

        public TestData DisConnect(TestData data)
        {
            try
            {
                if(btStatus)
                {
                    ATc.BtsimSettings.Disconnect();
                }
                else
                {
                    ATc.BluetoothSettings.Disconnect();
                }
                
                data.Result = "Pass";
                data.Value = "Pass";
            }
            catch(Exception ex)
            {
                data.Result = "Fail";
                data.Value = "Fail";
            }
            return data;
        }

        public TestData SetMaxVolume(TestData data)
        {
            for (int i = 0; i < 50; i++)
            {
                Others.MaxVolume();
            }
            return data;
        }

        public void ExitA2()
        {
            if (ATc != null)
            {
                ATc.Exit();
            }
        }

    }
}
