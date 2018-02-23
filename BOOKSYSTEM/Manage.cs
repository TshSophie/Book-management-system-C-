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
    public partial class Manage : Form
    {
        public Manage()
        {
            InitializeComponent();
        }

        string newfilepath = "";  //保存数据表中的图片上传的目标路径
        string oldfilepath = null;  //保存用户上传图片的来源路径
        string tempfilepath = null; //保存点击数据表时的图片路径

        private void BindData()
        {
            //实例化SqlConnection变量sqlConn，连接数据库
           // SqlConnection sqlConn = new SqlConnection("server=.;database=BOOK_MANAGE_SYSTEM;Trusted_Connection=SSPI");
            SqlConnection sqlConn = DBconnect.BooksystemCon();
            //创建一个SqlDataAdapter对象
            SqlDataAdapter sda = new SqlDataAdapter("select B_ID,NAME,AUTHOR,PUBLISHER,NUMBER,PUB_TIME,TYPE,STORAGE,IMAGES from BOOKS", sqlConn);
            //创建一个DataSet对象
            DataSet ds = new DataSet();
            //使用SqlDataAdapter对象的Fill方法填充DataSetS
            sda.Fill(ds, "BOOKS");
            //设置dataGridView1控件数据源
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void Manage_Load(object sender, EventArgs e)
        {
            addbutton.Enabled = true;
            changebutton.Enabled = false;
            deletebutton.Enabled = false;
            searchbutton.Enabled = true;
            savebutton.Enabled = false;
            cancelbutton.Enabled = false ;

            BOOKIDtextBox.Enabled = false;
            BOOKNAMEtextBox.Enabled = false;
            AUTHORtextBox.Enabled = false;
            PUBLISHERtextBox.Enabled = false;
            STORAGEtextBox.Enabled = false;
            numtextBox.Enabled = false;
            dateTimePicker1.Enabled = false;
            TYPEcomboBox.Enabled = false;
            PICTUREbutton.Enabled = false;

            //从数据库中读取图书类型，并放入下拉菜单中
            TYPEcomboBox.DropDownStyle = ComboBoxStyle.DropDownList;
          //  SqlConnection sqlConn = new SqlConnection("server=.;database=BOOK_MANAGE_SYSTEM;Trusted_Connection=SSPI");
            SqlConnection sqlConn = DBconnect.BooksystemCon();
            sqlConn.Open();	//打开连接
            SqlCommand cmd = new SqlCommand("select NAME from BOOK_TYPES ", sqlConn);
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                TYPEcomboBox.Items.Add(sdr["NAME"].ToString().Trim());            
            }
           
           

            //TYPEcomboBox.SelectedIndex = 0;
        }

     
   

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr;
            dr = MessageBox.Show("确认退出吗？", "确认对话框", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                Mainform main = new Mainform();
                main.Show();
                this.Close();
            }
            else
            {
                e.Cancel = true;
            }
        }

       
        private void button1_Click_1(object sender, EventArgs e)
        {
            BOOKIDtextBox.Text = "";
            BOOKNAMEtextBox.Text = "";
            AUTHORtextBox.Text = "";
            PUBLISHERtextBox.Text = "";
            STORAGEtextBox.Text = "";
            numtextBox.Text = null;
            newfilepath = "";

            addbutton.Enabled = false;
            savebutton.Enabled = true;
            cancelbutton.Enabled = true;
            searchbutton.Enabled = false;
            deletebutton.Enabled = false;
            changebutton.Enabled = false;


            BOOKIDtextBox.Enabled = true;
            BOOKNAMEtextBox.Enabled = true;
            AUTHORtextBox.Enabled = true;
            PUBLISHERtextBox.Enabled = true;
            STORAGEtextBox.Enabled = true;
            numtextBox.Enabled = true;
            dateTimePicker1.Enabled = true;
            TYPEcomboBox.Enabled = true;
            PICTUREbutton.Enabled = true;

        }



        private void savebutton_Click(object sender, EventArgs e)
        {
             if (BOOKIDtextBox.Text=="")
             {
                 MessageBox.Show("还没有图书被选中哦！无法进行操作");
            
             }else{
            //实例化SqlConnection变量sqlConn，连接数据库
           // SqlConnection sqlConn = new SqlConnection("server=.;database=BOOK_MANAGE_SYSTEM;Trusted_Connection=SSPI");
            SqlConnection sqlConn = DBconnect.BooksystemCon();
            sqlConn.Open();	//打开连接
            SqlCommand cmd = new SqlCommand("select count(*) from BOOKS where NAME='" + BOOKNAMEtextBox.Text + "'", sqlConn);
            int i = Convert.ToInt32(cmd.ExecuteScalar());
              
            SqlCommand cmd1 = new SqlCommand("select ID from BOOK_TYPES WHERE NAME ='" + TYPEcomboBox.Text + "'", sqlConn);
            SqlDataReader sdr = cmd1.ExecuteReader();
            sdr.Read();
            string Type = sdr["ID"].ToString().Trim();
           // MessageBox.Show(Type);

            sqlConn.Close();
           // MessageBox.Show( Convert.ToString(i));
            if (i > 0)
            {  
                if (tempfilepath != oldfilepath) //当用户更改了图片时才执行此步操作
                { //tempfilepath为用户点击dategridview时，传过来的路径，即原来数据库中保存的那个;
                    //oldfilepath为用户点击选择图片按钮时选择的图片路径
                    //如果用户点击了选择图片按钮，则oldfilepat会发生改变
                    MessageBox.Show(tempfilepath);
                    Filedealing upfile = new Filedealing();
                    newfilepath = upfile.uploadfile(oldfilepath); //将用户选定图片上传至系统制定目录后，将其完整的路径赋给一个变量                        
                    this.pictureBox1.Load(newfilepath);
                    FileInfo delete = new FileInfo(tempfilepath);
                    delete.Delete(); //删除记录对于的图片文件
                    MessageBox.Show("图片更新成功！" + tempfilepath);
                    sqlConn.Open();	//打开连接
                    string sql = "update BOOKS set NAME='" + BOOKNAMEtextBox.Text.Trim() + "',AUTHOR='" + AUTHORtextBox.Text.Trim() + "',PUBLISHER='" + PUBLISHERtextBox.Text.Trim() + "',NUMBER=" + numtextBox.Text.Trim() + ",TYPE='" + Type.Trim() + "',PUB_TIME='" + dateTimePicker1.Text.Trim() + "',STORAGE='" + STORAGEtextBox.Text.Trim() + "',IMAGES='" + newfilepath.Trim() + "' where B_ID='" + dataGridView1.SelectedCells[0].Value.ToString().Trim() + "'";
                    SqlCommand cmd2 = new SqlCommand(sql, sqlConn);
                    cmd2.ExecuteNonQuery();
                    MessageBox.Show(newfilepath);
                    sqlConn.Close();
                    BindData();
                }
                else
                {
                    MessageBox.Show(tempfilepath);
                    sqlConn.Open();	//打开连接
                    string sql = "update BOOKS set NAME='" + BOOKNAMEtextBox.Text.Trim() + "',AUTHOR='" + AUTHORtextBox.Text.Trim() + "',PUBLISHER='" + PUBLISHERtextBox.Text.Trim() + "',NUMBER=" + numtextBox.Text.Trim() + ",TYPE='" + Type.Trim() + "',PUB_TIME='" + dateTimePicker1.Text.Trim() + "',STORAGE='" + STORAGEtextBox.Text.Trim() + "',IMAGES='" + tempfilepath.Trim() + "' where B_ID='" + dataGridView1.SelectedCells[0].Value.ToString().Trim() + "'";
                    MessageBox.Show(sql);
                    SqlCommand cmd2 = new SqlCommand(sql, sqlConn);
                    int a = cmd2.ExecuteNonQuery();
                    MessageBox.Show("affect---" + a);
                    sqlConn.Close();
                    BindData();
                }
                
                       
                addbutton.Enabled = true;
                deletebutton.Enabled = false;
                changebutton.Enabled = false;
                searchbutton.Enabled = true;
                savebutton.Enabled = false ;
                cancelbutton.Enabled = true;
                BOOKNAMEtextBox.Enabled = false;
            }
            else
            {
                Filedealing upfile = new Filedealing();
                newfilepath = upfile.uploadfile(oldfilepath); //将用户选定图片上传至系统制定目录后，将其完整的路径赋给一个变量                        
                MessageBox.Show(newfilepath);
                sqlConn.Open();	//打开连接
                string sql = "insert into BOOKS(B_ID,NAME,AUTHOR,PUBLISHER,NUMBER,TYPE,PUB_TIME,STORAGE,IMAGES ) values('" + BOOKIDtextBox.Text.Trim() + "','" + BOOKNAMEtextBox.Text.Trim() + "','" + AUTHORtextBox.Text.Trim() + "','" + PUBLISHERtextBox.Text.Trim() + "','" + numtextBox.Text.Trim() + "','" + Type.Trim() + "','" + dateTimePicker1.Text.Trim() + "','" + STORAGEtextBox.Text.Trim() + "','" + newfilepath.Trim() + "')";      
                MessageBox.Show(sql);
                cmd = new SqlCommand(sql, sqlConn);
                int a = cmd.ExecuteNonQuery();
                MessageBox.Show("affect---"+a);
                sqlConn.Close();              
                BindData();
               
                addbutton.Enabled = true;
                deletebutton.Enabled = false;
                changebutton.Enabled = false;
                searchbutton.Enabled = true;
                savebutton.Enabled = false;
                cancelbutton.Enabled = true;
                BOOKNAMEtextBox.Enabled = false;
            }
          }
        }
        private void button1_Click(object sender, EventArgs e)
        {

            //将用户选择的图片上传到系统指定目录            

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "所有文件(*jpg*)|*.jpg*"; //设置要选择的文件的类型
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                oldfilepath = fileDialog.FileName;//返回文件的完整路径   
                // MessageBox.Show("文件的路径是：" + oldfilepath);
            }
            this.pictureBox1.Load(oldfilepath);

        }
        private void changebutton_Click(object sender, EventArgs e)
        {
            oldfilepath = tempfilepath;
            addbutton.Enabled = false;
            deletebutton.Enabled = false;
            searchbutton.Enabled = false;
            savebutton.Enabled = true;
            cancelbutton.Enabled = true;

            BOOKIDtextBox.Enabled = false;
            BOOKNAMEtextBox.Enabled = false;
            AUTHORtextBox.Enabled = true;
            PUBLISHERtextBox.Enabled = true;
            STORAGEtextBox.Enabled = true;
            numtextBox.Enabled = true;
            dateTimePicker1.Enabled = true;
            TYPEcomboBox.Enabled = true;
            PICTUREbutton.Enabled = true;
           
            

        }

        private void searchbutton_Click(object sender, EventArgs e)
        {
            addbutton.Enabled = true;
            deletebutton.Enabled = true;
            changebutton.Enabled = true;
            searchbutton.Enabled = true;
            savebutton.Enabled = false;
            cancelbutton.Enabled = true;

            BOOKIDtextBox.Enabled = false;
            BOOKNAMEtextBox.Enabled = false;
            AUTHORtextBox.Enabled = false;
            PUBLISHERtextBox.Enabled = false;
            STORAGEtextBox.Enabled = false;
            numtextBox.Enabled = false;
            dateTimePicker1.Enabled = false;
            TYPEcomboBox.Enabled = false;
            PICTUREbutton.Enabled = false;
            BindData(); //查询所有数据  
        }

        private void cancelbutton_Click(object sender, EventArgs e)
        {
            addbutton.Enabled = true;
            changebutton.Enabled = false;
            deletebutton.Enabled = false;
            searchbutton.Enabled = true;
            savebutton.Enabled = false;
            cancelbutton.Enabled = true;

            BOOKIDtextBox.Enabled = false;
            BOOKNAMEtextBox.Enabled = false;
            AUTHORtextBox.Enabled = false;
            PUBLISHERtextBox.Enabled = false;
            STORAGEtextBox.Enabled = false;
            numtextBox.Enabled = false;
            dateTimePicker1.Enabled = false;
            TYPEcomboBox.Enabled = false;
            PICTUREbutton.Enabled = false;

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //实例化SqlConnection变量sqlConn，连接数据库
           // SqlConnection sqlConn = new SqlConnection("server=.;database=BOOK_MANAGE_SYSTEM;Trusted_Connection=SSPI");
            SqlConnection sqlConn = DBconnect.BooksystemCon();
            sqlConn.Open();	//打开连接
            SqlCommand cmd1 = new SqlCommand("select NAME from BOOK_TYPES WHERE ID =" + dataGridView1.SelectedCells[6].Value.ToString().Trim() , sqlConn);
            SqlDataReader sdr = cmd1.ExecuteReader();
            sdr.Read();           
            string Type = sdr["NAME"].ToString().Trim();
            sqlConn.Close();
            BOOKIDtextBox.Text = dataGridView1.SelectedCells[0].Value.ToString();
            BOOKNAMEtextBox.Text = dataGridView1.SelectedCells[1].Value.ToString();
            AUTHORtextBox.Text = dataGridView1.SelectedCells[2].Value.ToString();
            PUBLISHERtextBox.Text = dataGridView1.SelectedCells[3].Value.ToString().Trim();
            numtextBox.Text = dataGridView1.SelectedCells[4].Value.ToString().Trim();
            dateTimePicker1.Text = dataGridView1.SelectedCells[5].Value.ToString().Trim();
            TYPEcomboBox.Text = Type;
            STORAGEtextBox.Text = dataGridView1.SelectedCells[7].Value.ToString().Trim();
            tempfilepath = dataGridView1.SelectedCells[8].Value.ToString().Trim();
            FileStream pFileStream = new FileStream(tempfilepath, FileMode.Open, FileAccess.Read);
            pictureBox1.Image = Image.FromStream(pFileStream);
            pFileStream.Close();
            pFileStream.Dispose();

            changebutton.Enabled = true;
            deletebutton.Enabled = true;
          //  MessageBox.Show(dataGridView1.SelectedCells[5].Value.ToString().Trim());
           
        }

        private void deletebutton_Click(object sender, EventArgs e)
        {
            if (BOOKIDtextBox.Text == "")            
             {
                MessageBox.Show("还没有图书被选中哦！无法进行操作");

             }
             else
              {
                //实例化SqlConnection变量sqlConn，连接数据库
                // SqlConnection sqlConn = new SqlConnection("server=.;database=BOOK_MANAGE_SYSTEM;Trusted_Connection=SSPI");
                SqlConnection sqlConn = DBconnect.BooksystemCon();
                sqlConn.Open();	//打开连接
                string sql = "delete from books where B_ID='" + dataGridView1.SelectedCells[0].Value.ToString() + "'";
                string sql1 = "delete from BOOK_COMMENT where BOOK_ID = '" + BOOKIDtextBox.Text + "'";
                string sql2 = "delete from CUR_BORROW where B_ID = '" + BOOKIDtextBox.Text + "'";
                string sql3 = "delete from HIS_BORROW where B_ID = '" + BOOKIDtextBox.Text + "'";
                MessageBox.Show(dataGridView1.SelectedCells[0].Value.ToString() + sql);
                SqlCommand cmd = new SqlCommand(sql1, sqlConn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand(sql2, sqlConn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand(sql3, sqlConn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand(sql, sqlConn);
                cmd.ExecuteNonQuery();
                sqlConn.Close();
                pictureBox1.Image = null;
                FileInfo delete = new FileInfo(tempfilepath);
                delete.Delete();
                MessageBox.Show("删除成功！" + tempfilepath);
                BindData();
            }
            
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void BOOKIDtextBox_TextChanged(object sender, EventArgs e)
        {

        }

       

      

       



    }
}
