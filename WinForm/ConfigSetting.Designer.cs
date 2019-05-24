namespace WinForm
{
    partial class ConfigSetting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tb_Title = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cb_Serial = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cb_Power = new System.Windows.Forms.ComboBox();
            this.cb_Multimeter = new System.Windows.Forms.ComboBox();
            this.cb_GPIB = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tb_Packets = new System.Windows.Forms.TextBox();
            this.tb_InqTimeOut = new System.Windows.Forms.TextBox();
            this.tb_RXPower = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tb_HiLoss = new System.Windows.Forms.TextBox();
            this.tb_ModLoss = new System.Windows.Forms.TextBox();
            this.tb_HiFreq = new System.Windows.Forms.TextBox();
            this.tb_LowLoss = new System.Windows.Forms.TextBox();
            this.tb_ModFreq = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tb_LowFreq = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.bt_Save = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cb_cd = new System.Windows.Forms.CheckBox();
            this.cb_mi = new System.Windows.Forms.CheckBox();
            this.cb_ss = new System.Windows.Forms.CheckBox();
            this.cb_ic = new System.Windows.Forms.CheckBox();
            this.cb_op = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.tb_Current = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.tb_Voltage2 = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tb_Voltage1 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.Multimeter_Select = new System.Windows.Forms.CheckBox();
            this.Power_Select = new System.Windows.Forms.CheckBox();
            this.Serial_Select = new System.Windows.Forms.CheckBox();
            this.GPIB_Select = new System.Windows.Forms.CheckBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.tb_SNLength = new System.Windows.Forms.TextBox();
            this.tb_CompareString = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.cb_SerialSelect = new System.Windows.Forms.CheckBox();
            this.bt_Select = new System.Windows.Forms.Button();
            this.cb_AudioEnable = new System.Windows.Forms.CheckBox();
            this.tb_Path = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.cb_SNAuto = new System.Windows.Forms.CheckBox();
            this.label21 = new System.Windows.Forms.Label();
            this.tb_SNHear = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.tb_Line = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.cb_FixPort = new System.Windows.Forms.ComboBox();
            this.cb_FixAuto = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tb_Title);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(8, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(507, 47);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "标题设置";
            // 
            // tb_Title
            // 
            this.tb_Title.Location = new System.Drawing.Point(43, 17);
            this.tb_Title.Name = "tb_Title";
            this.tb_Title.Size = new System.Drawing.Size(460, 21);
            this.tb_Title.TabIndex = 1;
            this.tb_Title.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "标题";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cb_Serial);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.cb_Power);
            this.groupBox2.Controls.Add(this.cb_Multimeter);
            this.groupBox2.Controls.Add(this.cb_GPIB);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.label19);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(8, 52);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(507, 55);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "测试仪器端口设置";
            // 
            // cb_Serial
            // 
            this.cb_Serial.FormattingEnabled = true;
            this.cb_Serial.Location = new System.Drawing.Point(173, 20);
            this.cb_Serial.Name = "cb_Serial";
            this.cb_Serial.Size = new System.Drawing.Size(54, 20);
            this.cb_Serial.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(142, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "串口";
            // 
            // cb_Power
            // 
            this.cb_Power.FormattingEnabled = true;
            this.cb_Power.Location = new System.Drawing.Point(261, 20);
            this.cb_Power.Name = "cb_Power";
            this.cb_Power.Size = new System.Drawing.Size(99, 20);
            this.cb_Power.TabIndex = 1;
            // 
            // cb_Multimeter
            // 
            this.cb_Multimeter.FormattingEnabled = true;
            this.cb_Multimeter.Location = new System.Drawing.Point(404, 20);
            this.cb_Multimeter.Name = "cb_Multimeter";
            this.cb_Multimeter.Size = new System.Drawing.Size(99, 20);
            this.cb_Multimeter.TabIndex = 1;
            // 
            // cb_GPIB
            // 
            this.cb_GPIB.FormattingEnabled = true;
            this.cb_GPIB.Location = new System.Drawing.Point(36, 20);
            this.cb_GPIB.Name = "cb_GPIB";
            this.cb_GPIB.Size = new System.Drawing.Size(99, 20);
            this.cb_GPIB.TabIndex = 1;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(231, 23);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(29, 12);
            this.label16.TabIndex = 0;
            this.label16.Text = "电源";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(364, 23);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(41, 12);
            this.label19.TabIndex = 0;
            this.label19.Text = "万用表";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "8852";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tb_Packets);
            this.groupBox3.Controls.Add(this.tb_InqTimeOut);
            this.groupBox3.Controls.Add(this.tb_RXPower);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(9, 358);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(507, 50);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "RF参数设置";
            // 
            // tb_Packets
            // 
            this.tb_Packets.Location = new System.Drawing.Point(290, 17);
            this.tb_Packets.Name = "tb_Packets";
            this.tb_Packets.Size = new System.Drawing.Size(48, 21);
            this.tb_Packets.TabIndex = 1;
            // 
            // tb_InqTimeOut
            // 
            this.tb_InqTimeOut.Location = new System.Drawing.Point(455, 17);
            this.tb_InqTimeOut.Name = "tb_InqTimeOut";
            this.tb_InqTimeOut.Size = new System.Drawing.Size(48, 21);
            this.tb_InqTimeOut.TabIndex = 1;
            // 
            // tb_RXPower
            // 
            this.tb_RXPower.Location = new System.Drawing.Point(129, 17);
            this.tb_RXPower.Name = "tb_RXPower";
            this.tb_RXPower.Size = new System.Drawing.Size(48, 21);
            this.tb_RXPower.TabIndex = 1;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(340, 21);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(113, 12);
            this.label12.TabIndex = 0;
            this.label12.Text = "Inquiry TimeOut(s)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(180, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "number of packets";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "Sensitivity_Tx-power";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tb_HiLoss);
            this.groupBox4.Controls.Add(this.tb_ModLoss);
            this.groupBox4.Controls.Add(this.tb_HiFreq);
            this.groupBox4.Controls.Add(this.tb_LowLoss);
            this.groupBox4.Controls.Add(this.tb_ModFreq);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.tb_LowFreq);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Location = new System.Drawing.Point(174, 410);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(347, 105);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "PathLoss Table";
            // 
            // tb_HiLoss
            // 
            this.tb_HiLoss.Location = new System.Drawing.Point(258, 72);
            this.tb_HiLoss.Name = "tb_HiLoss";
            this.tb_HiLoss.Size = new System.Drawing.Size(77, 21);
            this.tb_HiLoss.TabIndex = 1;
            // 
            // tb_ModLoss
            // 
            this.tb_ModLoss.Location = new System.Drawing.Point(258, 45);
            this.tb_ModLoss.Name = "tb_ModLoss";
            this.tb_ModLoss.Size = new System.Drawing.Size(77, 21);
            this.tb_ModLoss.TabIndex = 1;
            // 
            // tb_HiFreq
            // 
            this.tb_HiFreq.Location = new System.Drawing.Point(94, 70);
            this.tb_HiFreq.Name = "tb_HiFreq";
            this.tb_HiFreq.Size = new System.Drawing.Size(77, 21);
            this.tb_HiFreq.TabIndex = 1;
            // 
            // tb_LowLoss
            // 
            this.tb_LowLoss.Location = new System.Drawing.Point(258, 19);
            this.tb_LowLoss.Name = "tb_LowLoss";
            this.tb_LowLoss.Size = new System.Drawing.Size(77, 21);
            this.tb_LowLoss.TabIndex = 1;
            // 
            // tb_ModFreq
            // 
            this.tb_ModFreq.Location = new System.Drawing.Point(94, 43);
            this.tb_ModFreq.Name = "tb_ModFreq";
            this.tb_ModFreq.Size = new System.Drawing.Size(77, 21);
            this.tb_ModFreq.TabIndex = 1;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(175, 77);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(71, 12);
            this.label11.TabIndex = 0;
            this.label11.Text = "Hi_PathLoss";
            // 
            // tb_LowFreq
            // 
            this.tb_LowFreq.Location = new System.Drawing.Point(94, 17);
            this.tb_LowFreq.Name = "tb_LowFreq";
            this.tb_LowFreq.Size = new System.Drawing.Size(77, 21);
            this.tb_LowFreq.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(175, 48);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 12);
            this.label10.TabIndex = 0;
            this.label10.Text = "Mod_PathLoss";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 75);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "Hi_Frequency";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(175, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 12);
            this.label9.TabIndex = 0;
            this.label9.Text = "Low_PathLoss";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 46);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "Mod_Frequency";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "Low_Frequency";
            // 
            // bt_Save
            // 
            this.bt_Save.Location = new System.Drawing.Point(223, 522);
            this.bt_Save.Name = "bt_Save";
            this.bt_Save.Size = new System.Drawing.Size(117, 43);
            this.bt_Save.TabIndex = 3;
            this.bt_Save.Text = "保存并退出";
            this.bt_Save.UseVisualStyleBackColor = true;
            this.bt_Save.Click += new System.EventHandler(this.bt_Save_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cb_cd);
            this.groupBox5.Controls.Add(this.cb_mi);
            this.groupBox5.Controls.Add(this.cb_ss);
            this.groupBox5.Controls.Add(this.cb_ic);
            this.groupBox5.Controls.Add(this.cb_op);
            this.groupBox5.Location = new System.Drawing.Point(9, 284);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(507, 70);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "MT8852B测试项目";
            // 
            // cb_cd
            // 
            this.cb_cd.AutoSize = true;
            this.cb_cd.Location = new System.Drawing.Point(7, 43);
            this.cb_cd.Name = "cb_cd";
            this.cb_cd.Size = new System.Drawing.Size(132, 16);
            this.cb_cd.TabIndex = 0;
            this.cb_cd.Text = "Carrier drift test";
            this.cb_cd.UseVisualStyleBackColor = true;
            // 
            // cb_mi
            // 
            this.cb_mi.AutoSize = true;
            this.cb_mi.Location = new System.Drawing.Point(170, 43);
            this.cb_mi.Name = "cb_mi";
            this.cb_mi.Size = new System.Drawing.Size(150, 16);
            this.cb_mi.TabIndex = 0;
            this.cb_mi.Text = "Modulation Index test";
            this.cb_mi.UseVisualStyleBackColor = true;
            // 
            // cb_ss
            // 
            this.cb_ss.AutoSize = true;
            this.cb_ss.Location = new System.Drawing.Point(170, 20);
            this.cb_ss.Name = "cb_ss";
            this.cb_ss.Size = new System.Drawing.Size(162, 16);
            this.cb_ss.TabIndex = 0;
            this.cb_ss.Text = "Single sensitivity test";
            this.cb_ss.UseVisualStyleBackColor = true;
            // 
            // cb_ic
            // 
            this.cb_ic.AutoSize = true;
            this.cb_ic.Location = new System.Drawing.Point(351, 21);
            this.cb_ic.Name = "cb_ic";
            this.cb_ic.Size = new System.Drawing.Size(144, 16);
            this.cb_ic.TabIndex = 0;
            this.cb_ic.Text = "Initial carrier test";
            this.cb_ic.UseVisualStyleBackColor = true;
            // 
            // cb_op
            // 
            this.cb_op.AutoSize = true;
            this.cb_op.Location = new System.Drawing.Point(7, 21);
            this.cb_op.Name = "cb_op";
            this.cb_op.Size = new System.Drawing.Size(108, 16);
            this.cb_op.TabIndex = 0;
            this.cb_op.Text = "Out Power test";
            this.cb_op.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.tb_Current);
            this.groupBox6.Controls.Add(this.label15);
            this.groupBox6.Controls.Add(this.tb_Voltage2);
            this.groupBox6.Controls.Add(this.label14);
            this.groupBox6.Controls.Add(this.tb_Voltage1);
            this.groupBox6.Controls.Add(this.label13);
            this.groupBox6.Location = new System.Drawing.Point(9, 410);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(154, 105);
            this.groupBox6.TabIndex = 5;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "电源设置";
            // 
            // tb_Current
            // 
            this.tb_Current.Location = new System.Drawing.Point(60, 73);
            this.tb_Current.Name = "tb_Current";
            this.tb_Current.Size = new System.Drawing.Size(88, 21);
            this.tb_Current.TabIndex = 1;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(5, 76);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(47, 12);
            this.label15.TabIndex = 0;
            this.label15.Text = "电流(A)";
            // 
            // tb_Voltage2
            // 
            this.tb_Voltage2.Location = new System.Drawing.Point(60, 46);
            this.tb_Voltage2.Name = "tb_Voltage2";
            this.tb_Voltage2.Size = new System.Drawing.Size(88, 21);
            this.tb_Voltage2.TabIndex = 1;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(4, 50);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 12);
            this.label14.TabIndex = 0;
            this.label14.Text = "通道2(V)";
            // 
            // tb_Voltage1
            // 
            this.tb_Voltage1.Location = new System.Drawing.Point(60, 18);
            this.tb_Voltage1.Name = "tb_Voltage1";
            this.tb_Voltage1.Size = new System.Drawing.Size(88, 21);
            this.tb_Voltage1.TabIndex = 1;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(5, 21);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 0;
            this.label13.Text = "通道1(V)";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.Multimeter_Select);
            this.groupBox7.Controls.Add(this.Power_Select);
            this.groupBox7.Controls.Add(this.Serial_Select);
            this.groupBox7.Controls.Add(this.GPIB_Select);
            this.groupBox7.Location = new System.Drawing.Point(8, 153);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(243, 55);
            this.groupBox7.TabIndex = 6;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "仪器使用选择";
            // 
            // Multimeter_Select
            // 
            this.Multimeter_Select.AutoSize = true;
            this.Multimeter_Select.Location = new System.Drawing.Point(168, 25);
            this.Multimeter_Select.Name = "Multimeter_Select";
            this.Multimeter_Select.Size = new System.Drawing.Size(60, 16);
            this.Multimeter_Select.TabIndex = 0;
            this.Multimeter_Select.Text = "万用表";
            this.Multimeter_Select.UseVisualStyleBackColor = true;
            // 
            // Power_Select
            // 
            this.Power_Select.AutoSize = true;
            this.Power_Select.Location = new System.Drawing.Point(115, 24);
            this.Power_Select.Name = "Power_Select";
            this.Power_Select.Size = new System.Drawing.Size(48, 16);
            this.Power_Select.TabIndex = 0;
            this.Power_Select.Text = "电源";
            this.Power_Select.UseVisualStyleBackColor = true;
            // 
            // Serial_Select
            // 
            this.Serial_Select.AutoSize = true;
            this.Serial_Select.Location = new System.Drawing.Point(62, 25);
            this.Serial_Select.Name = "Serial_Select";
            this.Serial_Select.Size = new System.Drawing.Size(48, 16);
            this.Serial_Select.TabIndex = 0;
            this.Serial_Select.Text = "串口";
            this.Serial_Select.UseVisualStyleBackColor = true;
            // 
            // GPIB_Select
            // 
            this.GPIB_Select.AutoSize = true;
            this.GPIB_Select.Location = new System.Drawing.Point(8, 25);
            this.GPIB_Select.Name = "GPIB_Select";
            this.GPIB_Select.Size = new System.Drawing.Size(48, 16);
            this.GPIB_Select.TabIndex = 0;
            this.GPIB_Select.Text = "8852";
            this.GPIB_Select.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.tb_SNLength);
            this.groupBox8.Controls.Add(this.tb_CompareString);
            this.groupBox8.Controls.Add(this.label18);
            this.groupBox8.Controls.Add(this.label17);
            this.groupBox8.Location = new System.Drawing.Point(258, 153);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(257, 55);
            this.groupBox8.TabIndex = 7;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "SN比对及长度";
            // 
            // tb_SNLength
            // 
            this.tb_SNLength.Location = new System.Drawing.Point(183, 23);
            this.tb_SNLength.Name = "tb_SNLength";
            this.tb_SNLength.Size = new System.Drawing.Size(66, 21);
            this.tb_SNLength.TabIndex = 1;
            // 
            // tb_CompareString
            // 
            this.tb_CompareString.Location = new System.Drawing.Point(62, 23);
            this.tb_CompareString.Name = "tb_CompareString";
            this.tb_CompareString.Size = new System.Drawing.Size(73, 21);
            this.tb_CompareString.TabIndex = 1;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(141, 26);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(41, 12);
            this.label18.TabIndex = 0;
            this.label18.Text = "SN长度";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(7, 26);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(53, 12);
            this.label17.TabIndex = 0;
            this.label17.Text = "判断字符";
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.cb_SerialSelect);
            this.groupBox9.Controls.Add(this.bt_Select);
            this.groupBox9.Controls.Add(this.cb_AudioEnable);
            this.groupBox9.Controls.Add(this.tb_Path);
            this.groupBox9.Controls.Add(this.label20);
            this.groupBox9.Location = new System.Drawing.Point(8, 107);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(507, 40);
            this.groupBox9.TabIndex = 8;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Audio测试";
            // 
            // cb_SerialSelect
            // 
            this.cb_SerialSelect.AutoSize = true;
            this.cb_SerialSelect.Location = new System.Drawing.Point(94, 18);
            this.cb_SerialSelect.Name = "cb_SerialSelect";
            this.cb_SerialSelect.Size = new System.Drawing.Size(60, 16);
            this.cb_SerialSelect.TabIndex = 4;
            this.cb_SerialSelect.Tag = "";
            this.cb_SerialSelect.Text = "串口板";
            this.cb_SerialSelect.UseVisualStyleBackColor = true;
            // 
            // bt_Select
            // 
            this.bt_Select.Location = new System.Drawing.Point(424, 14);
            this.bt_Select.Name = "bt_Select";
            this.bt_Select.Size = new System.Drawing.Size(75, 23);
            this.bt_Select.TabIndex = 3;
            this.bt_Select.Text = "选择";
            this.bt_Select.UseVisualStyleBackColor = true;
            this.bt_Select.Click += new System.EventHandler(this.bt_Select_Click);
            // 
            // cb_AudioEnable
            // 
            this.cb_AudioEnable.AutoSize = true;
            this.cb_AudioEnable.Location = new System.Drawing.Point(13, 18);
            this.cb_AudioEnable.Name = "cb_AudioEnable";
            this.cb_AudioEnable.Size = new System.Drawing.Size(78, 16);
            this.cb_AudioEnable.TabIndex = 0;
            this.cb_AudioEnable.Text = "PCB_Audio";
            this.cb_AudioEnable.UseVisualStyleBackColor = true;
            // 
            // tb_Path
            // 
            this.tb_Path.Location = new System.Drawing.Point(240, 14);
            this.tb_Path.Name = "tb_Path";
            this.tb_Path.Size = new System.Drawing.Size(178, 21);
            this.tb_Path.TabIndex = 2;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(157, 19);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(77, 12);
            this.label20.TabIndex = 1;
            this.label20.Text = "项目文件路径";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.tb_Line);
            this.groupBox10.Controls.Add(this.label22);
            this.groupBox10.Controls.Add(this.tb_SNHear);
            this.groupBox10.Controls.Add(this.label21);
            this.groupBox10.Controls.Add(this.cb_SNAuto);
            this.groupBox10.Location = new System.Drawing.Point(9, 215);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(298, 63);
            this.groupBox10.TabIndex = 9;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "SN自动测试设置";
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.cb_FixPort);
            this.groupBox11.Controls.Add(this.label23);
            this.groupBox11.Controls.Add(this.cb_FixAuto);
            this.groupBox11.Location = new System.Drawing.Point(313, 215);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(202, 63);
            this.groupBox11.TabIndex = 9;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "屏蔽箱自动测试设置";
            // 
            // cb_SNAuto
            // 
            this.cb_SNAuto.AutoSize = true;
            this.cb_SNAuto.Location = new System.Drawing.Point(7, 30);
            this.cb_SNAuto.Name = "cb_SNAuto";
            this.cb_SNAuto.Size = new System.Drawing.Size(48, 16);
            this.cb_SNAuto.TabIndex = 0;
            this.cb_SNAuto.Text = "自动";
            this.cb_SNAuto.UseVisualStyleBackColor = true;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(54, 32);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(53, 12);
            this.label21.TabIndex = 1;
            this.label21.Text = "SN首字符";
            // 
            // tb_SNHear
            // 
            this.tb_SNHear.Location = new System.Drawing.Point(110, 28);
            this.tb_SNHear.Name = "tb_SNHear";
            this.tb_SNHear.Size = new System.Drawing.Size(74, 21);
            this.tb_SNHear.TabIndex = 2;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(191, 33);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(29, 12);
            this.label22.TabIndex = 1;
            this.label22.Text = "线体";
            // 
            // tb_Line
            // 
            this.tb_Line.Location = new System.Drawing.Point(225, 29);
            this.tb_Line.Name = "tb_Line";
            this.tb_Line.Size = new System.Drawing.Size(62, 21);
            this.tb_Line.TabIndex = 2;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(61, 32);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(65, 12);
            this.label23.TabIndex = 0;
            this.label23.Text = "屏蔽箱串口";
            // 
            // cb_FixPort
            // 
            this.cb_FixPort.FormattingEnabled = true;
            this.cb_FixPort.Location = new System.Drawing.Point(131, 28);
            this.cb_FixPort.Name = "cb_FixPort";
            this.cb_FixPort.Size = new System.Drawing.Size(60, 20);
            this.cb_FixPort.TabIndex = 1;
            // 
            // cb_FixAuto
            // 
            this.cb_FixAuto.AutoSize = true;
            this.cb_FixAuto.Location = new System.Drawing.Point(4, 31);
            this.cb_FixAuto.Name = "cb_FixAuto";
            this.cb_FixAuto.Size = new System.Drawing.Size(48, 16);
            this.cb_FixAuto.TabIndex = 0;
            this.cb_FixAuto.Text = "自动";
            this.cb_FixAuto.UseVisualStyleBackColor = true;
            // 
            // ConfigSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(523, 576);
            this.Controls.Add(this.groupBox11);
            this.Controls.Add(this.groupBox10);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.bt_Save);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "ConfigSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "配置参数设置";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ConfigSetting_FormClosed);
            this.Load += new System.EventHandler(this.ConfigSetting_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tb_Title;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cb_Serial;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cb_GPIB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox tb_Packets;
        private System.Windows.Forms.TextBox tb_RXPower;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox tb_HiLoss;
        private System.Windows.Forms.TextBox tb_ModLoss;
        private System.Windows.Forms.TextBox tb_HiFreq;
        private System.Windows.Forms.TextBox tb_LowLoss;
        private System.Windows.Forms.TextBox tb_ModFreq;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tb_LowFreq;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_InqTimeOut;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button bt_Save;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox cb_cd;
        private System.Windows.Forms.CheckBox cb_mi;
        private System.Windows.Forms.CheckBox cb_ss;
        private System.Windows.Forms.CheckBox cb_ic;
        private System.Windows.Forms.CheckBox cb_op;
        private System.Windows.Forms.ComboBox cb_Power;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox tb_Current;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox tb_Voltage2;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tb_Voltage1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.CheckBox Power_Select;
        private System.Windows.Forms.CheckBox Serial_Select;
        private System.Windows.Forms.CheckBox GPIB_Select;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.TextBox tb_SNLength;
        private System.Windows.Forms.TextBox tb_CompareString;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox cb_Multimeter;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.CheckBox Multimeter_Select;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Button bt_Select;
        private System.Windows.Forms.TextBox tb_Path;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.CheckBox cb_AudioEnable;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.CheckBox cb_SerialSelect;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.TextBox tb_Line;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox tb_SNHear;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.CheckBox cb_SNAuto;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.ComboBox cb_FixPort;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.CheckBox cb_FixAuto;
    }
}