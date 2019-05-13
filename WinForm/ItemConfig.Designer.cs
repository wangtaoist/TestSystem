﻿namespace WinForm
{
    partial class ItemConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemConfig));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tb_Other = new System.Windows.Forms.TextBox();
            this.bt_Exit = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_Check = new System.Windows.Forms.CheckBox();
            this.tb_ItemFriendname = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_afterDelayTime = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tb_Delaytimes = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tb_limitMin = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_limitMax = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_unit = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_ItemName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.bt_Save = new System.Windows.Forms.Button();
            this.gb_FunctionItem = new System.Windows.Forms.GroupBox();
            this.lst_Test_selectFunction = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_installFunction = new System.Windows.Forms.Label();
            this.cmd_down = new System.Windows.Forms.Button();
            this.cmd_up = new System.Windows.Forms.Button();
            this.cmd_delete = new System.Windows.Forms.Button();
            this.cmd_insert = new System.Windows.Forms.Button();
            this.lst_installFunction = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tb_Remark = new System.Windows.Forms.TextBox();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gb_FunctionItem.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tb_Other);
            this.groupBox2.Location = new System.Drawing.Point(8, 469);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(252, 86);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "其他配置";
            // 
            // tb_Other
            // 
            this.tb_Other.Location = new System.Drawing.Point(6, 18);
            this.tb_Other.Multiline = true;
            this.tb_Other.Name = "tb_Other";
            this.tb_Other.Size = new System.Drawing.Size(241, 59);
            this.tb_Other.TabIndex = 0;
            // 
            // bt_Exit
            // 
            this.bt_Exit.Location = new System.Drawing.Point(460, 523);
            this.bt_Exit.Name = "bt_Exit";
            this.bt_Exit.Size = new System.Drawing.Size(103, 28);
            this.bt_Exit.TabIndex = 12;
            this.bt_Exit.Text = "保存并退出";
            this.bt_Exit.UseVisualStyleBackColor = true;
            this.bt_Exit.Click += new System.EventHandler(this.bt_Exit_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb_Check);
            this.groupBox1.Controls.Add(this.tb_ItemFriendname);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tb_afterDelayTime);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.tb_Delaytimes);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.tb_limitMin);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.tb_limitMax);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.tb_unit);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tb_ItemName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(8, 355);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(559, 110);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "测试项目配置";
            // 
            // cb_Check
            // 
            this.cb_Check.AutoSize = true;
            this.cb_Check.Location = new System.Drawing.Point(470, 53);
            this.cb_Check.Name = "cb_Check";
            this.cb_Check.Size = new System.Drawing.Size(72, 16);
            this.cb_Check.TabIndex = 2;
            this.cb_Check.Text = "检查Fail";
            this.cb_Check.UseVisualStyleBackColor = true;
            // 
            // tb_ItemFriendname
            // 
            this.tb_ItemFriendname.Location = new System.Drawing.Point(275, 20);
            this.tb_ItemFriendname.Name = "tb_ItemFriendname";
            this.tb_ItemFriendname.Size = new System.Drawing.Size(183, 21);
            this.tb_ItemFriendname.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(215, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "项目名称";
            // 
            // tb_afterDelayTime
            // 
            this.tb_afterDelayTime.Location = new System.Drawing.Point(251, 77);
            this.tb_afterDelayTime.Name = "tb_afterDelayTime";
            this.tb_afterDelayTime.Size = new System.Drawing.Size(50, 21);
            this.tb_afterDelayTime.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(157, 81);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "测试后Delay(ms)";
            // 
            // tb_Delaytimes
            // 
            this.tb_Delaytimes.Location = new System.Drawing.Point(102, 77);
            this.tb_Delaytimes.Name = "tb_Delaytimes";
            this.tb_Delaytimes.Size = new System.Drawing.Size(50, 21);
            this.tb_Delaytimes.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 81);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "测试前Delay(ms)";
            // 
            // tb_limitMin
            // 
            this.tb_limitMin.Location = new System.Drawing.Point(38, 48);
            this.tb_limitMin.Name = "tb_limitMin";
            this.tb_limitMin.Size = new System.Drawing.Size(168, 21);
            this.tb_limitMin.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 52);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "下限";
            // 
            // tb_limitMax
            // 
            this.tb_limitMax.Location = new System.Drawing.Point(275, 48);
            this.tb_limitMax.Name = "tb_limitMax";
            this.tb_limitMax.Size = new System.Drawing.Size(168, 21);
            this.tb_limitMax.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(222, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "上限";
            // 
            // tb_unit
            // 
            this.tb_unit.Location = new System.Drawing.Point(497, 20);
            this.tb_unit.Name = "tb_unit";
            this.tb_unit.Size = new System.Drawing.Size(51, 21);
            this.tb_unit.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(464, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "单位";
            // 
            // tb_ItemName
            // 
            this.tb_ItemName.Enabled = false;
            this.tb_ItemName.Location = new System.Drawing.Point(38, 20);
            this.tb_ItemName.Name = "tb_ItemName";
            this.tb_ItemName.Size = new System.Drawing.Size(168, 21);
            this.tb_ItemName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "项目";
            // 
            // bt_Save
            // 
            this.bt_Save.Location = new System.Drawing.Point(460, 479);
            this.bt_Save.Name = "bt_Save";
            this.bt_Save.Size = new System.Drawing.Size(103, 28);
            this.bt_Save.TabIndex = 9;
            this.bt_Save.Text = "更新";
            this.bt_Save.UseVisualStyleBackColor = true;
            this.bt_Save.Click += new System.EventHandler(this.bt_Save_Click);
            // 
            // gb_FunctionItem
            // 
            this.gb_FunctionItem.Controls.Add(this.lst_Test_selectFunction);
            this.gb_FunctionItem.Controls.Add(this.label1);
            this.gb_FunctionItem.Controls.Add(this.lbl_installFunction);
            this.gb_FunctionItem.Controls.Add(this.cmd_down);
            this.gb_FunctionItem.Controls.Add(this.cmd_up);
            this.gb_FunctionItem.Controls.Add(this.cmd_delete);
            this.gb_FunctionItem.Controls.Add(this.cmd_insert);
            this.gb_FunctionItem.Controls.Add(this.lst_installFunction);
            this.gb_FunctionItem.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_FunctionItem.Location = new System.Drawing.Point(8, 3);
            this.gb_FunctionItem.Name = "gb_FunctionItem";
            this.gb_FunctionItem.Size = new System.Drawing.Size(559, 346);
            this.gb_FunctionItem.TabIndex = 10;
            this.gb_FunctionItem.TabStop = false;
            this.gb_FunctionItem.Text = "测试项目选项";
            // 
            // lst_Test_selectFunction
            // 
            this.lst_Test_selectFunction.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lst_Test_selectFunction.ForeColor = System.Drawing.Color.Blue;
            this.lst_Test_selectFunction.FullRowSelect = true;
            this.lst_Test_selectFunction.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lst_Test_selectFunction.Location = new System.Drawing.Point(322, 34);
            this.lst_Test_selectFunction.MultiSelect = false;
            this.lst_Test_selectFunction.Name = "lst_Test_selectFunction";
            this.lst_Test_selectFunction.Size = new System.Drawing.Size(226, 305);
            this.lst_Test_selectFunction.TabIndex = 4;
            this.lst_Test_selectFunction.UseCompatibleStateImageBehavior = false;
            this.lst_Test_selectFunction.View = System.Windows.Forms.View.Details;
            this.lst_Test_selectFunction.SelectedIndexChanged += new System.EventHandler(this.lst_Test_selectFunction_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 150;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(305, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "选择后待测试项目";
            // 
            // lbl_installFunction
            // 
            this.lbl_installFunction.AutoSize = true;
            this.lbl_installFunction.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_installFunction.Location = new System.Drawing.Point(20, 17);
            this.lbl_installFunction.Name = "lbl_installFunction";
            this.lbl_installFunction.Size = new System.Drawing.Size(85, 15);
            this.lbl_installFunction.TabIndex = 3;
            this.lbl_installFunction.Text = "原始测试项目";
            // 
            // cmd_down
            // 
            this.cmd_down.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmd_down.Location = new System.Drawing.Point(266, 272);
            this.cmd_down.Name = "cmd_down";
            this.cmd_down.Size = new System.Drawing.Size(48, 27);
            this.cmd_down.TabIndex = 1;
            this.cmd_down.Text = "↓";
            this.cmd_down.UseVisualStyleBackColor = true;
            this.cmd_down.Click += new System.EventHandler(this.cmd_down_Click);
            // 
            // cmd_up
            // 
            this.cmd_up.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmd_up.Location = new System.Drawing.Point(266, 205);
            this.cmd_up.Name = "cmd_up";
            this.cmd_up.Size = new System.Drawing.Size(48, 27);
            this.cmd_up.TabIndex = 1;
            this.cmd_up.Text = "↑";
            this.cmd_up.UseVisualStyleBackColor = true;
            this.cmd_up.Click += new System.EventHandler(this.cmd_up_Click);
            // 
            // cmd_delete
            // 
            this.cmd_delete.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmd_delete.Location = new System.Drawing.Point(266, 143);
            this.cmd_delete.Name = "cmd_delete";
            this.cmd_delete.Size = new System.Drawing.Size(48, 27);
            this.cmd_delete.TabIndex = 1;
            this.cmd_delete.Text = "<--";
            this.cmd_delete.UseVisualStyleBackColor = true;
            this.cmd_delete.Click += new System.EventHandler(this.cmd_delete_Click);
            // 
            // cmd_insert
            // 
            this.cmd_insert.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmd_insert.Location = new System.Drawing.Point(266, 85);
            this.cmd_insert.Name = "cmd_insert";
            this.cmd_insert.Size = new System.Drawing.Size(48, 27);
            this.cmd_insert.TabIndex = 1;
            this.cmd_insert.Text = "-->";
            this.cmd_insert.UseVisualStyleBackColor = true;
            this.cmd_insert.Click += new System.EventHandler(this.cmd_insert_Click);
            // 
            // lst_installFunction
            // 
            this.lst_installFunction.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lst_installFunction.ForeColor = System.Drawing.Color.Blue;
            this.lst_installFunction.FormattingEnabled = true;
            this.lst_installFunction.ItemHeight = 15;
            this.lst_installFunction.Location = new System.Drawing.Point(5, 35);
            this.lst_installFunction.Name = "lst_installFunction";
            this.lst_installFunction.Size = new System.Drawing.Size(255, 304);
            this.lst_installFunction.TabIndex = 0;
            this.lst_installFunction.SelectedIndexChanged += new System.EventHandler(this.lst_installFunction_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tb_Remark);
            this.groupBox3.Location = new System.Drawing.Point(266, 468);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(188, 87);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "其他配置说明";
            // 
            // tb_Remark
            // 
            this.tb_Remark.Location = new System.Drawing.Point(6, 19);
            this.tb_Remark.Multiline = true;
            this.tb_Remark.Name = "tb_Remark";
            this.tb_Remark.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_Remark.Size = new System.Drawing.Size(172, 59);
            this.tb_Remark.TabIndex = 0;
            // 
            // ItemConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(571, 562);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.bt_Exit);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.bt_Save);
            this.Controls.Add(this.gb_FunctionItem);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ItemConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "测试项目配置";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ItemConfig_FormClosed);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gb_FunctionItem.ResumeLayout(false);
            this.gb_FunctionItem.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tb_Other;
        private System.Windows.Forms.Button bt_Exit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tb_ItemFriendname;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_afterDelayTime;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tb_Delaytimes;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb_limitMin;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_limitMax;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_unit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_ItemName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bt_Save;
        private System.Windows.Forms.GroupBox gb_FunctionItem;
        private System.Windows.Forms.ListView lst_Test_selectFunction;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_installFunction;
        private System.Windows.Forms.Button cmd_down;
        private System.Windows.Forms.Button cmd_up;
        private System.Windows.Forms.Button cmd_delete;
        private System.Windows.Forms.Button cmd_insert;
        private System.Windows.Forms.ListBox lst_installFunction;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox tb_Remark;
        private System.Windows.Forms.CheckBox cb_Check;
    }
}