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

        /// <summary>
        /// 温度历史数据读取并显示
        /// *当前进度
        ///  可以通过面板查询同一天的单点温度记录 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            //得到控件数据
            DateTime startTime = dtpStartTime.Value;
            DateTime endTime = dtpEndTime.Value;
            int ch = Convert.ToInt32(cmbChannel.SelectedIndex) + 1;
            int pointNum = Convert.ToInt32(cmbPointNum.Text);
            string stiles;
            Hashtable myhas = new Hashtable();
            DtsComm dtscom = new DtsComm();
            stiles = "通道:" + cmbChannel.Text + "<>点位置:" + cmbPointNum.Text;
            tChart1.Header.Lines = new string[] { stiles };
            tChart1.Series[0].Clear();
            myhas = dtscom.getTempDataFromTxtFileForOnePoint("CH1", pointNum, startTime, endTime);
            if (myhas != null)
            {
                tChart1.Series[0].XValues.DateTime = true;
                foreach (DictionaryEntry dic in myhas)
                {
                    double dValue = Convert.ToDouble(dic.Value);
                    endTime = Convert.ToDateTime(dic.Key);
                    tChart1.Series[0].Add(endTime, dValue);
                }
            }
            else
            {
                MessageBox.Show("该时间历史数据不存在，请重新选择时间段！");
;
            }



            /*
            //startTime与endTime不在同一天处理
            //24小时判断+隔天天数判断
            TimeSpan ts = new TimeSpan(endTime.Ticks - startTime.Ticks);
            if (ts.TotalSeconds < 0)
            {
                MessageBox.Show("选择的时间段有误，请重新选择");
            }
            //日期在同一天
            else if (Math.Floor(ts.TotalHours) <= 24 && startTime.Day == endTime.Day)
            {               
                myhas = dtscom.getTempDataFromTxtFileForOnePoint("CH1", pointNum, startTime, endTime);

                tChart1.Series[0].XValues.DateTime = true;
                foreach (DictionaryEntry dic in myhas)
                {
                    double dValue = Convert.ToDouble(dic.Value);
                    endTime = Convert.ToDateTime(dic.Key);
                    tChart1.Series[0].Add(endTime, dValue);
                }
            }
            //日期不在同一天
            //多读取几个文件，并处理
            else
            {

            }
             */
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
