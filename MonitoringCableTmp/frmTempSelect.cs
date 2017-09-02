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
using System.Collections;

namespace MonitoringCableTmp
{
    public partial class frmTempSelect : Form
    {
        public frmTempSelect()
        {
            InitializeComponent();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            DtsComm dtscom;
            dtscom = new DtsComm();
            Hashtable myhas = null;
            DateTime dt,dt1;
          
            dt= new DateTime(2017,9,1,14,0,0);
            dt1 = new DateTime(2017,9,1,14,3,0);
            //dt=dateTimePicker1.Value;
            if (DateTime.Compare(dt, dt1) > 0)
            {
                MessageBox.Show("选择的时间段有误，请重新选择");
            }
            myhas = dtscom.getTempDataFromTxtFileForOnePoint(1, 155, dt, dt1);
            
            //测试结果
            //经测试返回结果正常，但仅测试时间段在同一天的情况
            //foreach (DictionaryEntry dic in myhas){
            //    Console.WriteLine(dic.Value);
            //}


            tChart1.Series[0].XValues.DateTime = true;
            foreach (DictionaryEntry dic in myhas)
            {
                double dValue = Convert.ToDouble(dic.Value);
                dt1 = Convert.ToDateTime(dic.Key);
                tChart1.Series[0].Add(dt1, dValue);
            }
            
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
