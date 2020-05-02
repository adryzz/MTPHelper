using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediaDevices;

namespace MTPHelper
{
    public partial class Form1 : Form
    {
        List<MediaDevice> Devices = new List<MediaDevice>();
        MediaDevice Device;
        MediaDriveInfo Drive;
        MediaDriveInfo[] Drives;
        Settings Settings = new Settings(true);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists("config.xml"))
            {
                Settings = Settings.FromFile("config.xml");
            }
            else
            {
                Settings.Save("config.xml");
            }
            foreach(MediaDevice d in MediaDevice.GetDevices())
            {
                Devices.Add(d);
                comboBox1.Items.Add(d.FriendlyName);
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {

        }

        private void Form1_Resize(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var res = MessageBox.Show("Do you really wanna exit?", "MTPHelper", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Devices.Clear();
            comboBox1.Items.Clear();
            foreach (MediaDevice d in MediaDevice.GetDevices())
            {
                Devices.Add(d);
                comboBox1.Items.Add(d.FriendlyName);
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (Device == null)
            {
                Device = Devices.ElementAt(comboBox1.SelectedIndex);
                Device.Connect();
                Device.DeviceRemoved += Device_DeviceRemoved;
                button2.Text = "Disconnect";
                Drives = Device.GetDrives();
                foreach(MediaDriveInfo d in Drives)
                {
                    comboBox2.Items.Add(d.VolumeLabel);
                }
            }
            else if (Device.IsConnected)
            {
                Device.Disconnect();
                Device = null;
                button2.Text = "Connect";
                Drive = null;
                Drives = null;
                comboBox2.Items.Clear();
            }
            else
            {
                Device = Devices.ElementAt(comboBox1.SelectedIndex);
                Device.Connect();
                Device.DeviceRemoved += Device_DeviceRemoved;
                button2.Text = "Disconnect";
                Drives = Device.GetDrives();
                if (Drives == null)
                {
                    return;
                }
                if (Drives.Length == 0)
                {
                    return;
                }
                foreach (MediaDriveInfo d in Drives)
                {
                    comboBox2.Items.Add(d.VolumeLabel);
                }
            }
        }

        private void Device_DeviceRemoved(object sender, MediaDeviceEventArgs e)
        {
            MessageBox.Show(Device.FriendlyName + " has been disconnected unexpectedly", "MTPHelper", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Device = null;
            button2.Text = "Connect";
            Devices.Clear();
            comboBox1.Items.Clear();
            foreach (MediaDevice d in MediaDevice.GetDevices())
            {
                Devices.Add(d);
                comboBox1.Items.Add(d.FriendlyName);
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            InfoViewer i = new InfoViewer(Device);
            i.Show(this);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            var res = folderBrowserDialog1.ShowDialog();
            if (res != DialogResult.OK)
            {
                return;
            }
            new Thread(new ThreadStart(SaveImages)).Start();
        }//images

        private void Button5_Click(object sender, EventArgs e)
        {
            var res = folderBrowserDialog1.ShowDialog();
            if (res != DialogResult.OK)
            {
                return;
            }
            new Thread(new ThreadStart(SaveSounds)).Start();
        }//sounds

        private void Button6_Click(object sender, EventArgs e)
        {
            var res = folderBrowserDialog1.ShowDialog();
            if (res != DialogResult.OK)
            {
                return;
            }
            new Thread(new ThreadStart(SaveVideos)).Start();
        }//videos

        private void Button7_Click(object sender, EventArgs e)
        {
            var res = folderBrowserDialog1.ShowDialog();
            if (res != DialogResult.OK)
            {
                return;
            }
            new Thread(new ThreadStart(SaveAll)).Start();
        }//all

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Drive = Drives[comboBox2.SelectedIndex];
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            Drive = null;
            Drives = Device.GetDrives();
            if (Drives == null)
            {
                return;
            }
            if (Drives.Length == 0)
            {
                return;
            }
            foreach (MediaDriveInfo d in Drives)
            {
                comboBox2.Items.Add(d.VolumeLabel);
            }
        }

        private void SaveImages()
        {
            string path = folderBrowserDialog1.SelectedPath;

            List<MediaFileInfo> files = Search(Settings.ImageTypes.ToArray());

            if (radioButton1.Checked)
            {
                SaveMode1(files, path);
            }
            else if (radioButton2.Checked)
            {
                SaveMode2(files, path);
            }
            else if (radioButton3.Checked)
            {
                SaveMode3(files, path);
            }
        }

        private void SaveSounds()
        {
            string path = folderBrowserDialog1.SelectedPath;

            List<MediaFileInfo> files = Search(Settings.SoundTypes.ToArray());

            if (radioButton1.Checked)
            {
                SaveMode1(files, path);
            }
            else if (radioButton2.Checked)
            {
                SaveMode2(files, path);
            }
            else if (radioButton3.Checked)
            {
                SaveMode3(files, path);
            }
        }

        private void SaveVideos()
        {
            string path = folderBrowserDialog1.SelectedPath;

            List<MediaFileInfo> files = Search(Settings.VideoTypes.ToArray());

            if (radioButton1.Checked)
            {
                SaveMode1(files, path);
            }
            else if (radioButton2.Checked)
            {
                SaveMode2(files, path);
            }
            else if (radioButton3.Checked)
            {
                SaveMode3(files, path);
            }
        }

        private void SaveAll()
        {
            string path = folderBrowserDialog1.SelectedPath;

            List<MediaFileInfo> files = Search(Settings.ImageTypes.Concat(Settings.SoundTypes).Concat(Settings.VideoTypes).ToArray());

            if (radioButton1.Checked)
            {
                SaveMode1(files, path);
            }
            else if (radioButton2.Checked)
            {
                SaveMode2(files, path);
            }
            else if (radioButton3.Checked)
            {
                SaveMode3(files, path);
            }
        }

        #region Delegates and stuff

        private delegate void SetBarValueDelegate(int value);

        private delegate void SetBarStyleDelegate(ProgressBarStyle style);

        private delegate void SetTextDelegate(string text);

        private void SetBarValue(int value)
        {
            progressBar1.Value = value;
        }

        private void SetBarMaxValue(int value)
        {
            progressBar1.Maximum = value;
        }

        private void SetBarStyle(ProgressBarStyle style)
        {
            progressBar1.Style = style;
        }

        private void SetText(string text)
        {
            label1.Text = text;
        }

        #endregion

        #region Search and save

        /// <summary>
        /// Searches in a MTP device for all files with one or more specific extension(s)
        /// </summary>
        /// <param name="extensions">The extension in the format "*.*"</param>
        /// <returns></returns>
        private List<MediaFileInfo> Search(params string[] extensions)
        {
            string path = folderBrowserDialog1.SelectedPath;
            progressBar1.Invoke(new SetBarValueDelegate(SetBarValue), 0);
            List<MediaFileInfo> files = new List<MediaFileInfo>();
            int count = 0;
            progressBar1.Invoke(new SetBarStyleDelegate(SetBarStyle), ProgressBarStyle.Marquee);
            for (int i = 0; i < extensions.Length; i++)
            {
                try
                {
                    foreach (MediaFileInfo m in Drive.RootDirectory.EnumerateFiles(extensions[i], SearchOption.AllDirectories))
                    {
                        files.Add(m);
                        count++;
                        label1.Invoke(new SetTextDelegate(SetText), string.Format("Speed: {0}\nFiles copied: {1}/{2}", "unknown", 0, count));
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return files;
        }

        private void SaveMode1(List<MediaFileInfo> files, string path)
        {
            progressBar1.Invoke(new SetBarStyleDelegate(SetBarStyle), ProgressBarStyle.Blocks);

            progressBar1.Invoke(new SetBarValueDelegate(SetBarMaxValue), files.Count);

            label1.Invoke(new SetTextDelegate(SetText), string.Format("Speed: {0}\nFiles copied: {1}/{2}", "unknown", 0, files.Count));
            foreach (MediaFileInfo m in files)
            {
                string filePath = Path.Combine(path, Guid.NewGuid().ToString() + Path.GetExtension(m.Name));

                FileStream s = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write);

                Device.DownloadFile(m.FullName, s);

                //Utils.WriteSreamToDisk(diskPath, s);
                s.Flush();
                s.Close();
                progressBar1.Invoke(new SetBarValueDelegate(SetBarValue), progressBar1.Value + 1);
                label1.Invoke(new SetTextDelegate(SetText), string.Format("Speed: {0}\nFiles copied: {1}/{2}", "unknown", progressBar1.Value, files.Count));
                label1.Text = string.Format("Speed: {0}\nFiles copied: {1}/{2}", "unknown", progressBar1.Value, files.Count);
            }
        }

        private void SaveMode2(List<MediaFileInfo> files, string path)
        {
            progressBar1.Invoke(new SetBarStyleDelegate(SetBarStyle), ProgressBarStyle.Blocks);

            progressBar1.Invoke(new SetBarValueDelegate(SetBarMaxValue), files.Count);

            label1.Invoke(new SetTextDelegate(SetText), string.Format("Speed: {0}\nFiles copied: {1}/{2}", "unknown", 0, files.Count));
            foreach (MediaFileInfo m in files)
            {
                string filePath = Path.Combine(path, m.Name);

                int i = 1;
                while (File.Exists(filePath))
                {
                    filePath = Path.GetFileNameWithoutExtension(filePath) + " (" + i + ")" + Path.GetExtension(filePath);
                    i++;
                }

                FileStream s = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write);

                Device.DownloadFile(m.FullName, s);

                //Utils.WriteSreamToDisk(diskPath, s);
                s.Flush();
                s.Close();
                progressBar1.Invoke(new SetBarValueDelegate(SetBarValue), progressBar1.Value + 1);
                label1.Invoke(new SetTextDelegate(SetText), string.Format("Speed: {0}\nFiles copied: {1}/{2}", "unknown", progressBar1.Value, files.Count));
                label1.Text = string.Format("Speed: {0}\nFiles copied: {1}/{2}", "unknown", progressBar1.Value, files.Count);
            }
        }

        private void SaveMode3(List<MediaFileInfo> files, string path)
        {
            progressBar1.Invoke(new SetBarStyleDelegate(SetBarStyle), ProgressBarStyle.Blocks);

            progressBar1.Invoke(new SetBarValueDelegate(SetBarMaxValue), files.Count);

            label1.Invoke(new SetTextDelegate(SetText), string.Format("Speed: {0}\nFiles copied: {1}/{2}", "unknown", 0, files.Count));
            foreach (MediaFileInfo m in files)
            {
                string filePath = Path.Combine(path, m.Name);

                FileStream s = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write);

                Device.DownloadFile(m.FullName, s);

                //Utils.WriteSreamToDisk(diskPath, s);
                s.Flush();
                s.Close();
                progressBar1.Invoke(new SetBarValueDelegate(SetBarValue), progressBar1.Value + 1);
                label1.Invoke(new SetTextDelegate(SetText), string.Format("Speed: {0}\nFiles copied: {1}/{2}", "unknown", progressBar1.Value, files.Count));
                label1.Text = string.Format("Speed: {0}\nFiles copied: {1}/{2}", "unknown", progressBar1.Value, files.Count);
            }
        }

        #endregion

        private void Button9_Click(object sender, EventArgs e)
        {

        }
    }
}
