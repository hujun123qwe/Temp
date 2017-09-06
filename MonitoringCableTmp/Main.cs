using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Steema.TeeChart.Drawing;
using Steema.TeeChart.Styles;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace MonitoringCableTmp
{
    public partial class frmMain : Form
    {
        private string FILE_DIR = Directory.GetCurrentDirectory();
        private string[] chInfo = new string[3];
        private int chLenth;
        private int[,] AlarmUPDown; //报警上下限数组
        private int[] AlarmValues;//报警值
        /// <summary>
        /// 主窗口构造函数
        /// </summary>
        public frmMain()
        {
            InitializeComponent();
            InitFrame();
        }

        /// <summary>
        /// 初始化窗口
        /// </summary>
        private void InitFrame()
        {

            IntChart1(1000);
            IntChart2(1000);
            timer1.Enabled = false;
            lblStatus.Text = "系统准备就绪";
        }
        /// <summary>
        /// 根据通道号获取报警上下限及报警值到数组
        /// </summary>
        /// <param name="ch"></param>
        private void getAlarmByCh(string ch)
        {
            string[] alarmStr;
          //  string[] stempV = tempValue.Split('_');
          //  ponitID = stempV[0];
          //  tempValue = stempV[1];
            string alarmReStr;
            dbComm dbcom;
            dbcom = new dbComm();
            alarmStr = dbcom.getParagAlarmBYChIN(ch);
            AlarmUPDown = new int[alarmStr.Length, 2];
            AlarmValues = new int[alarmStr.Length];
            for (int i=0 ;i<alarmStr.Length;i++)
            {
                alarmReStr = alarmStr[i];
                string[] stempV = alarmReStr.Split('_');
                AlarmUPDown[i, 0] =Convert.ToInt32( stempV[0]);
                AlarmUPDown[i, 1] = Convert.ToInt32(stempV[1]);
                AlarmValues[i] = Convert.ToInt32(stempV[2]); 
            }
        }
        public int AlarmValue(int ponitValues)
        {
            int alarmV=0;
            for (int i = 0; i < AlarmValues.Length; i++)
            {
                if ((ponitValues>=AlarmUPDown[i,0])&(ponitValues<=AlarmUPDown[i,1])){
                    alarmV = AlarmValues[i];
                    break;
                }
            }
            return alarmV;
        }
        /// <summary>
        /// 通道分区温度曲线
        /// </summary>
        /// <param name="length">X轴（距离）最大数值</param>
        private void IntChart1(double length)
        {
            tChart1.Legend.Visible = false;
            tChart1.Aspect.View3D = false;//3D为假
            tChart1.Walls.Visible = false;  //墙纸不见
            tChart1.Panel.Brush.Gradient.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(246)))), ((int)(((byte)(249)))));//panel 面版背景色渐变
            tChart1.Panel.Brush.Gradient.MiddleColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(231)))), ((int)(((byte)(239)))));
            tChart1.Panel.Brush.Gradient.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(178)))), ((int)(((byte)(200)))));
            tChart1.Header.Lines = new string[] { "通道分区温度曲线" };

            tChart1.Axes.Bottom.Title.Text = "(距离m)";//左标题
            tChart1.Axes.Left.Title.Text = "(温度℃)"; //右标题
           
            tChart1.Axes.Bottom.Automatic = false;//底部轴点
            tChart1.Axes.Bottom.AutomaticMaximum = false;
            tChart1.Axes.Bottom.AutomaticMinimum = false;
            tChart1.Axes.Bottom.Maximum = length;
            tChart1.Axes.Bottom.Minimum = 0;
 
            tChart1.Axes.Left.Automatic = false;//左轴点
            tChart1.Axes.Left.AutomaticMaximum = false;
            tChart1.Axes.Left.AutomaticMinimum = false;
            tChart1.Axes.Left.Maximum = 120;
            tChart1.Axes.Left.Minimum = 0;

        }
        /// <summary>
        /// 通道温度曲线总览
        /// </summary>
        /// <param name="length">X轴（距离）最多数值</param>
        private void IntChart2(double length)
        {
            tChart2.Legend.Visible = false;
            tChart2.Aspect.View3D = false;//3D为假
            tChart2.Walls.Visible = false;  //墙纸不见
            tChart2.Panel.Brush.Gradient.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(246)))), ((int)(((byte)(249)))));//panel 面版背景色渐变
            tChart2.Panel.Brush.Gradient.MiddleColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(231)))), ((int)(((byte)(239)))));
            tChart2.Panel.Brush.Gradient.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(178)))), ((int)(((byte)(200)))));
            tChart2.Header.Lines = new string[] { "通道温度总览曲线" };

            tChart2.Axes.Bottom.Title.Text = "(距离m)";//左标题
            tChart2.Axes.Left.Title.Text = "(温度℃)"; //右标题
      
            tChart2.Axes.Bottom.Automatic = false;//底部轴点
            tChart2.Axes.Bottom.AutomaticMaximum = false;
            tChart2.Axes.Bottom.AutomaticMinimum = false;
            tChart2.Axes.Bottom.Maximum = length;
            tChart2.Axes.Bottom.Minimum = 0;

            tChart2.Axes.Left.Automatic = false;//左轴点
            tChart2.Axes.Left.AutomaticMaximum = false;
            tChart2.Axes.Left.AutomaticMinimum = false;
            tChart2.Axes.Left.Maximum = 100;
            tChart2.Axes.Left.Minimum = 10;
        }
        private void 参数管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 关闭系统并记录事件
        /// 2017-9-1 HJ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton4_Click(object sender, EventArgs e)
                    {
            MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
            DialogResult dr =  MessageBox.Show("确认要退出吗？", "系统退出", messButton);
            if (dr == DialogResult.OK)
            {
                writeLog("     关闭系统");
                Close();
            }
 
        }

        /// <summary>
        /// 开始运行系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void btnShowAlarm_Click(object sender, EventArgs e)
        {
            frmAlarm frm = new frmAlarm();
            frm.Show();
        }

  
        /// <summary>
        /// 每5秒执行一次
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            int ii=0;
            //读取通道1各距离温度值，并存储在浮点数组_pfData内
            DtsComm dts = new DtsComm();
            dbComm dbcomm = new dbComm();
            int intp;
            int alarmVlues;
            DateTime dt;
            dt= new DateTime();
            dt =DateTime.Now;
            string strAlarm;
            chInfo = dbcomm.getChanalStartAndEnd("1");
            
            getAlarmByCh("1");//查找1通道对应的报警值

            chLenth = Convert.ToInt32(chInfo[2]);
            IntChart1(chLenth);
            IntChart2(chLenth);

            ii = dts.ReadDtsData(1);

            tChart2.Series[0].Clear();
            tChart1.Series[0].Clear();
            toolStripProgressBar1.Value = 0;
            if (ii > 0)
            {
                for (int i = dts.chStart; i < dts.chEnd; i++)//分区开始130，分区结束2000
                {
                    intp = i - dts.chStart;

                    alarmVlues=AlarmValue(intp);

                    if ((double)dts._pfData[i] > alarmVlues)
                    {
                        strAlarm = "报警时间：" + dt.ToString() + "|  距离：" + intp.ToString() + "（米）|  温度：" + Math.Round(dts._pfData[i], 1) + "℃";
                        listBox1.Items.Add("报警时间：" + dt.ToString() + "|  距离：" + intp.ToString() + "（米）|  温度：" + Math.Round(dts._pfData[i], 1) + "℃");
                        writeAlarm(strAlarm);
                    }


                    tChart2.Series[0].Add(i - dts.chStart, (double)dts._pfData[i], Color.Red);
                    tChart1.Series[0].Add(i - dts.chStart, (double)dts._pfData[i], Color.Red);
                    toolStripProgressBar1.PerformStep();
                }
            }
        }

        /// <summary>
        /// 温度曲线总表中鼠标所在点显示的信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void marksTip2_GetText(Steema.TeeChart.Tools.MarksTip sender, Steema.TeeChart.Tools.MarksTipGetTextEventArgs e)
        {
            string strXYValues;
            
            strXYValues = e.Text;
            string[] arr = strXYValues.Split(' ');
            e.Text = "距离：" + arr[0] + " (米)" + "\r\n" + "温度：" + arr[1] + "(°C)";
        }
        
        /// <summary>
        /// 历史温度记录查看窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            frmTempSelect TempSelect = new frmTempSelect();
            TempSelect.ShowDialog();
        }

        /// <summary>
        /// 开始按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string[] alist;

            dbComm dbconn;
            dbconn = new dbComm();
            alist = dbconn.getChanaleList();
            foreach(string chName in alist)
            {
              comboBox1.Items.Add(chName);
            }
            

            if (!timer1.Enabled)
            {
                writeLog("     开始运行");
                timer1.Enabled = true;
                lblStatus.Text = "正在监测各节点温度";
                btnStart.Text = "暂停";
            }
            else
            {
                writeLog("     系统暂停");
                timer1.Enabled = false;
                lblStatus.Text = "监测系统已暂停";
                btnStart.Text = "开始";
            }
        }

        /// <summary>
        /// 停止按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确认要停止采集数据吗？", "系统", MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
            {
                writeLog("  系统停止");
                timer1.Enabled = false;
                lblStatus.Text = "系统已停止运行";
                btnStart.Text = "暂停";
            }
        }

        private void writeLog(string line)
        {
            DirectoryInfo difo = Directory.GetParent(FILE_DIR);
            MyFile myFile = new MyFile("runing.txt", difo.ToString() + "\\log");
            myFile.writeFile(DateTime.Now + line);
        }

        private void writeAlarm(string line)
        {
            DirectoryInfo difo = Directory.GetParent(FILE_DIR);
            MyFile myFile = new MyFile("alarm.txt", difo.ToString() + "\\Alarm");
            myFile.writeFile(DateTime.Now + line);
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void 基本参数设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDtsConfig dstconfig;
            dstconfig = new frmDtsConfig();
            dstconfig.Show();
        }
    }
}
