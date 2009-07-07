using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ISUtils;
using ISUtils.Common;

namespace IndexEditor
{
    public partial class frmFields : Form
    {
        #region Private Vars
        private TextBox tbFields;
        private List<FieldProperties> fpList=new List<FieldProperties>();
        #endregion
        #region Property
        public List<FieldProperties> Fields
        {
            get { return fpList; }
            set { fpList = value; }
        }
        public string StringFields
        {
            get 
            {
                if (fpList == null)
                    fpList = new List<FieldProperties>();
                StringBuilder buffer = new StringBuilder();
                foreach (FieldProperties fp in fpList)
                {
                    buffer.Append(fp.ToString() + ",");
                }
                if (buffer.Length > 0)
                    buffer.Remove(buffer.Length - 1, 1);
                return buffer.ToString();
            }
            set
            {
                if (fpList == null)
                    fpList = new List<FieldProperties>();
                if (value.IndexOf(')') > 0)
                {
                    string[] split = SupportClass.String.Split(value, ")");
                    foreach (string token in split)
                        fpList.Add(new FieldProperties(token));
                }
                else
                {
                    string[] split = SupportClass.String.Split(value, ",");
                    foreach (string token in split)
                        fpList.Add(new FieldProperties(token));
                }
            }
        }
        #endregion
        #region Constructor
        public frmFields(TextBox tbFields)
        {
            InitializeComponent();
            this.tbFields = tbFields;
            fpList.AddRange(FieldProperties.ToArray(tbFields.Text));
            UpdateListData(true);
            EnableControls(false);
        }
        #endregion
        #region Form Event
        private void frmFields_Load(object sender, EventArgs e)
        {

        }
        #endregion
        #region Public Dialog Function
        private void ShowInformation(string msg)
        {
            string caption = this.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icon = MessageBoxIcon.Information;
            MessageBoxDefaultButton defbtn = MessageBoxDefaultButton.Button1;
            DialogResult result = MessageBox.Show(this, msg, caption, buttons, icon, defbtn);
        }
        private void ShowExclamation(string msg)
        {
            string caption = this.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icon = MessageBoxIcon.Exclamation;
            MessageBoxDefaultButton defbtn = MessageBoxDefaultButton.Button1;
            DialogResult result = MessageBox.Show(this, msg, caption, buttons, icon, defbtn);
        }
        private void ShowError(string msg)
        {
            string caption = this.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icon = MessageBoxIcon.Error;
            MessageBoxDefaultButton defbtn = MessageBoxDefaultButton.Button1;
            DialogResult result = MessageBox.Show(this, msg, caption, buttons, icon, defbtn);
        }
        private void ShowWarning(string msg)
        {
            string caption = this.Text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icon = MessageBoxIcon.Warning;
            MessageBoxDefaultButton defbtn = MessageBoxDefaultButton.Button1;
            DialogResult result = MessageBox.Show(this, msg, caption, buttons, icon, defbtn);
        }
        private bool ShowQuestion(string msg)
        {
            string caption = this.Text;
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            MessageBoxIcon icon = MessageBoxIcon.Question;
            MessageBoxDefaultButton defbtn = MessageBoxDefaultButton.Button1;
            DialogResult result = MessageBox.Show(this, msg, caption, buttons, icon, defbtn);
            return result == DialogResult.Yes;
        }
        #endregion
        #region Control Event
        private void lbFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbFields.SelectedItems != null && lbFields.SelectedItems.Count > 0)
            {
                //message = listView.SelectedItems[0].Text+listView.SelectedItems[0].Index.ToString();
                //string message ="no item select";
                //string caption = "no server name specified";
                //MessageBoxButtons buttons = MessageBoxButtons.OK;
                //DialogResult result;
                //// displays the messagebox.
                //result = MessageBox.Show(this, message, caption, buttons,
                //MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);//,MessageBoxOptions.);
                //if (result == DialogResult.Yes)
                //{
                //    //do your action here.
                //}
                UpdateData(true);
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            EnableControls(true);
            EnableControls(btnAdd, btnEdit, btnDel, btnConfim, btnCancel);
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lbFields.SelectedIndex < 0)
                return;
            EnableControls(true);
            txtName.Enabled = false;
            EnableControls(btnAdd, btnEdit,btnDel, btnConfim, btnCancel);
        }
        private void btnDel_Click(object sender, EventArgs e)
        {
            if (lbFields.SelectedIndex < 0)
                return;
            if (ShowQuestion("确认删除吗？"))
            {
                fpList.RemoveAt(lbFields.SelectedIndex);
                UpdateListData(true);
            }
        }
        private void btnConfim_Click(object sender, EventArgs e)
        {
            FieldProperties fp = new FieldProperties();
            fp.Field = txtName.Text;
            fp.Caption = txtCaption.Text;
            fp.Boost = (float)numBoost.Value;
            fp.TitleOrContent = radioTitle.Checked;
            if (txtName.Enabled ==false)
               fpList.RemoveAt(lbFields.SelectedIndex);
            fpList.Insert(lbFields.SelectedIndex, fp);
            UpdateListData(true);
            EnableControls(btnAdd, btnEdit, btnDel, btnConfim, btnCancel);
            EnableControls(false);
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            UpdateListData(true);
            EnableControls(btnAdd, btnEdit, btnDel, btnConfim, btnCancel);
            EnableControls(false);
        }
        #endregion
        #region Data Function
        private void UpdateListData(bool update)
        {
            if (update)
            {
                int sel = lbFields.SelectedIndex;
                lbFields.Items.Clear();
                foreach (FieldProperties fp in fpList)
                {
                    lbFields.Items.Add(fp.ToString());
                }
                lbFields.SelectedIndex = sel;
            }
            else
            {
                fpList.Clear();
                foreach (string item in lbFields.Items)
                {
                    fpList.Add(new FieldProperties(item));
                }
            }
        }
        private void UpdateData(bool update)
        {
            if (lbFields.SelectedIndex < 0)
                return;
            if (update)
            {
                FieldProperties fp = fpList[lbFields.SelectedIndex];
                txtName.Text = fp.Field;
                txtCaption.Text = fp.Caption;
                numBoost.Value = (decimal) fp.Boost;
                radioTitle.Checked = fp.TitleOrContent;
                radioContent.Checked = !radioTitle.Checked;
            }
            else
            {
                fpList[lbFields.SelectedIndex].Field = txtName.Text;
                fpList[lbFields.SelectedIndex].Caption = txtCaption.Text;
                fpList[lbFields.SelectedIndex].Boost = (float)numBoost.Value;
                fpList[lbFields.SelectedIndex].TitleOrContent = radioTitle.Checked;
            }
        }
        #endregion
        #region Control Function
        private void EnableControls(params Control[] controls)
        {
            foreach (Control control in controls)
            {
                control.Enabled = !control.Enabled;
            }
        }
        private void EnableControls(bool enabled)
        {
            Control[] controls = new Control[] { txtName,
                                                txtCaption,
                                                numBoost, 
                                                radioTitle,
                                                radioContent};
            foreach (Control control in controls)
            {
                control.Enabled = enabled;
            }
        }
        #endregion
    }
}