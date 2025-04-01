namespace WinForm
{
    partial class Winform
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Winform));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ReLoadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TestItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mES功能ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mESToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StyleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label_Test_Version = new System.Windows.Forms.Label();
            this.cht_PassRadio = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.label_Time = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.dgv_Data = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lb_Message = new System.Windows.Forms.ListBox();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label_TotalNumber = new System.Windows.Forms.Label();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.label_Defect_rate = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.label_PassNumber = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label_TestResult = new Sunny.UI.UILabel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.uiStyleManager1 = new Sunny.UI.UIStyleManager(this.components);
            this.btTest = new Sunny.UI.UIButton();
            this.tb_SN = new Sunny.UI.UITextBox();
            this.ProgressEnd = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.StopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.程序重新加载ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cht_PassRadio)).BeginInit();
            this.groupBox13.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Data)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.ProgressEnd.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(6, 56);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(185, 80);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.配置ToolStripMenuItem,
            this.mES功能ToolStripMenuItem,
            this.StyleToolStripMenuItem,
            this.关于ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 31);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(764, 25);
            this.menuStrip1.TabIndex = 15;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenToolStripMenuItem,
            this.SaveAsToolStripMenuItem,
            this.ReLoadToolStripMenuItem,
            this.ExitToolStripMenuItem});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // OpenToolStripMenuItem
            // 
            this.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem";
            this.OpenToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.OpenToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.OpenToolStripMenuItem.Text = "打开";
            this.OpenToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // SaveAsToolStripMenuItem
            // 
            this.SaveAsToolStripMenuItem.Name = "SaveAsToolStripMenuItem";
            this.SaveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.SaveAsToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.SaveAsToolStripMenuItem.Text = "另存为";
            this.SaveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAsToolStripMenuItem_Click);
            // 
            // ReLoadToolStripMenuItem
            // 
            this.ReLoadToolStripMenuItem.Name = "ReLoadToolStripMenuItem";
            this.ReLoadToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.ReLoadToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.ReLoadToolStripMenuItem.Text = "重新加载程序";
            this.ReLoadToolStripMenuItem.Click += new System.EventHandler(this.ReLoadToolStripMenuItem_Click);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.ExitToolStripMenuItem.Text = "退出";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // 配置ToolStripMenuItem
            // 
            this.配置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ConfigToolStripMenuItem,
            this.TestItemToolStripMenuItem});
            this.配置ToolStripMenuItem.Name = "配置ToolStripMenuItem";
            this.配置ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.配置ToolStripMenuItem.Text = "配置";
            // 
            // ConfigToolStripMenuItem
            // 
            this.ConfigToolStripMenuItem.Name = "ConfigToolStripMenuItem";
            this.ConfigToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.ConfigToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.ConfigToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.ConfigToolStripMenuItem.Text = "测试参数配置";
            this.ConfigToolStripMenuItem.Click += new System.EventHandler(this.ConfigToolStripMenuItem_Click);
            // 
            // TestItemToolStripMenuItem
            // 
            this.TestItemToolStripMenuItem.Name = "TestItemToolStripMenuItem";
            this.TestItemToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.TestItemToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.TestItemToolStripMenuItem.Text = "测试项目配置";
            this.TestItemToolStripMenuItem.Click += new System.EventHandler(this.TestItemToolStripMenuItem_Click);
            // 
            // mES功能ToolStripMenuItem
            // 
            this.mES功能ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mESToolStripMenuItem});
            this.mES功能ToolStripMenuItem.Name = "mES功能ToolStripMenuItem";
            this.mES功能ToolStripMenuItem.Size = new System.Drawing.Size(70, 21);
            this.mES功能ToolStripMenuItem.Text = "MES功能";
            // 
            // mESToolStripMenuItem
            // 
            this.mESToolStripMenuItem.Name = "mESToolStripMenuItem";
            this.mESToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.mESToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.mESToolStripMenuItem.Text = "MES功能设置";
            this.mESToolStripMenuItem.Click += new System.EventHandler(this.mESToolStripMenuItem_Click);
            // 
            // StyleToolStripMenuItem
            // 
            this.StyleToolStripMenuItem.Name = "StyleToolStripMenuItem";
            this.StyleToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.StyleToolStripMenuItem.Text = "主题";
            this.StyleToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.StyleToolStripMenuItem_DropDownItemClicked);
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AboutToolStripMenuItem});
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.关于ToolStripMenuItem.Text = "关于";
            // 
            // AboutToolStripMenuItem
            // 
            this.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
            this.AboutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.AboutToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.AboutToolStripMenuItem.Text = "关于";
            this.AboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // label_Test_Version
            // 
            this.label_Test_Version.Font = new System.Drawing.Font("Arial", 35F, System.Drawing.FontStyle.Bold);
            this.label_Test_Version.Location = new System.Drawing.Point(196, 63);
            this.label_Test_Version.Name = "label_Test_Version";
            this.label_Test_Version.Size = new System.Drawing.Size(551, 67);
            this.label_Test_Version.TabIndex = 1;
            this.label_Test_Version.Text = "RF Function Test";
            this.label_Test_Version.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cht_PassRadio
            // 
            this.cht_PassRadio.BackColor = System.Drawing.SystemColors.Control;
            chartArea2.BackColor = System.Drawing.SystemColors.Control;
            chartArea2.Name = "ChartArea1";
            this.cht_PassRadio.ChartAreas.Add(chartArea2);
            this.cht_PassRadio.Location = new System.Drawing.Point(162, 25);
            this.cht_PassRadio.Name = "cht_PassRadio";
            this.cht_PassRadio.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Berry;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series2.Name = "Series1";
            this.cht_PassRadio.Series.Add(series2);
            this.cht_PassRadio.Size = new System.Drawing.Size(120, 109);
            this.cht_PassRadio.TabIndex = 6;
            this.cht_PassRadio.Text = "chart1";
            this.cht_PassRadio.Click += new System.EventHandler(this.cht_PassRadio_Click);
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.label_Time);
            this.groupBox13.Location = new System.Drawing.Point(84, 79);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(70, 57);
            this.groupBox13.TabIndex = 4;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "测试时间";
            // 
            // label_Time
            // 
            this.label_Time.BackColor = System.Drawing.SystemColors.Control;
            this.label_Time.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_Time.Font = new System.Drawing.Font("Arial", 9F);
            this.label_Time.Location = new System.Drawing.Point(4, 20);
            this.label_Time.Name = "label_Time";
            this.label_Time.Size = new System.Drawing.Size(59, 29);
            this.label_Time.TabIndex = 4;
            this.label_Time.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.dgv_Data);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.groupBox4.Location = new System.Drawing.Point(4, 136);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(466, 355);
            this.groupBox4.TabIndex = 18;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "测试项目";
            // 
            // dgv_Data
            // 
            this.dgv_Data.AllowUserToAddRows = false;
            this.dgv_Data.AllowUserToDeleteRows = false;
            this.dgv_Data.AllowUserToResizeColumns = false;
            this.dgv_Data.AllowUserToResizeRows = false;
            this.dgv_Data.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_Data.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgv_Data.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgv_Data.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_Data.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dgv_Data.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgv_Data.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column7,
            this.Column5,
            this.Column6});
            this.dgv_Data.ContextMenuStrip = this.contextMenuStrip1;
            this.dgv_Data.Location = new System.Drawing.Point(5, 19);
            this.dgv_Data.MultiSelect = false;
            this.dgv_Data.Name = "dgv_Data";
            this.dgv_Data.ReadOnly = true;
            this.dgv_Data.RowHeadersVisible = false;
            this.dgv_Data.RowHeadersWidth = 10;
            this.dgv_Data.RowTemplate.Height = 24;
            this.dgv_Data.Size = new System.Drawing.Size(455, 328);
            this.dgv_Data.TabIndex = 11;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "ID";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column1.Width = 27;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column2.HeaderText = "Test_Item";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column2.Width = 76;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column3.HeaderText = "L_Spec";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column3.Width = 62;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column4.HeaderText = "H_Spec";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column4.Width = 64;
            // 
            // Column7
            // 
            this.Column7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column7.HeaderText = "Unit";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column7.Width = 39;
            // 
            // Column5
            // 
            this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column5.HeaderText = "Value";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column5.Width = 50;
            // 
            // Column6
            // 
            this.Column6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column6.HeaderText = "Result";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(107, 26);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lb_Message);
            this.groupBox2.Location = new System.Drawing.Point(4, 492);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(466, 94);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "程序运行信息";
            // 
            // lb_Message
            // 
            this.lb_Message.BackColor = System.Drawing.Color.White;
            this.lb_Message.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lb_Message.ForeColor = System.Drawing.Color.Black;
            this.lb_Message.FormattingEnabled = true;
            this.lb_Message.ItemHeight = 16;
            this.lb_Message.Location = new System.Drawing.Point(6, 20);
            this.lb_Message.Name = "lb_Message";
            this.lb_Message.ScrollAlwaysVisible = true;
            this.lb_Message.Size = new System.Drawing.Size(456, 68);
            this.lb_Message.TabIndex = 8;
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.cht_PassRadio);
            this.groupBox11.Controls.Add(this.groupBox13);
            this.groupBox11.Controls.Add(this.groupBox7);
            this.groupBox11.Controls.Add(this.groupBox9);
            this.groupBox11.Controls.Add(this.groupBox8);
            this.groupBox11.Location = new System.Drawing.Point(473, 133);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(287, 140);
            this.groupBox11.TabIndex = 22;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "产品测试信息";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.label_TotalNumber);
            this.groupBox7.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.groupBox7.Location = new System.Drawing.Point(6, 19);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(70, 57);
            this.groupBox7.TabIndex = 1;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Total";
            // 
            // label_TotalNumber
            // 
            this.label_TotalNumber.BackColor = System.Drawing.SystemColors.Control;
            this.label_TotalNumber.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_TotalNumber.Font = new System.Drawing.Font("Arial", 9F);
            this.label_TotalNumber.Location = new System.Drawing.Point(5, 19);
            this.label_TotalNumber.Name = "label_TotalNumber";
            this.label_TotalNumber.Size = new System.Drawing.Size(59, 29);
            this.label_TotalNumber.TabIndex = 2;
            this.label_TotalNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.label_Defect_rate);
            this.groupBox9.Location = new System.Drawing.Point(6, 79);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(68, 57);
            this.groupBox9.TabIndex = 3;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Pass比率";
            // 
            // label_Defect_rate
            // 
            this.label_Defect_rate.BackColor = System.Drawing.SystemColors.Control;
            this.label_Defect_rate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_Defect_rate.Font = new System.Drawing.Font("Arial", 9F);
            this.label_Defect_rate.Location = new System.Drawing.Point(4, 20);
            this.label_Defect_rate.Name = "label_Defect_rate";
            this.label_Defect_rate.Size = new System.Drawing.Size(59, 29);
            this.label_Defect_rate.TabIndex = 4;
            this.label_Defect_rate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.label_PassNumber);
            this.groupBox8.Location = new System.Drawing.Point(84, 19);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(70, 57);
            this.groupBox8.TabIndex = 2;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Pass";
            // 
            // label_PassNumber
            // 
            this.label_PassNumber.BackColor = System.Drawing.SystemColors.Control;
            this.label_PassNumber.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_PassNumber.Font = new System.Drawing.Font("Arial", 9F);
            this.label_PassNumber.Location = new System.Drawing.Point(4, 20);
            this.label_PassNumber.Name = "label_PassNumber";
            this.label_PassNumber.Size = new System.Drawing.Size(59, 29);
            this.label_PassNumber.TabIndex = 3;
            this.label_PassNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label_TestResult);
            this.groupBox6.Location = new System.Drawing.Point(472, 274);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(289, 217);
            this.groupBox6.TabIndex = 23;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "测试状态";
            // 
            // label_TestResult
            // 
            this.label_TestResult.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label_TestResult.Font = new System.Drawing.Font("Courier New", 60F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_TestResult.Location = new System.Drawing.Point(7, 24);
            this.label_TestResult.Name = "label_TestResult";
            this.label_TestResult.Size = new System.Drawing.Size(276, 185);
            this.label_TestResult.Style = Sunny.UI.UIStyle.Custom;
            this.label_TestResult.TabIndex = 0;
            this.label_TestResult.Text = "Ready";
            this.label_TestResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btTest
            // 
            this.btTest.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btTest.Font = new System.Drawing.Font("微软雅黑", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btTest.Location = new System.Drawing.Point(473, 547);
            this.btTest.MinimumSize = new System.Drawing.Size(1, 1);
            this.btTest.Name = "btTest";
            this.btTest.Size = new System.Drawing.Size(288, 40);
            this.btTest.Style = Sunny.UI.UIStyle.Custom;
            this.btTest.TabIndex = 24;
            this.btTest.Text = "测试";
            this.btTest.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btTest.Click += new System.EventHandler(this.btTest_Click);
            // 
            // tb_SN
            // 
            this.tb_SN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tb_SN.ButtonSymbol = 61761;
            this.tb_SN.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tb_SN.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.tb_SN.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_SN.Location = new System.Drawing.Point(474, 505);
            this.tb_SN.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tb_SN.Maximum = 2147483647D;
            this.tb_SN.Minimum = -2147483648D;
            this.tb_SN.MinimumSize = new System.Drawing.Size(1, 16);
            this.tb_SN.Name = "tb_SN";
            this.tb_SN.Size = new System.Drawing.Size(285, 35);
            this.tb_SN.Style = Sunny.UI.UIStyle.Custom;
            this.tb_SN.TabIndex = 25;
            this.tb_SN.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.tb_SN.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Tb_SN_KeyDown);
            // 
            // ProgressEnd
            // 
            this.ProgressEnd.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.ProgressEnd.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StopToolStripMenuItem,
            this.程序重新加载ToolStripMenuItem,
            this.退出ToolStripMenuItem});
            this.ProgressEnd.Name = "ProgressEnd";
            this.ProgressEnd.Size = new System.Drawing.Size(149, 70);
            // 
            // StopToolStripMenuItem
            // 
            this.StopToolStripMenuItem.Name = "StopToolStripMenuItem";
            this.StopToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.StopToolStripMenuItem.Text = "测试停止";
            this.StopToolStripMenuItem.Click += new System.EventHandler(this.StopToolStripMenuItem_Click);
            // 
            // 程序重新加载ToolStripMenuItem
            // 
            this.程序重新加载ToolStripMenuItem.Name = "程序重新加载ToolStripMenuItem";
            this.程序重新加载ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.程序重新加载ToolStripMenuItem.Text = "程序重新加载";
            this.程序重新加载ToolStripMenuItem.Click += new System.EventHandler(this.ReLoadToolStripMenuItem_Click);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // Winform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(764, 591);
            this.ContextMenuStrip = this.ProgressEnd;
            this.Controls.Add(this.tb_SN);
            this.Controls.Add(this.btTest);
            this.Controls.Add(this.label_Test_Version);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox11);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Winform";
            this.Padding = new System.Windows.Forms.Padding(0, 31, 0, 0);
            this.ShowDragStretch = true;
            this.ShowRadius = false;
            this.ShowShadow = true;
            this.ShowTitleIcon = true;
            this.Style = Sunny.UI.UIStyle.Custom;
            this.Text = "WinForm";
            this.TitleHeight = 31;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Winform_FormClosed);
            this.Load += new System.EventHandler(this.Winform_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cht_PassRadio)).EndInit();
            this.groupBox13.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Data)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox11.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.ProgressEnd.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 配置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ConfigToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AboutToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.Label label_Test_Version;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox lb_Message;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.DataVisualization.Charting.Chart cht_PassRadio;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.DataGridView dgv_Data;
        private System.Windows.Forms.ToolStripMenuItem TestItemToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label_PassNumber;
        private System.Windows.Forms.Label label_Defect_rate;
        private System.Windows.Forms.Label label_TotalNumber;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem mES功能ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mESToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OpenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ReLoadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem StyleToolStripMenuItem;
        private Sunny.UI.UIStyleManager uiStyleManager1;
        private System.Windows.Forms.Label label_Time;
        private Sunny.UI.UIButton btTest;
        private Sunny.UI.UILabel label_TestResult;
        private Sunny.UI.UITextBox tb_SN;
        private System.Windows.Forms.ContextMenuStrip ProgressEnd;
        private System.Windows.Forms.ToolStripMenuItem StopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 程序重新加载ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
    }
}

