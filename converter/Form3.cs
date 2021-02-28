using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace converter
{
    public partial class Form3 : Form
    {
        bool isPatchNotesHidden;

        public Form3(bool isOpenFolderEnabled)
        {
            InitializeComponent();
            try
            {
                using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\CziterGaming\"))
                {
                    if (rk.GetValue("isPatchNotesHidden").ToString() == "1")
                    {
                        isPatchNotesHidden = true;
                    }
                    rk.Close();
                }
                using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\CziterGaming\"))
                {
                    if (rk.GetValue("isOpenFolderEnabled").ToString() == "1")
                    {
                        isOpenFolderEnabled = true;
                    }
                    else if (rk.GetValue("isOpenFolderEnabled").ToString() == "0")
                    {
                        isOpenFolderEnabled = false;
                    }
                    else
                    {
                        MessageBox.Show("Error.");
                    }
                    rk.Close();
                }
            }
            catch (Exception ex)
            {
                isPatchNotesHidden = false;
                Console.WriteLine(ex);
            }
            Console.WriteLine(isOpenFolderEnabled);

            if (isPatchNotesHidden == true)
            {
                label2.Text = "hidden.";
            }
            else
            {
                label2.Text = "visible.";
            }

            if (isOpenFolderEnabled == true)
            {
                label4.Text = "true.";
            }
            else
            {
                label4.Text = "false.";
            }
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            using (RegistryKey regkey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\CziterGaming\\", true))
            {
                if (regkey.GetValue("isPatchNotesHidden").ToString() == "1")
                {
                    regkey.SetValue("isPatchNotesHidden", "0", RegistryValueKind.String);
                    label2.Text = "visible.";
                }
                else if (regkey.GetValue("isPatchNotesHidden").ToString() == "0")
                {
                    regkey.SetValue("isPatchNotesHidden", "1", RegistryValueKind.String);
                    label2.Text = "hidden.";
                }
                else
                {
                    MessageBox.Show("Error.");
                }
                regkey.Close();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (RegistryKey regkey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\CziterGaming\\", true))
            {
                if (regkey.GetValue("isOpenFolderEnabled").ToString() == "1")
                {
                    regkey.SetValue("isOpenFolderEnabled", "0", RegistryValueKind.String);
                    label4.Text = "false.";
                }
                else if (regkey.GetValue("isOpenFolderEnabled").ToString() == "0")
                {
                    regkey.SetValue("isOpenFolderEnabled", "1", RegistryValueKind.String);
                    label4.Text = "true.";
                }
                else
                {
                    MessageBox.Show("Error.");
                }
                regkey.Close();
            }
        }
    }
}
