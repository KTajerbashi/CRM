﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using BLL;
using BEE;
using System.Runtime.Remoting.Messaging;

namespace CRM
{
    public partial class ActivityForm : Form
    {
        #region CodeRound
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        );
        #endregion

        public ActivityForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        Customer C = new Customer();
        User U = new User();
        ActivityCategory ACC = new ActivityCategory();
        
        ActivityBLL BLL = new ActivityBLL();

        CustomerBLL CustomerBLL = new CustomerBLL();
        UserBLL UserBLL = new UserBLL();
        ActivityCategoryBLL ActivityCategoryBLL = new ActivityCategoryBLL();
        ReminderBLL ReminderBLL = new ReminderBLL();

        

        MSGClass MSG = new MSGClass();

        int ID = 0;
        bool SW = false;    
        public void ShowDGV()
        {
            DGV.DataSource = null;
            DGV.DataSource = BLL.ReadAll();
            DGV.Columns["آیدی"].Visible = false;
        }
        public void DGVSearch()
        {
            DGV.DataSource = null;
            DGV.DataSource = BLL.ReadSearch(SearchTXT.Text);
            DGV.Columns["آیدی"].Visible = false;
        }

        private void ActivityForm_Load(object sender, EventArgs e)
        {
            AutoCompleteStringCollection CustomersName = new AutoCompleteStringCollection();
            AutoCompleteStringCollection UsersName = new AutoCompleteStringCollection();
            AutoCompleteStringCollection ActivityName = new AutoCompleteStringCollection();

            foreach (var i in CustomerBLL.ReadCustomerByPhone())
            {
                CustomersName.Add(i);
            }
            foreach (var i in UserBLL.ReadUserbyUserName())
            {
                UsersName.Add(i);
            }
            foreach (var i in ActivityCategoryBLL.ReadActivityByName())
            {
                ActivityName.Add(i);
            }

            CustomerTXT.AutoCompleteCustomSource = CustomersName;
            UserNameTXT.AutoCompleteCustomSource = UsersName;
            ActivityTXT.AutoCompleteCustomSource = ActivityName;

            ShowDGV();
        }

        private void xuiButton5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SearchTXT_TextChanged(object sender, EventArgs e)
        {
            DGVSearch();
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            Activity activity = new Activity();
            activity.Title=TitleTXT.Text;
            activity.Info = InfoTXT.Text;
            activity.RegDate = DateTime.Now;
            activity.ReminderDate = DateTXT.Value;

            if (ReminderCheck.Checked)
            {
                //  یاد آور ذخیره شود
                Reminder reminder = new Reminder();
                reminder.Title = TitleTXT.Text;
                reminder.ReminderInfo = InfoTXT.Text;
                reminder.RegDate = DateTime.Now;
                reminder.ReminderDate = DateTXT.Value;
                reminder.IsDone = false;
                if (ReminderBLL.Create(reminder,U))
                {
                    MSG.ShowMSGBoxDialog("ثبت اطلاعات","یادآور با موفقیت ثبت شد","",1,2);
                }
                else
                {
                    MSG.ShowMSGBoxDialog("خطای ثبت اطلاعات", "یادآور ذخیره نشد", "", 3, 1);
                }

            }
            if (BLL.Create(activity, C, U, ACC))
            {
                MSG.ShowMSGBoxDialog("ثبت اطلاعات","فعالیت با موفقیت ذخیره شد","",1,2);
            }
            else
            {
                MSG.ShowMSGBoxDialog("خطای ثبت اطلاعات", "فعالیت ذخیره نشد", "", 3, 1);
            }
            ShowDGV();
        }

        private void SaveBtn1_Click(object sender, EventArgs e)
        {
            C = CustomerBLL.ReadByPhone(CustomerTXT.Text);
            CustomerTXT.Enabled = false;
        }

        private void SaveBtn2_Click(object sender, EventArgs e)
        {
            U = UserBLL.ReadByUserName(UserNameTXT.Text);
            UserNameTXT.Enabled = false;
        }

        private void SaveBtn3_Click(object sender, EventArgs e)
        {
            ACC = ActivityCategoryBLL.ReadByName(ActivityTXT.Text);
            ActivityTXT.Enabled = false;
        }

        private void DGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ID = Convert.ToInt32(DGV.Rows[DGV.CurrentRow.Index].Cells["آیدی"].Value);
        }

        private void DGV_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button==MouseButtons.Right)
            {
                contextMenuStrip1.Show(Cursor.Position.X,Cursor.Position.Y);
            }
        }

        private void ویرایشToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SW = false;
            Activity activity = BLL.ReadByID(ID);
            TitleTXT.Text = activity.Title;
            InfoTXT.Text = activity.Info;
            DateTXT.Value = activity.ReminderDate;
        }

        private void حذفToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult d = MSG.ShowMSGBoxDialog("حذف اطلاعات", "آیا میخواهید اطلاعات مورد نظر حذف شود؟", "",2,1);

            if (d==DialogResult.Yes)
            {
                BLL.Delete(ID);
            }
        }

        private void نمایشجزییاتToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Activity activity = BLL.ReadByID(ID);
            
            MSG.ShowMSGBoxDialog("نمایش اطلاعات", activity.Info, "", 1, 1);
        }

        private void تغییرحالتToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
    }
}
