namespace WinForm
{
    partial class MesWindow
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
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.label30 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.tb_NowStation = new System.Windows.Forms.TextBox();
            this.tb_Station = new System.Windows.Forms.TextBox();
            this.cb_MesEnable = new System.Windows.Forms.CheckBox();
            this.bt_Save = new System.Windows.Forms.Button();
            this.groupBox13.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.label30);
            this.groupBox13.Controls.Add(this.label28);
            this.groupBox13.Controls.Add(this.tb_NowStation);
            this.groupBox13.Controls.Add(this.tb_Station);
            this.groupBox13.Controls.Add(this.cb_MesEnable);
            this.groupBox13.Location = new System.Drawing.Point(3, 7);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(355, 54);
            this.groupBox13.TabIndex = 12;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "MES自动拦截";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(191, 29);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(41, 12);
            this.label30.TabIndex = 2;
            this.label30.Text = "现工站";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(49, 28);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(53, 12);
            this.label28.TabIndex = 2;
            this.label28.Text = "上一工站";
            // 
            // tb_NowStation
            // 
            this.tb_NowStation.Location = new System.Drawing.Point(236, 24);
            this.tb_NowStation.Name = "tb_NowStation";
            this.tb_NowStation.Size = new System.Drawing.Size(113, 21);
            this.tb_NowStation.TabIndex = 1;
            // 
            // tb_Station
            // 
            this.tb_Station.Location = new System.Drawing.Point(103, 23);
            this.tb_Station.Name = "tb_Station";
            this.tb_Station.Size = new System.Drawing.Size(87, 21);
            this.tb_Station.TabIndex = 1;
            // 
            // cb_MesEnable
            // 
            this.cb_MesEnable.AutoSize = true;
            this.cb_MesEnable.Location = new System.Drawing.Point(6, 27);
            this.cb_MesEnable.Name = "cb_MesEnable";
            this.cb_MesEnable.Size = new System.Drawing.Size(48, 16);
            this.cb_MesEnable.TabIndex = 0;
            this.cb_MesEnable.Text = "启用";
            this.cb_MesEnable.UseVisualStyleBackColor = true;
            // 
            // bt_Save
            // 
            this.bt_Save.Location = new System.Drawing.Point(139, 67);
            this.bt_Save.Name = "bt_Save";
            this.bt_Save.Size = new System.Drawing.Size(75, 23);
            this.bt_Save.TabIndex = 13;
            this.bt_Save.Text = "保存";
            this.bt_Save.UseVisualStyleBackColor = true;
            this.bt_Save.Click += new System.EventHandler(this.bt_Save_Click);
            // 
            // MesWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(361, 98);
            this.Controls.Add(this.bt_Save);
            this.Controls.Add(this.groupBox13);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MesWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MesWindow";
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox tb_NowStation;
        private System.Windows.Forms.TextBox tb_Station;
        private System.Windows.Forms.CheckBox cb_MesEnable;
        private System.Windows.Forms.Button bt_Save;
    }
}