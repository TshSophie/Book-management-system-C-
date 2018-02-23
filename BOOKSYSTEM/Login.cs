using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace BOOKSYSTEM
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (NametextBox.Text == "")
            {
                MessageBox.Show("请输入用户名", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (PasswordtextBox.Text == "")
                {
                    MessageBox.Show("请输入密码", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    try
                    {
                        //实例化数据库连接对象，并设置连接到数据库的参数
                        //SqlConnection sqlConn = new SqlConnection("server=.;database=BOOK_MANAGE_SYSTEM;Trusted_Connection=SSPI");
                       SqlConnection sqlConn = DBconnect.BooksystemCon();
                        sqlConn.Open();	//打开连接
                        if (sqlConn.State == ConnectionState.Open)
                        {
                            string sql = "select * from USERS where NAME='" + NametextBox.Text + "' and PASSWORDS='" + PasswordtextBox.Text+ "'";
                         //   MessageBox.Show(sql);
                            SqlCommand cmd = new SqlCommand(sql, sqlConn);
                            
                            SqlDataReader sdr = cmd.ExecuteReader(); //使用ExecuteReader创建SqlDataReader对象
                            sdr.Read(); //读取/前进到下一条记录
                            if (sdr.HasRows) //查询结果集中是否有值
                            {
                             //   MessageBox.Show("恭喜！登录成功");

                                sdr.Close();
                                cmd = new SqlCommand("select * from USERS where NAME='" + NametextBox.Text + "'", sqlConn);
                                SqlDataReader sdr1 = cmd.ExecuteReader();
                                sdr1.Read();
                                string UserPower = sdr1["POWERS"].ToString().Trim();
                                string UserId = sdr1["U_ID"].ToString().Trim();
                                sqlConn.Close();
                                Mainform main = new Mainform();
                                main.power = UserPower;
                                main.U_ID = UserId;
                                main.Names = NametextBox.Text;
                                main.Times = DateTime.Now.ToShortDateString();
                                main.Show();
                                this.Hide();
                              
                            }
                            else
                            {
                                MessageBox.Show("用户名或密码错误");
                            }
                        }
                        else {

                            MessageBox.Show("数据库连接失败！");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);     
                    }
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //进入注册界面
            Register register = new Register();
            register.Show();
            this.Hide();
        }

       

        
    }
}
