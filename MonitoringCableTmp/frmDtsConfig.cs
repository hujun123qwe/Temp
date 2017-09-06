using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonitoringCableTmp
{
    public partial class frmDtsConfig : Form
    {
        public frmDtsConfig()
        {
            InitializeComponent();
            readChannelInfo();

        }
        private void readChannelInfo()
        {
            dbComm dbcomm;
            dbcomm = new dbComm();
            DataView dv1;
            dv1 = new DataView();
            dv1 = dbcomm.getChanaleInfoAll();
            dataGridView1.DataSource = dv1;
            this.dataGridView1.Columns[5].Width = 180;
        }
        private void tabPage1_Click(object sender, EventArgs e)
        {
 
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.SelectedCells[0].Value.ToString();
            textBox2.Text = dataGridView1.SelectedCells[1].Value.ToString();
            textBox3.Text = dataGridView1.SelectedCells[2].Value.ToString();
            textBox4.Text = dataGridView1.SelectedCells[3].Value.ToString();
            textBox5.Text = dataGridView1.SelectedCells[4].Value.ToString();
            textBox6.Text = dataGridView1.SelectedCells[5].Value.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            int renUpdateNum = 0;
                dbComm dbcomm;
            dbcomm = new dbComm();
            string strSql;
            strSql = "update Channel set chStart=" + textBox3.Text + ", chEnd=" + textBox4.Text + ", chLenth= " + textBox5.Text + ", remark='" + textBox6.Text + "' where chID=" + textBox1.Text;
            renUpdateNum=dbcomm.upTable(strSql);
            if (renUpdateNum == 1)
            {
                MessageBox.Show("修改数据成功！");
                readChannelInfo();
            }

            
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name == "tabPage2")
            {
                string[] alist;
                dbComm dbconn;
                dbconn = new dbComm();
                alist = dbconn.getChanaleList();
                foreach (string chName in alist)
                {
                    comboBox1.Items.Add(chName);
                }

            }
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            string strch, stra;
            dbComm dbcomm;
            dbcomm = new dbComm();
            DataView dv1;
            dv1 = new DataView();
            stra = comboBox1.Text;
            strch = stra.Substring(stra.Length - 1, 1);
            dv1 = dbcomm.getParagraphInfoAllByChID(strch);
            dataGridView2.DataSource = dv1;
            this.dataGridView2.Columns[5].Width = 180;
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
  
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox7.Text = dataGridView2.SelectedCells[0].Value.ToString();
            textBox8.Text = dataGridView2.SelectedCells[1].Value.ToString();
            textBox9.Text = dataGridView2.SelectedCells[2].Value.ToString();
            textBox10.Text = dataGridView2.SelectedCells[3].Value.ToString();
            textBox11.Text = dataGridView2.SelectedCells[4].Value.ToString();
            textBox12.Text = dataGridView2.SelectedCells[5].Value.ToString();
            textBox13.Text = dataGridView2.SelectedCells[6].Value.ToString();
        }

        private void btnPUpdate_Click(object sender, EventArgs e)
        {

            int renUpdateNum = 0;
            dbComm dbcomm;
            dbcomm = new dbComm();
            string strSql;
            strSql = "update Paragraph set ParStart=" + textBox10.Text + ", ParEnd=" + textBox11.Text + " , parAlarmUpTemp=" + textBox12.Text + ", remark='" + textBox13.Text + "' where chID=" + textBox7.Text + " and parID=" + textBox8.Text;
            renUpdateNum = dbcomm.upTable(strSql);
            if (renUpdateNum == 1)
            {
                MessageBox.Show("修改数据成功！");
                comboBox1_Leave(this, e);
            }
        }

        private void bntInsert_Click(object sender, EventArgs e)
        {
            int renUpdateNum = 0;
            dbComm dbcomm;
            dbcomm = new dbComm();
            string strSql;
            strSql = "insert into Paragraph values(" + textBox7.Text + "," + textBox8.Text + ",' " +textBox9.Text +"',"+ textBox10.Text+"," + textBox11.Text + " ," + textBox12.Text + ",'" + textBox13.Text + "')";
            renUpdateNum = dbcomm.upTable(strSql);
            if (renUpdateNum == 1)
            {
                MessageBox.Show("修改数据成功！");
                comboBox1_Leave(this, e);
            }
        }

     }
}
