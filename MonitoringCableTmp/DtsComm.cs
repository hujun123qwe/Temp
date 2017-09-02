using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

using System.Collections;
using System.Runtime.InteropServices;

using System.Text.RegularExpressions;
using Steema.TeeChart.Drawing;
using Steema.TeeChart.Styles;
using System.Windows.Forms;

namespace MonitoringCableTmp
{
    public class DtsComm
    {
        #region  调用dll结构参数传递的函数
            //[DllImport("DTS.dll", EntryPoint = "#1", CallingConvention = CallingConvention.Cdecl)]
            //public static extern int Connect(Socket SocketClient,string dtsIP,int dtsPort);

            [DllImport("DTS.dll", EntryPoint = "#8", CallingConvention = CallingConvention.Cdecl)]
            public static extern int SendDataCSharp(byte[] SendByte, ref int SendSize, int Sendtype, int Channel);

            [DllImport("DTS.dll", EntryPoint = "#5", CallingConvention = CallingConvention.Cdecl)]
            public static extern int RecvDataCSharp(byte[] buff_in, int buff_in_len, byte[] buff_out, ref int buff_out_len, float[] pfData, ref int nSize);
        #endregion

        #region 构造函数
            public DtsComm()
            {            
            }
        #endregion


        #region 对象定义
            TcpClient tcpClient = new TcpClient();
        #endregion

        #region 属性定义
           public float[] _pfData = new float[65535];
           int _SendSize = 0;
           int _buff_out_len = 0;
           int _nSize = 0;
           byte[] sendBytes = new Byte[7];
           ArrayList gbRecvBuff = new ArrayList();

           byte[] readBuff = new byte[1024];
           int readCount = 0;
           byte[] buff_in = new byte[65535];
           int buff_in_Length = 0;
           byte[] buff_out = new byte[65535];
           int buff_out_Length = 0;

           int m = 0;
           int n = 0;
           bool bDataFlag = true;

           private string dtsTcpIp;

           public string DtsTcpIp
           {
               set { dtsTcpIp = value; }
               get { return dtsTcpIp; }
           }

           private string filePath;
           public string FilePath
           {
               set { filePath = value; }
               get { return filePath; }
           }

           private int teechartBottomMaximum;
           public int TeechartBottomMaximum
           {
               set { teechartBottomMaximum = value; }
               get { return teechartBottomMaximum; }
              
           }
        #endregion

            /// <summary>
            /// 写温度值到文本文件中；格式为：短时间+点位1_数据 点位2_数据 点位2_数据（中间有空格）
            /// </summary>
            /// <param name="startLen">开始点</param>
            /// <param name="endLen">结束点</param>
           private void WriteTempToTxtFiles(int startLen, int endLen)
           {
               string runTemp;
               string hisDataFileName;

               FileInfo finfo;
               DateTime dt;

               hisDataFileName = FindOrCreateDirectoryAndFiles();
               finfo = new FileInfo(hisDataFileName);
               if (finfo.Exists)
               {
                   dt = new DateTime();
                   dt = DateTime.Now;
                   StreamWriter sw2 = new StreamWriter(hisDataFileName, true, Encoding.UTF8);
                   runTemp = dt.ToLongTimeString();
                   for (int i = startLen; i<endLen+1; i++)
                   {
                       runTemp = runTemp +" "+ i.ToString()+"_"+Math.Round(_pfData[i],1).ToString();
                   }
                   sw2.WriteLine(runTemp);
                   sw2.Close();
               }
               else
               {
                   dt = new DateTime();
                   dt = DateTime.Now;
                   StreamWriter sw = new StreamWriter(hisDataFileName, true);
                   runTemp = dt.ToLongTimeString();
                   for (int i = startLen; i < endLen + 1; i++)
                   {
                       runTemp = runTemp + " " + Math.Round(_pfData[i], 1).ToString();
                   }
                   sw.WriteLine(runTemp);
                   sw.Close();
               }
           }
          /// <summary>
          /// 查找获建立HisData文件夹下当日文件，对文件夹和 文件查找，如果不存在就创建。
          /// </summary>
          /// <returns>返回已存在的或者新建的文件完整路径，无法找到或建立返回空</returns>
           public string FindOrCreateDirectoryAndFiles()
           {
                string directoryName,filePath,hisDatadirectoryName;
                string hisDataName, hisDataPathName;
                string reHisDataName=null;
                   
                FileInfo finfo;
                DirectoryInfo difo;
                filePath = Directory.GetCurrentDirectory();
                DateTime dt;
                dt = new DateTime();
                dt = DateTime.Now;
                directoryName = "HisData";
                difo = Directory.GetParent(filePath);
                hisDatadirectoryName= difo+@"\"+directoryName;
                if (!Directory.Exists(hisDatadirectoryName))
                {
                    Directory.CreateDirectory(hisDatadirectoryName);

                }

                //以日期作为文件名
                //hisDataName = dt.ToString("yyyy-MM-dd");
                hisDataName = dt.ToShortDateString();
                hisDataName= hisDataName.Replace('/','-') +".dat";
                hisDataPathName=hisDatadirectoryName+@"\"+ hisDataName;
                finfo = new FileInfo(hisDataPathName);
                if (finfo.Exists)
                {
                    reHisDataName = hisDataPathName;
                }
                else
                {
                    //创建文件
                    //File.Create(hisDataPathName).Close();
                    FileStream fs = new FileStream(hisDataPathName, FileMode.Create, FileAccess.Write);
                    fs.Close();
                    reHisDataName = hisDataPathName;
                }
                 return reHisDataName;
           }
        /// <summary>
        /// 把时间转换为YYYY-MM-DD字符串输出
        /// 建议使用DataTime.toString("yyyy-MM-dd");方法
        /// </summary>
        /// <param name="dt">时间</param>
        /// <returns>字符串</returns>
           public string DateFormatToString(DateTime dt)
           {
               string sDate=null;
               DateTime myDate;
               myDate = new DateTime();
               myDate = dt;
               sDate = myDate.ToShortDateString();
               sDate = sDate.Replace('/', '-');
               return sDate;
           }
           /// <summary>
           /// 通过文件名查找对应保存温度数据的文件名是否存在(路径为当前目录下HisData)
           /// </summary>
           /// <param name="tempFileName">文件名（YYYY-MM-DD）</param>
           /// <returns>文件名及全路径（不存在返回NULL）</returns>
           public string FindTempTxtFiles(string tempFileName)
           {
               string directoryName, filePath, hisDatadirectoryName;
               string hisDataPathName;
               string hisDataFileName = null;

               FileInfo finfo;
               DirectoryInfo difo;
               filePath = Directory.GetCurrentDirectory();
               DateTime dt = DateTime.Now;
               directoryName = "HisData";
               difo = Directory.GetParent(filePath);
               hisDatadirectoryName = difo + @"\" + directoryName;

               hisDataPathName = hisDatadirectoryName + @"\" + tempFileName+".dat";
               finfo = new FileInfo(hisDataPathName);
               if (finfo.Exists)
               {
                   hisDataFileName = hisDataPathName;
               }
               return hisDataFileName;
           }
        /// <summary>
        /// 获取指定通道、指定点、指定开始时间、结束时间温度数据从文本文件中
        /// 建议在记录文件名后面加上通道值，好查询，如2017-09-01-01表示1通道记录
        /// 2017-9-1完善
        /// </summary>
        /// <param name="ch">通道号</param>
        /// <param name="pointNum">点</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>哈希表（时间，温度值）</returns>
           public Hashtable getTempDataFromTxtFileForOnePoint(int ch, int pointNum, DateTime startTime, DateTime endTime)
           {
               Hashtable tempData = new Hashtable();

               string dataFileName;
               //string hisDataFileName;
               //string dataStringLine;
               //string sDatetime;
               //string tempValue;
               ArrayList fileLines = new ArrayList();

               dataFileName = DateTime.Now.ToString("yyyy-MM-dd")+".dat";
               DirectoryInfo difo = Directory.GetParent(Directory.GetCurrentDirectory());
               MyFile myFile = new MyFile(dataFileName, difo.ToString()+"\\HisData");
               fileLines = myFile.readFile();

               foreach (string line in fileLines)
               {
                   string[] lineBlock = line.Split(' ');
                   //做时间差，如果在时间范围内，就记录这个时间行
                   if (DateTime.Compare(startTime, Convert.ToDateTime(lineBlock[0]))<=0 
                       && DateTime.Compare(endTime, Convert.ToDateTime(lineBlock[0]))>=0)
                   {
                       for (int i = 1; i < lineBlock.Length; i++)
                       {
                           //判断点编号是否相同，相同就保存到哈希表中
                           string[] pntTmp = lineBlock[i].Split('_');
                           if (Convert.ToInt32(pntTmp[0]) == pointNum)
                           {
                               tempData.Add(lineBlock[0], pntTmp[1]);
                           }
                       }
                   }
               }


               //dataFileName = DateFormatToString(startTime);
               //hisDataFileName = FindTempTxtFiles(dataFileName);
               //StreamReader sw = new StreamReader(hisDataFileName);
            
               //dataStringLine = sw.ReadLine();
               //while(sw.Peek() != -1)
               //{
               //    string[] stempValue = dataStringLine.Split(' ');
               //    sDatetime = stempValue[0];
               //    tempValue = stempValue[pointNum + 1];
               //    string[] stempV = tempValue.Split('_');
               //    tempValue = stempV[1];
               //    tempData.Add(sDatetime, tempValue);
               //    dataStringLine = sw.ReadLine();
               //}
               //sw.Close();
               return tempData;
           }
        //----标记
        /// <summary>
        /// 获取选定通道时间段内点值
        /// </summary>
        /// <param name="ch">通道号</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>点与温度值的哈希表（点为关键字）</returns>
           public Hashtable getTempDataFromTxtFileForAllPoint(int ch, DateTime startTime, DateTime endTime)
           {
               Hashtable tempData = null;

               string dataFileName;
               string hisDataFileName;
               string dataStringLine;
               string sDatetime;
               string tempValue;
               string ponitID;
               dataFileName = DateFormatToString(startTime);
               hisDataFileName = FindTempTxtFiles(dataFileName);
               tempData = new Hashtable();
               StreamReader sw = new StreamReader(hisDataFileName);
               int arryNum = 0;
               dataStringLine = sw.ReadLine();
               while (sw.Peek() != -1)
               {
                   string[] stempValue = dataStringLine.Split(' ');
                   sDatetime = stempValue[0];
                   arryNum = stempValue.Length;
                   for (int i = 0; i < arryNum; i++)
                   {
                       tempValue = stempValue[i + 1];
                       string[] stempV = tempValue.Split('_');
                       ponitID = stempV[0];
                       tempValue = stempV[1];
                       tempData.Add(ponitID, tempValue);
                   }
                   dataStringLine = sw.ReadLine();
               }
               sw.Close();
               return tempData;
           }

           /// <summary>
           /// 写数据到二进制文件中；未完成--ZJJ 20170831
           /// </summary>
           /// <param name="startLen">开始点</param>
           /// <param name="endLen">结束点</param>
           private void WriteTempToBinaryFiles(int startLen, int endLen)
           {
               string ffilepath, logFilename;
               string runTemp;
               string strHour, strMinute, strSecond;
               FileInfo finfo;
               DirectoryInfo difo;
               ffilepath = Directory.GetCurrentDirectory();
               DateTime dt;
               difo = Directory.GetParent(ffilepath);

               logFilename = difo + @"\log\20170824.dat";
               finfo = new FileInfo(logFilename);
           
               if (finfo.Exists)
               {
                   dt = new DateTime();
                   dt = DateTime.Now;
                   FileStream myStream = new FileStream(logFilename, FileMode.Append);
                   BinaryWriter bwTempWriter = new BinaryWriter(myStream);
                   runTemp = dt.ToString();
                   strHour = dt.Hour.ToString();
                   strMinute = dt.Minute.ToString();
                   strSecond = dt.Second.ToString();
                   bwTempWriter.Seek(0, SeekOrigin.End);
                   bwTempWriter.Write(Convert.ToInt32(strHour));
                   bwTempWriter.Write(Convert.ToInt32(strMinute));
                   bwTempWriter.Write(Convert.ToInt32(strSecond));
                   for (int i = startLen; i < endLen + 1; i++)
                   {
                      // runTemp = runTemp + " " + i.ToString() + "_" + Math.Round(_pfData[i], 1).ToString();
                     //  bwTempWriter.Write(i);
                       bwTempWriter.Write(Math.Round(_pfData[i], 1));
                   }
                    bwTempWriter.Close();
                   myStream.Close();
               }
               else
               {
                   dt = new DateTime();
                   dt = DateTime.Now;
                   FileStream myStream = new FileStream(logFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                   BinaryWriter bwTempWriter = new BinaryWriter(myStream, Encoding.UTF8, true);

                   runTemp = dt.ToString();
                   strHour = dt.Hour.ToString();
                   strMinute = dt.Minute.ToString();
                   strSecond = dt.Second.ToString();
                 //  bwTempWriter.Seek(0, SeekOrigin.End);
                   bwTempWriter.Write(Convert.ToInt32(strHour));
                   bwTempWriter.Write(Convert.ToInt32(strMinute));
                   bwTempWriter.Write(Convert.ToInt32(strSecond));
                   for (int i = startLen; i < endLen + 1; i++)
                   {
                      // runTemp = runTemp + " " + i.ToString() + "_" + Math.Round(_pfData[i], 1).ToString();
                       bwTempWriter.Write(Math.Round(_pfData[i], 1));
                   }
                 //  bwTempWriter.Write(runTemp);
                   bwTempWriter.Close();
                   myStream.Close();
               }
           }
           public int ReadDtsData(int chNO)
           {
               int reValue = 0;

               TcpClient tcpClient = new TcpClient();
               tcpClient.Connect("192.168.2.134", 8603);

               NetworkStream ns = tcpClient.GetStream();
               NetworkStream nsRev = tcpClient.GetStream();

               SendDataCSharp(sendBytes, ref _SendSize, 1, chNO);
           //    Console.Write("0x{0:X},0x{1:X},0x{2:X},0x{3:X},0x{4:X},0x{5:X},0x{6:X}\n",
           //           sendBytes[0], sendBytes[1], sendBytes[2], sendBytes[3], sendBytes[4], sendBytes[5], sendBytes[6]);
               

               if (ns.CanWrite)
               {
                   ns.Write(sendBytes, 0, sendBytes.Length);
             //      Console.Write("0x{0:X},0x{1:X},0x{2:X},0x{3:X},0x{4:X},0x{5:X},0x{6:X}\n",
             //          sendBytes[0], sendBytes[1], sendBytes[2], sendBytes[3], sendBytes[4], sendBytes[5], sendBytes[6]);
                   //接收数据开始{{{
                   readCount = nsRev.Read(readBuff, 0, 1024);
              ///     Console.Write("readBuff[0]=0x{0:X},buff_in_Length={1:d}\n", readBuff[0], ((readBuff[1] << 8) + readBuff[2]));

                   
                   for (m = 0; m < readCount; m++)
                   {
                       buff_in[buff_in_Length + m] = readBuff[m];
                   }
                   buff_in_Length = readCount;
                   

                   //一次读取未结束，再次读取
                   while (bDataFlag)
                   {
                       readCount = nsRev.Read(readBuff, 0, 1024);
                       if (readCount > 0)
                       {
                           buff_in_Length += readCount;
                           for (m = 0; m < readCount; m++)
                           {
                            //   if (readBuff[m] != 0xAA)
                              // {
                                   buff_in[buff_in_Length - readCount + m] = readBuff[m];
                               //}else
                                   if (readBuff[m] == 0xAA)
                                   {
                                     bDataFlag = false;
                                     break;
                                    }

                           }
                       }
                   }

                   //接收数据结束
             //      RecvDataCSharp(buff_in, buff_in_Length, buff_out, ref buff_out_Length, _pfData, ref _nSize);
                   if (buff_in_Length > 0)
                   {
                       RecvDataCSharp(buff_in, buff_in_Length, buff_out, ref buff_out_Length, _pfData, ref _nSize);
                       reValue= buff_in_Length;
                   }
                   else
                   {
                       reValue = 0;
                   }
                  // WriteTempToFiles(130, 700);
                   WriteTempToTxtFiles(130, 700);
/*
                   double fMaxTemp = 0.0;
                   double fPos = 0.0;

                   for (m = 130; m < 4000; m++)//130米开始    4000米结束
                   {
                       double tempVal = (double)_pfData[m];
                       if (n > 590 && n < 600) //获取分区590米开始,600结束
                       {
                           if (fMaxTemp < tempVal)
                           {
                               fMaxTemp = tempVal;
                               fPos = n;
                           }
                           Console.Write("{0:F2}℃\n", tempVal);
                       }
                       n++;
                   }

                   return 1;
 */
               }
               tcpClient.Close();
               ns.Close();
               return reValue;
           }

  }
}
