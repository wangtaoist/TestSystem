﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestTool;
using TestModel;
using TestDAL;

namespace WinForm
{
    public partial class ItemConfig : Form
    {
        private string initPath;
        private DataBase dataBase;
        private List<TestData> TestItems;
        private List<InitTestItem> initTestItems;

        public ItemConfig(string path)
        {
            InitializeComponent();
            this.initPath = path;
            dataBase = new DataBase(path);
            TestItems = new List<TestData>();
            initTestItems = ConfigRead.GetInitTestItem();
            GetInitTesetItem();
            GetTestItem();
        }

        public void GetInitTesetItem()
        {
            lst_installFunction.DataSource = initTestItems.Select(s => s.TestItem).ToList();
        }

        public void GetTestItem()
        {
            TestItems = dataBase.GetTestItem();
            var item = TestItems.Select(s => s.TestItemName).ToArray();
            for (int i = 0; i < item.Count(); i++)
            {
                lst_Test_selectFunction.Items.Add(item[i]);
            }
        }

        private void lst_Test_selectFunction_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView view = (ListView)sender;
            ListView.SelectedIndexCollection collect = view.SelectedIndices;
            if (collect.Count > 0)
            {
                TestData item = TestItems[collect[0]];
                tb_ItemName.Text = item.TestItem;
                tb_ItemFriendname.Text = item.TestItemName;
                tb_unit.Text = item.Unit;
                tb_limitMax.Text = item.UppLimit;
                tb_limitMin.Text = item.LowLimit;
                tb_Delaytimes.Text = item.beferTime.ToString();
                tb_afterDelayTime.Text = item.AfterTime.ToString();
                tb_Other.Text = item.Other;
                tb_Remark.Text = item.Remark;
                cb_Check.Checked = item.Check;
                //cb_Show.Checked = item.Show;
            }
         
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection collect = lst_Test_selectFunction.SelectedIndices;
            if (collect.Count > 0)
            {
                int i = collect[0];
                TestItems[i].TestItem = tb_ItemName.Text.Trim();
                TestItems[i].TestItemName = tb_ItemFriendname.Text.Trim();
                TestItems[i].Unit = tb_unit.Text.Trim();
                TestItems[i].LowLimit = tb_limitMin.Text.Trim();
                TestItems[i].UppLimit = tb_limitMax.Text.Trim();
                TestItems[i].AfterTime = int.Parse(tb_afterDelayTime.Text.Trim());
                TestItems[i].beferTime = int.Parse(tb_Delaytimes.Text.Trim());
                TestItems[i].Remark = tb_Remark.Text.Trim();
                TestItems[i].Other = tb_Other.Text.Trim();
                TestItems[i].Check = cb_Check.Checked;
                //TestItems[i].Show = cb_Show.Checked;

                lst_Test_selectFunction.Items[i].Text = tb_ItemFriendname.Text.Trim();
            }
        }

        private void cmd_insert_Click(object sender, EventArgs e)
        {
            TestItems.Add(new TestData()
            {
                TestItem = lst_installFunction.SelectedItem.ToString(),
                TestItemName = lst_installFunction.SelectedItem.ToString()
            });
            lst_Test_selectFunction.Items.Add(lst_installFunction.SelectedItem.ToString());
            lst_Test_selectFunction.Items[lst_Test_selectFunction.Items.Count - 1].Selected = true;
            lst_Test_selectFunction.Select();
            Others.SendMessage(lst_Test_selectFunction.Handle, 0x0115, 1, 0);
        }

        private void cmd_delete_Click(object sender, EventArgs e)
        {
             ListView.SelectedIndexCollection collect = lst_Test_selectFunction.SelectedIndices;
             if (collect.Count > 0)
             {
                 int i = collect[0];
                 lst_Test_selectFunction.Items.RemoveAt(i);
                 TestItems.RemoveAt(i);
             }
        }

        private void cmd_up_Click(object sender, EventArgs e)
        {
             ListView.SelectedIndexCollection collect = lst_Test_selectFunction.SelectedIndices;
             if (collect.Count > 0)
             {
                 int i = collect[0];
                 if (i != 0)
                 {
                     TestData data = TestItems[i];
                     TestItems.RemoveAt(i);
                     lst_Test_selectFunction.Items.RemoveAt(i);
                     TestItems.Insert(i - 1, data);
                     lst_Test_selectFunction.Items.Insert(i - 1, data.TestItemName);
                     lst_Test_selectFunction.Items[i - 1].Selected = true;
                     lst_Test_selectFunction.Select();
                 }
             }
        }

        private void cmd_down_Click(object sender, EventArgs e)
        {
             ListView.SelectedIndexCollection collect = lst_Test_selectFunction.SelectedIndices;
             if (collect.Count > 0)
             {
                 int i = collect[0];
                 if (i != lst_Test_selectFunction.Items.Count)
                 {
                     TestData data = TestItems[i];
                     TestItems.RemoveAt(i);
                     lst_Test_selectFunction.Items.RemoveAt(i);
                     TestItems.Insert(i +1, data);
                     lst_Test_selectFunction.Items.Insert(i + 1, data.TestItemName);
                     lst_Test_selectFunction.Items[i + 1].Selected = true;
                     lst_Test_selectFunction.Select();
                 }
             }
        }

        private void ItemConfig_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!(this.DialogResult == System.Windows.Forms.DialogResult.Yes))
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
        }

        private void bt_Exit_Click(object sender, EventArgs e)
        {
            dataBase.DelTestItem();
            dataBase.InsterTestItem(TestItems);
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;            
        }

        private void lst_installFunction_SelectedIndexChanged(object sender, EventArgs e)
        {
            int collect = lst_installFunction.SelectedIndex;
            tb_Remark.Text = initTestItems[collect].Remark;
        }
    }
}
