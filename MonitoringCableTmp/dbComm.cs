using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.OleDb;
using System.Data;
using System.Collections;


namespace MonitoringCableTmp
{
    class dbComm
    {
        private string connectionString;
        protected OleDbConnection Connection;
   //     public  dbComm()
    //    {

    //    }
        public bool connDb()
        {
            string filePath ;
            DirectoryInfo difo;
            filePath = Directory.GetCurrentDirectory();
            difo = Directory.GetParent(filePath);
            connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source= ";
            connectionString = connectionString +  difo + @"\" + "DB" + @"\" + "dtsDB.mdb";
            Connection = new OleDbConnection(connectionString);
            Connection.Open();
            return true;
        }
        /// <summary>
        /// /获取定义通道列表
        /// </summary>
        /// <returns>通道名称数组</returns>
        public string[] getChanaleList()
        {
            string[] Chlist = null; 
            string  chNum;
            if (connDb())
            {
                OleDbCommand acommand = new OleDbCommand("select count(*) from Channel ", Connection);
                OleDbDataReader odr = acommand.ExecuteReader();
                odr.Read();
                chNum = odr[0].ToString();
                Chlist = new string[Convert.ToInt16(chNum)];

                OleDbCommand olecomm = new OleDbCommand("select chName from Channel order by chID", Connection);
                OleDbDataReader areader = olecomm.ExecuteReader();

                int i = 0;
                while (areader.Read())
                {
                    Chlist[i] = areader[0].ToString();
                    i++;
                }
                Connection.Close();
            }
            return Chlist;
        }
        /// <summary>
        /// 获取通道对应的开始距离，结束距离、总长度
        /// </summary>
        /// <param name="chNO">通道字符串</param>
        /// <returns>数组</returns>
        public string[] getChanalStartAndEnd(string chNO)
        {
            string[] Chlist = new string[3];
            string strSql;
            strSql="select chStart,chEnd,chLenth from Channel where chID= "+chNO;
            if (connDb())
            {
                OleDbCommand acommand = new OleDbCommand(strSql, Connection);
                OleDbDataReader odr = acommand.ExecuteReader();
                odr.Read();
                Chlist[0] = odr[0].ToString();
                Chlist[1] = odr[1].ToString();
                Chlist[2] = odr[2].ToString();
                Connection.Close();
            }
            return Chlist;
        }
        public DataView getChanaleInfoAll()
        {
            string strSql;
            DataSet ds;
            DataView dv;
            dv = new DataView();
            strSql = "select chID as 序号,chName as 通道名称,chStart as 开始距离,chEnd as 结束距离,chLenth as 总长度,remark as 备注 from Channel order by chID ";

           // strSql = "select * from Channel order by chID ";
            if (connDb())
            {
                OleDbCommand acommand = new OleDbCommand(strSql, Connection);
                OleDbDataAdapter olesda = new OleDbDataAdapter();
                olesda.SelectCommand = acommand;
                ds = new DataSet();
                olesda.Fill(ds, "Ch");
                dv = ds.Tables[0].DefaultView;
            }
            Connection.Close();
            return dv;

        }
        public int upTable(string strSql)
        {
            int ret = 0;
            if (connDb())
            {
                OleDbCommand acommand = new OleDbCommand(strSql, Connection);
                ret = Convert.ToInt32(acommand.ExecuteNonQuery());
            }
            Connection.Close();
            return ret;
        }
        public DataView getParagraphInfoAllByChID(string chID)
        {
            string strSql;
            DataSet ds;
            DataView dv;
            dv = new DataView();
            strSql = "select * from Paragraph where chID="+chID +" order by parID ";

            // strSql = "select * from Channel order by chID ";
            if (connDb())
            {
                OleDbCommand acommand = new OleDbCommand(strSql, Connection);
                OleDbDataAdapter olesda = new OleDbDataAdapter();
                olesda.SelectCommand = acommand;
                ds = new DataSet();
                olesda.Fill(ds, "Ch");
                dv = ds.Tables[0].DefaultView;
            }
            Connection.Close();
            return dv;
        }
        public string[] getParagAlarmBYChIN(string chID)
        {
            string[] tempAlarm;
            string strSql;
            int i = 0;
            string  chNum;
            tempAlarm = null;
            if (connDb())
            {
              OleDbCommand acommand = new OleDbCommand("select count(*) from Paragraph where chID=" + chID, Connection);
              OleDbDataReader odr = acommand.ExecuteReader();
              odr.Read();
              chNum = odr[0].ToString();

              tempAlarm = new string[Convert.ToInt16(chNum)];

              strSql = "select ParStart,ParEnd,ParAlarmUPTemp from Paragraph where chID=" + chID + " order by parID ";

              OleDbCommand bcommand = new OleDbCommand(strSql, Connection);
              OleDbDataReader odr1 = bcommand.ExecuteReader();
              while (odr1.Read())
              {
                  tempAlarm[i] = odr1[0].ToString() + "_" + odr1[1].ToString() + "_" + odr1[2].ToString();
                  i++;
              }
            }
            Connection.Close();
            return tempAlarm;
        }

    }
}
