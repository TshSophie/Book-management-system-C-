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
    public partial class ADDtypes : Form
    {
        public ADDtypes()
        {
            InitializeComponent();
        }

        private void ADDtypes_Load(object sender, EventArgs e)
        {

        }

        private void submitbutton_Click(object sender, EventArgs e)
        {
            if (addtypestextBox.Text != "")
            {
                try
                {
                    //实例化数据库连接对象，并设置连接到数据库的参数
                   // SqlConnection sqlConn = new SqlConnection("server=.;database=BOOK_MANAGE_SYSTEM;Trusted_Connection=SSPI");
                    SqlConnection sqlConn = DBconnect.BooksystemCon();
                    sqlConn.Open();	//打开连接
                    if (sqlConn.State == ConnectionState.Open)
                    {

                        //将注册信息写入数据库
                        string sql = "insert into BOOK_TYPES (NAME) VALUES('"+addtypestextBox.Text+ "')";
                        MessageBox.Show(sql);
                        SqlCommand cmd = new SqlCommand(sql, sqlConn);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("插入数据成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                        //写入数据库完毕，清空输入框
                        addtypestextBox.Text = "";

                    }

                    else
                    {

                        MessageBox.Show("数据库连接失败！");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }


            }
            else
            {
                MessageBox.Show("输入不能为空！");
            }
        }
    }
}
