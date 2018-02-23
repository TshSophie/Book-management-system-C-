using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace BOOKSYSTEM
{
    public partial class BookSearch : Form
    {
        public BookSearch()
        {
            InitializeComponent();
        }

        private void BookDisplay_Load(object sender, EventArgs e)
        {

           // MessageBox.Show(Mainform.Searchtext);
            NAMEtextBox1.Text = Mainform.Searchtext; 
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string sql = "";
            string where = " WHERE ";

            if (NAMEtextBox1.Text == "")
            {
                sql +="";
            }
            else
            {
                sql = "NAME LIKE '%" + NAMEtextBox1.Text + "%' ";
            }

            if (AUTHORtextBox2.Text == "")
            {
                sql += "";
            }
            else if (NAMEtextBox1.Text != "")
            {

                sql += " AND AUTHOR LIKE '%" + AUTHORtextBox2.Text + "%' ";
            }
            else {
                sql += "AUTHOR LIKE '%" + AUTHORtextBox2.Text + "%' ";
                
            }
            if (PUBLISHERtextBox3.Text == "")
            {
                sql += "";
            }
            else if (AUTHORtextBox2.Text != ""&&NAMEtextBox1.Text!="")
            {

                sql += " AND PUBLISHER LIKE '%" + PUBLISHERtextBox3.Text + "%' ";
            }
            else {
                sql += "PUBLISHER LIKE '%" + PUBLISHERtextBox3.Text + "%' ";
            }

            if (NAMEtextBox1.Text == ""&&AUTHORtextBox2.Text == "" && NAMEtextBox1.Text == "")
            { //如果三个输入框都为空则将所有数据都查出来
              where = "";
            }
                                               
          //  SqlConnection sqlConn = new SqlConnection("server=.;database=BOOK_MANAGE_SYSTEM;Trusted_Connection=SSPI");
            SqlConnection sqlConn = DBconnect.BooksystemCon();
            sqlConn.Open();	//打开连接
           //  MessageBox.Show("select B_ID,NAME,AUTHOR,PUBLISHER,NUMBER,PUB_TIME,TYPE,STORAGE,IMAGES from BOOKS " + where + sql.Trim());  
            //创建一个SqlDataAdapter对象
            SqlDataAdapter sda = new SqlDataAdapter("select B_ID,NAME,AUTHOR,PUBLISHER,NUMBER,PUB_TIME,TYPE,STORAGE,IMAGES from BOOKS " +where+ sql.Trim(), sqlConn);
                   
            //创建一个DataSet对象
            DataSet ds = new DataSet();
            //使用SqlDataAdapter对象的Fill方法填充DataSetS
            sda.Fill(ds, "BOOKS");
            //设置dataGridView1控件数据源
            dataGridView1.DataSource = ds.Tables[0];
              

        }
    }
}
