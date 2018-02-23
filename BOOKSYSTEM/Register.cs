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
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        //缓存注册基本信息word
        //protected int ID;
        //protected string Name;
        //protected string Pass;
        //protected string Department;

        private void Register_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (IDtextBox.Text == "")
            {
                MessageBox.Show("请输入ID", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
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
                    else {
                        if (DeparttextBox.Text == "")
                        {
                            MessageBox.Show("请输入您所在的班级", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }  //用户输入规范检查完毕，开始连接数据库

                        try
                        {
                            //实例化数据库连接对象，并设置连接到数据库的参数
                           // SqlConnection sqlConn = new SqlConnection("server=.;database=BOOK_MANAGE_SYSTEM;Trusted_Connection=SSPI");
                            SqlConnection sqlConn = DBconnect.BooksystemCon();
                            sqlConn.Open();	//打开连接
                            if (sqlConn.State == ConnectionState.Open)
                            {
                                //检查用户是否正在借阅该书
                                SqlCommand cmd1 = new SqlCommand("select U_ID from USERS where U_ID ='" + IDtextBox.Text+ "'", sqlConn);
                                SqlDataReader sdr1 = cmd1.ExecuteReader();
                                sdr1.Read();
                                if (sdr1.HasRows)
                                {
                                    MessageBox.Show("sorry！该用户ID 已经存在了哦！请换个ID 号进行注册");
                                    sqlConn.Close();
                                }
                                else
                                {
                                    sqlConn.Close();

                                    //将注册信息写入数据库
                                    string sql = "insert into USERS (U_ID,NAME,DEPARTMENT,PASSWORDS) VALUES('" + IDtextBox.Text + "','" + NametextBox.Text + "','" + DeparttextBox.Text + "','" + PasswordtextBox.Text + "')";
                                  //  MessageBox.Show(sql);
                                    SqlCommand cmd = new SqlCommand(sql, sqlConn);
                                    sqlConn.Open();

                                    cmd.ExecuteNonQuery();
                                    MessageBox.Show("注册成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                                    //写入数据库完毕，将用户信息初始化到系统
                                    Mainform main = new Mainform();
                                    main.power = "0";
                                    main.Names = NametextBox.Text;
                                    main.Times = DateTime.Now.ToShortDateString();
                                    main.Show();
                                    this.Hide();
            
                                    sqlConn.Close();
                                }
                               
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
                }
            }

        }

       
        private void Register_FormClosing(object sender, FormClosingEventArgs e)
        {

           // Login login = new Login();
           // login.Show();
            //this.Hide();
           Application.Exit();
        }
    }
}
