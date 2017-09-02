/**
 * 文件操作类
 * 
 **/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Collections;

namespace MonitoringCableTmp
{
    class MyFile
    {
        private string file_name { get; set; }
        private string file_addr { get; set; }
        public MyFile()
        {
            file_name = "dat.dat";
            file_addr = Directory.GetCurrentDirectory();
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="file_name">文件名</param>
        /// <param name="file_addr">文件路径</param>
        public MyFile(string file_name, string file_addr)
        {
            this.file_name = file_name;
            this.file_addr = file_addr;
        }

        /// <summary>
        /// 以字节方式读取文件
        /// </summary>
        /// <returns>返回文件内容,按行保存在ArrayList里面</returns>
        public ArrayList readFile()
        {
            string path = Path.Combine(file_addr, file_name);
            string line = null;
            ArrayList lines = new ArrayList(); 
            if (!File.Exists(path))
            {
                MessageBox.Show("文件"+file_name+"不存在");
            }
            else
            {
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                StreamReader sr = new StreamReader(fs, Encoding.UTF8);
                while ((line = sr.ReadLine()) != null)
                {
                    lines.Add(line);
                }
                sr.Close();
                fs.Close();
                return lines;
            }
            return lines;
        }

        /// <summary>
        /// 以字节方式写文件（一行）
        /// </summary>
        /// <param name="line">写入内容</param>
        public void writeFile(string line)
        {
            string path = Path.Combine(file_addr, file_name);
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }
            FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            sw.WriteLine(line);
            sw.Close();
            fs.Close();
        }
    }
}
