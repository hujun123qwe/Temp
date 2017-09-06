using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonitoringCableTmp
{
    public partial class frmAlarm : Form
    {
        public frmAlarm()
        {
            InitializeComponent();
            LoadAlarmLog();
        }

        private void LoadAlarmLog()
        {
            string FILE_DIR = Directory.GetCurrentDirectory();
            DirectoryInfo difo = Directory.GetParent(FILE_DIR);
            MyFile myFile = new MyFile("alarm.txt", difo.ToString() + "\\Alarm");
            ArrayList lines = myFile.readFile();
            lines.Reverse();
            foreach (string line in lines)
            {
                txtAlarm.Text += line + "\r\n";
            }


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
