using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace BOOKSYSTEM
{
    public partial class Mainform : Form
    {
        public Mainform()
        {
            InitializeComponent();
        }

        //预保留属性设置,用于显示用户信息以及判断操作权限
        public SqlDataReader sdr;
        public string power;  //权限等级
        public string Names;  //用户名
        public string U_ID;//用户id
        public string Times;  //登录时间
        public static string Searchtext; //用户输入的搜索词


        private void BindData()
        {
           // SqlConnection sqlConn = new SqlConnection("server=.;database=BOOK_MANAGE_SYSTEM;Trusted_Connection=SSPI");
            SqlConnection sqlConn = DBconnect.BooksystemCon();
            sqlConn.Open();	//打开连接
            //创建一个SqlDataAdapter对象
            SqlDataAdapter sda = new SqlDataAdapter("select B_ID,NAME,AUTHOR,PUBLISHER,IMAGES from BOOKS WHERE B_ID IN (SELECT B_ID FROM CUR_BORROW WHERE U_ID = '" + U_ID + "')", sqlConn);
            //创建一个DataSet对象
            DataSet ds = new DataSet();
            //使用SqlDataAdapter对象的Fill方法填充DataSetS
            sda.Fill(ds, "BOOKS");
            //设置dataGridView1控件数据源
            currentdataGridView1.DataSource = ds.Tables[0];

            
            
            //select BOOKS.NAME,HIS_BORROW.BOR_TIME,HIS_BORROW.BACK_TIME,HIS_BORROW.U_ID FROM BOOKS inner join HIS_BORROW ON BOOKS.B_ID = HIS_BORROW.B_ID AND  HIS_BORROW.U_ID = '201601';
             //创建一个SqlDataAdapter对象
            SqlDataAdapter sda1 = new SqlDataAdapter("select BOOKS.B_ID,BOOKS.NAME,BOOKS.AUTHOR,BOOKS.PUBLISHER,HIS_BORROW.BOR_TIME,HIS_BORROW.BACK_TIME FROM BOOKS inner join HIS_BORROW ON BOOKS.B_ID = HIS_BORROW.B_ID AND  HIS_BORROW.U_ID = '" + U_ID + "'", sqlConn);
            //创建一个DataSet对象
            DataSet ds1 = new DataSet();
            //使用SqlDataAdapter对象的Fill方法填充DataSetS
            sda1.Fill(ds1, "BOOKS2");
            //设置dataGridView1控件数据源
            dataGridView1.DataSource = ds1.Tables[0];
            
        }
        private void Refrensh()
        {
            //初始化图书导航

            //SqlConnection sqlConn = new SqlConnection("server=.;database=BOOK_MANAGE_SYSTEM;Trusted_Connection=SSPI");
            SqlConnection sqlConn = DBconnect.BooksystemCon();
            sqlConn.Open();	//打开连接
            SqlCommand cmd = new SqlCommand("select NAME from BOOK_TYPES ", sqlConn);
            SqlDataReader sdr = cmd.ExecuteReader();

            int i = 0;

            TreeNode[] sup = new TreeNode[20];  //父节点
            string[] savesup = new string[20];  //存放数据库查询到的数据

            TreeNode[] sub = new TreeNode[50];
            string sql;

            while (sdr.Read())
            {
                //添加父节点                           
                sup[i] = treeView2.Nodes.Add(sdr["NAME"].ToString().Trim());
                savesup[i] = sdr["NAME"].ToString().Trim();
                i++;
            }
            sqlConn.Close();
            //添加当前查询到的父节点的子节点
            int k = 0;
            for (int j = 0; j < i; j++)
            {
                sqlConn.Open();
                sql = "select NAME from BOOKS WHERE BOOKS.TYPE=(SELECT ID FROM BOOK_TYPES WHERE NAME ='" + savesup[j] + "')";
                SqlCommand cmd1 = new SqlCommand(sql, sqlConn);
                SqlDataReader sdr1 = cmd1.ExecuteReader();
                while (sdr1.Read())
                {
                    sub[k] = new TreeNode(sdr1["NAME"].ToString().Trim());
                    //将以上子节点添加到父节点中
                    sup[j].Nodes.Add(sub[k]);
                    k++;
                }
                sqlConn.Close();

            }
            //载入用户借书信息
            BindData();
        }
        private void Mainform_Load(object sender, EventArgs e)
        {
            switch (power)
            {
                case "0": UserInforrichTextBox2.Text = "权限：一般用户\n" + "欢迎," + Names; manageBookToolStripMenuItem.Enabled = false; break;
                case "1": UserInforrichTextBox2.Text = "权限：超级管理员\n" + "欢迎," + Names; break;             
            }

            Refrensh();
           
        }
        private void refrenshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
           
        }
        private void currentdataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

           
           
        }

       
        private void currentdataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            idtextBox2.Text = currentdataGridView1.SelectedCells[0].Value.ToString();
            NAMEtextBox2.Text = currentdataGridView1.SelectedCells[1].Value.ToString();
            AUTHORtextBox3.Text = currentdataGridView1.SelectedCells[2].Value.ToString();
            PUBLISHERtextBox4.Text = currentdataGridView1.SelectedCells[3].Value.ToString();
            string IMAGE = currentdataGridView1.SelectedCells[4].Value.ToString();

            FileStream pFileStream = new FileStream(IMAGE, FileMode.Open, FileAccess.Read);
            pictureBox2.Image = Image.FromStream(pFileStream);
            pFileStream.Close();
            pFileStream.Dispose();
            

        }


        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //在AfterSelect事件中获取控件中选中节点显示的文本
          //  label1.Text = "当前选中的节点：" + e.Node.Text;
           // SqlConnection sqlConn = new SqlConnection("server=.;database=BOOK_MANAGE_SYSTEM;Trusted_Connection=SSPI");
            SqlConnection sqlConn = DBconnect.BooksystemCon();
            sqlConn.Open();	//打开连接
            SqlCommand cmd = new SqlCommand("select B_ID,NAME,AUTHOR,PUBLISHER,NUMBER,PUB_TIME,TYPE,STORAGE,IMAGES from BOOKS where NAME ='" + e.Node.Text + "'", sqlConn);
            SqlDataReader sdr = cmd.ExecuteReader();
            sdr.Read();
            if(sdr.HasRows)
            {
            idtextBox.Text = sdr["B_ID"].ToString().Trim();
            nametextBox.Text = sdr["NAME"].ToString().Trim();
            authortextBox.Text = sdr["AUTHOR"].ToString().Trim();
            publishertextBox.Text = sdr["PUBLISHER"].ToString().Trim();
            timetextBox.Text = sdr["PUB_TIME"].ToString().Trim();
            storagetextBox.Text = sdr["STORAGE"].ToString().Trim();
            NUMtextBox.Text = sdr["NUMBER"].ToString().Trim();
            FileStream pFileStream = new FileStream(sdr["IMAGES"].ToString().Trim(), FileMode.Open, FileAccess.Read);
            pictureBox1.Image = Image.FromStream(pFileStream);
            pFileStream.Close();
            pFileStream.Dispose();
            

            }
            
           sqlConn.Close();

           //查询评论
//SqlConnection sqlConn = new SqlConnection("server=.;database=BOOK_MANAGE_SYSTEM;Trusted_Connection=SSPI");
           sqlConn.Open();	//打开连接
           string SQL = "select BOOK_COMMENT.CONTENT,BOOK_COMMENT.TIME,USERS.NAME from BOOK_COMMENT,USERS WHERE USERS.U_ID = BOOK_COMMENT.USERID AND BOOK_COMMENT.BOOK_ID = (SELECT B_ID FROM BOOKS WHERE NAME ='" + e.Node.Text.Trim() + "')";
           //SQL = "select *from BOOK_COMMENT";
           SqlCommand cmd4 = new SqlCommand(SQL, sqlConn);
         //  MessageBox.Show(SQL);
           SqlDataReader sdr4 = cmd4.ExecuteReader();

           string main = "";

           commentrichTextBox.Text = "";
           while (sdr4.Read())
           {                              
                   main += sdr4["NAME"] + "      " + sdr4["TIME"] + "\n" + sdr4["CONTENT"] + "\n\n";
                   commentrichTextBox.Text = main;                                                                                                                        
           }

           sqlConn.Close();


              

        }
        private void lookbutton_Click(object sender, EventArgs e)
        {
          

        }

        private void brrow_Click(object sender, EventArgs e)
        {
            if (idtextBox.Text=="")
            {
                MessageBox.Show("还没有选择图书哦！请选好图书再借阅");
            }else{

                    System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
                    long timeStamp = (long)(DateTime.Now - startTime).TotalMilliseconds; // 相差毫秒数

                   // SqlConnection sqlConn = new SqlConnection("server=.;database=BOOK_MANAGE_SYSTEM;Trusted_Connection=SSPI");
                    SqlConnection sqlConn = DBconnect.BooksystemCon();
                    sqlConn.Open();	//打开连接 
            
                    //检查用户是否正在借阅该书
                    SqlCommand cmd1 = new SqlCommand("select C_ID from CUR_BORROW where U_ID ='" + U_ID + "' AND B_ID ='" + idtextBox.Text+"'", sqlConn);
                    SqlDataReader sdr1 = cmd1.ExecuteReader();
                    sdr1.Read();
                    if (sdr1.HasRows)
                    {
                        MessageBox.Show("您已经借阅了该书籍了，一次只能借一本哦！");
                        sqlConn.Close();
                    }
                    else
                    {
                        sqlConn.Close();
                 
                        string sql = "insert into CUR_BORROW(C_ID,BOR_TIME,U_ID,B_ID) values('" + timeStamp + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + U_ID + "','" + idtextBox.Text + "')";
                        //MessageBox.Show(sql);
                        sqlConn.Open();	//打开连接
                        SqlCommand cmd = new SqlCommand(sql, sqlConn);
                        int a = cmd.ExecuteNonQuery(); //影响行数
                        MessageBox.Show("借阅成功！");
                        BindData(); //更新数据
                        sqlConn.Close();
                    }
            }
        }
        private void lend_Click(object sender, EventArgs e)
        {
            if (idtextBox2.Text == "")
            {
                MessageBox.Show("还没有图书被选中哦！无法进行操作");
            }
            else
            {
                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
                long timeStamp = (long)(DateTime.Now - startTime).TotalMilliseconds; // 相差毫秒数

                //SqlConnection sqlConn = new SqlConnection("server=.;database=BOOK_MANAGE_SYSTEM;Trusted_Connection=SSPI");
                SqlConnection sqlConn = DBconnect.BooksystemCon();
                sqlConn.Open();	//打开连接 

                //首先查询本书的借阅时间
                SqlCommand cmd1 = new SqlCommand("select BOR_TIME from CUR_BORROW where U_ID ='" + U_ID + "' AND B_ID ='" + idtextBox2.Text + "'", sqlConn);
                SqlDataReader sdr1 = cmd1.ExecuteReader();
                sdr1.Read();
                string BOR_TIME = sdr1["BOR_TIME"].ToString().Trim();
                sqlConn.Close();

                //将本书的信息插入到历史借阅表
                string sql = "insert into HIS_BORROW(H_ID,BOR_TIME,BACK_TIME,U_ID,B_ID) values('" + timeStamp + "','" + BOR_TIME + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + U_ID + "','" + idtextBox2.Text + "')";
                //  MessageBox.Show(sql);
                sqlConn.Open();	//打开连接
                SqlCommand cmd = new SqlCommand(sql, sqlConn);
                int a = cmd.ExecuteNonQuery(); //影响行数
                sqlConn.Close();

                //然后从当前借阅表中删除本条书的记录
                string sql2 = "delete from CUR_BORROW where U_ID ='" + U_ID + "' AND B_ID ='" + idtextBox2.Text + "'";
                //  MessageBox.Show(sql2);
                sqlConn.Open();	//打开连接 
                SqlCommand cmd2 = new SqlCommand(sql2, sqlConn);
                int a2 = cmd2.ExecuteNonQuery(); //影响行数
                sqlConn.Close();

                //将用户评论存入数据库
                if (richTextBox2.Text != "") //判断用户是否有输入评论，有才插入数据库
                {
                    string sql3 = "insert into BOOK_COMMENT(BC_ID,CONTENT,TIME,BOOK_ID,USERID) values('" + timeStamp + "','" + richTextBox2.Text + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + idtextBox2.Text + "','" + U_ID + "')";
                    MessageBox.Show(sql3);
                    sqlConn.Open();	//打开连接
                    SqlCommand cmd3 = new SqlCommand(sql3, sqlConn);
                    int a3 = cmd3.ExecuteNonQuery(); //影响行数

                }
                MessageBox.Show("还书成功！");
                BindData(); //更新数据
                sqlConn.Close();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Searchtext = textBox1.Text;
            //MessageBox.Show(textBox1.Text);
            BookSearch bookdisplay = new BookSearch();
            bookdisplay.Show();
          
        }

       

        private void manageBooksToolStripMenuItem_Click(object sender, EventArgs e)
        {             
            Manage manage = new Manage();
            manage.Show();
           // this.Hide();
        }

        private void existToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定退出本系统吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                Application.Exit();
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定退出本系统吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
               // Application.Exit();
                System.Environment.Exit(0);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            MessageBox.Show("当前选中的节点是"+e.Node.Text);
        }

        

        private void manageBooktypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ADDtypes addtypes = new ADDtypes();
            addtypes.Show();
        }



        private void commentrichTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        


           

        

       
    }
}
