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
            DirectoryInfo difo = Directory.GetParent(FILE_DIR);
            MyFile myFile = new MyFile("runing.txt", difo.ToString()+"\\log");
            myFile.writeFile(DateTime.Now + "     关闭系统");
            Close();
        }

        /// <summary>
        /// 开始运行系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            DirectoryInfo difo = Directory.GetParent(FILE_DIR);
            MyFile myFile = new MyFile("runing.txt", difo.ToString() + "\\log");
            if (!timer1.Enabled)
            {
                myFile.writeFile(DateTime.Now + "     开始运行");
                timer1.Enabled = true;
                lblStatus.Text = "正在监测各节点温度";
                btnStart.Text = "暂停";
            }
            else
            {
                myFile.writeFile(DateTime.Now + "     系统暂停");
                timer1.Enabled = false;
                lblStatus.Text = "监测系统已暂停";
                btnStart.Text = "开始";
            }
            //string ffilepath ,logFilename;
            //FileInfo finfo;
            //DirectoryInfo difo;
            //ffilepath = Directory.GetCurrentDirectory();
            //DateTime dt;
            //difo = Directory.GetParent(ffilepath);
            //logFilename = difo +@"\log\runing.txt";
            //finfo = new FileInfo(logFilename);

            //if (finfo.Exists)
            //{
            //    dt = new DateTime();
            //    dt = DateTime.Now;
            //    StreamWriter sw2 = new StreamWriter(logFilename, true, Encoding.UTF8);
            //    sw2.WriteLine(dt.ToString()+"     开始运行");
            //    sw2.Close();
            //}else
            //{
            //    dt = new DateTime();
            //    dt = DateTime.Now;
            //    StreamWriter sw = new StreamWriter(logFilename, true);
            //    sw.WriteLine(dt.ToString() + "     开始运行");
            //    sw.Close();
                    
            //}
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //string filename=@"D:\\MonitoringCableTmp\MonitoringCableTmp\bin\Debug\test1.xml";
            XMLHealper xml = new XMLHealper();
            //xml.CreatexmlDocument(filename, "key1", "gb2312");
            xml.OpenXML();
            label15.Text = xml.FilePath;
            //xml.AddParentNode("key");
            //xml.AddChildNode("key","key1");
            //xml.AddParentNode("aaaa");
            xml.AddAttribute("key1", "b");
            xml.SavexmlDocument();
        }

        private void button4_Click(object sender, EventArgs e)
        {
  
             timer1.Enabled = true;
        }

        private void tChart1_DoubleClick(object sender, EventArgs e)
        {
  

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
            ii = dts.ReadDtsData(1);
            tChart2.Series[0].Clear();
            toolStripProgressBar1.Value = 0;
            if (ii > 0)
            {
                for (int i = 0; i <1000; i++)//分区开始130，分区结束2000
                {
                    tChart2.Series[0].Add(i,(double)dts._pfData[i], Color.Red);
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            
            string ffilepath, logFilename;
            string runTemp;
            string stempValues, sDatetime;
            int ii = 0;
            double svalues = 0;
            //FileInfo finfo;
            DirectoryInfo difo;
            ffilepath = Directory.GetCurrentDirectory();
            DateTime dt;
            difo = Directory.GetParent(ffilepath);
            logFilename = difo + @"\log\runing.txt";
            StreamReader sw = new  StreamReader(logFilename);
            tChart1.Series[0].Clear();
            //tChart1.Series[0].XValues.DateTime = true;
            for (int i = 1; i < 1000; i++)
            {
                runTemp = sw.ReadLine();
                if (sw.Peek() != -1)
                {
                    string[] stempValue = runTemp.Split(' ');
                    sDatetime =  stempValue[0]+" "+stempValue[1];
                    sDatetime = sDatetime.Replace('/', '-');
                    stempValues = stempValue[2];
                    string[] stempV = stempValues.Split('_');  
                    //ii = Convert.ToInt32(stempV[0]);
                    svalues = Convert.ToDouble(stempV[1]);
                    tChart1.Series[0].Add(svalues, Color.Red);
                }
            }

            sw.Close();
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string ffilepath, logFilename;
            string runTemp;
            string stempValues, sDatetime;
            int ii = 0;
            double svalues = 0;
            //  FileInfo finfo;
            DirectoryInfo difo;
            ffilepath = Directory.GetCurrentDirectory();
            DateTime dt;
            difo = Directory.GetParent(ffilepath);
            logFilename = difo + @"\log\runing.txt";
            StreamReader sw = new StreamReader(logFilename);
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

        private void button7_Click(object sender, EventArgs e)
        {
            DtsComm dtscomm;
            string hisDataFileName;
            dtscomm = new DtsComm();
            hisDataFileName= dtscomm.FindOrCreateDirectoryAndFiles();
            if (hisDataFileName != null)
            {
                MessageBox.Show(hisDataFileName);
            }
        }

        /// <summary>
        /// 开始按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            DirectoryInfo difo = Directory.GetParent(FILE_DIR);
            MyFile myFile = new MyFile("runing.txt", difo.ToString() + "\\log");
            if (!timer1.Enabled)
            {
                myFile.writeFile(DateTime.Now + "     开始运行");
                timer1.Enabled = true;
                lblStatus.Text = "正在监测各节点温度";
                btnStart.Text = "暂停";
            }
            else
            {
                myFile.writeFile(DateTime.Now + "     系统暂停");
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
            DirectoryInfo difo = Directory.GetParent(FILE_DIR);
            MyFile myFile = new MyFile("runing.txt", difo.ToString() + "\\log");
            myFile.writeFile(DateTime.Now + "     系统停止");
            timer1.Enabled = false;
            lblStatus.Text = "系统已停止运行";
        }
        
    }
}
