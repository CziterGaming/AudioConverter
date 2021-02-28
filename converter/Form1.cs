using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using NAudio.Wave;
using NAudio.Lame;
using System.Diagnostics;
using Microsoft.Win32;

namespace converter
{
    public partial class Form1 : Form
    {
        string FileName;
        string InputFilePath;
        string OutputFilePath;
        bool isPanel1open = false;
        bool isPanel2open = false;
        string InputFilePathWithoutFile;
        public bool isOpenFolderEnabled;

        public Form1()
        {
            InitializeComponent();

            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\CziterGaming", true);
            if (key == null)
            {
                RegistryKey regkey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\CziterGaming", true);
                regkey.SetValue("isPatchNotesHidden", "0");
                regkey.SetValue("isOpenFolderEnabled", "1");
                regkey.Close();
            }

            try
            {
                using (RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\CziterGaming\\", true))
                {
                    if (rk.GetValue("isPatchNotesHidden").ToString() == "1")
                    {
                        Console.WriteLine("Patch Notes closed.");
                    }
                    else
                    {
                        openForm3();
                    }
                    rk.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if(ex.ToString() == "")
                {
                    openForm3();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Audio files (*.mp3) (*.wav)|*.mp3;*.wav|All files (*.*)|*.*";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    InputFilePath = openFileDialog.FileName;
                    InputFilePathWithoutFile = Path.GetDirectoryName(InputFilePath) + "/";
                    FileName = Path.GetFileNameWithoutExtension(InputFilePath);
                }
            }
            textBox1.Text = InputFilePath;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                OutputFilePath = fbd.SelectedPath + "/";
            }
            textBox2.Text = OutputFilePath;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("No input file selected.");
                return;
            }
            else if (String.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("No output folder selected.");
                return;
            }

            if (radioButton1.Checked)
            {
                string FileNameWithExtension = OutputFilePath + FileName + ".wav";
                using (MediaFoundationReader mfr = new MediaFoundationReader(InputFilePath))
                {
                    WaveFileWriter.CreateWaveFile(FileNameWithExtension, mfr);
                }
            }
            else if (radioButton2.Checked)
            {
                FileStream fs = new FileStream(InputFilePath, FileMode.Open, FileAccess.Read);
                MemoryStream ms = new MemoryStream();
                fs.CopyTo(ms);

                string FileNameWithExtension = OutputFilePath + FileName + ".mp3";

                ConvertWavStreamToMp3File(ref ms, FileNameWithExtension);
            }
            else
            {
                MessageBox.Show("Select output format.");
            }

            try
            {
                using (RegistryKey regk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\CziterGaming\\", true))
                {
                    if (regk.GetValue("isOpenFolderEnabled").ToString() == "1")
                    {
                        isOpenFolderEnabled = true;
                    }
                    else if (regk.GetValue("isOpenFolderEnabled").ToString() == "0")
                    {
                        isOpenFolderEnabled = false;
                    }
                    else
                    {
                        MessageBox.Show("Error");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            if (isOpenFolderEnabled)
            {
                Process.Start(OutputFilePath);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(!isPanel1open)
            {
                panel1.Height = 64;
                isPanel1open = true;
            }
            else
            {
                panel1.Height = 22;
                isPanel1open = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (!isPanel2open)
            {
                panel2.Height = 42;
                isPanel2open = true;
            }
            else
            {
                panel2.Height = 22;
                isPanel2open = false;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Made by CziterGaming©");
        }

        public static void ConvertWavStreamToMp3File(ref MemoryStream ms, string savetofilename)
        {
            ms.Seek(0, SeekOrigin.Begin);

            using (var retMs = new MemoryStream())
            using (var rdr = new WaveFileReader(ms))
            using (var wtr = new LameMP3FileWriter(savetofilename, rdr.WaveFormat, LAMEPreset.VBR_90))
            {
                rdr.CopyTo(wtr);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Input is empty.");
            }
            else
            {
                OutputFilePath = InputFilePathWithoutFile;
                textBox2.Text = InputFilePathWithoutFile;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3(isOpenFolderEnabled);
            f3.ShowDialog();
        }

        public static void openForm3()
        {
            Form2 f2 = new Form2();
            f2.ShowDialog();
        }
    }
}
