using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TerraClient
{
    public partial class LoginFormadd : Form
    {
        public LoginFormadd()
        {
            InitializeComponent();
        }

        public string UserName
        {
            get
            {
                return edtUserName.Text;
            }
            set
            {
                edtUserName.Text = value;
            }
        }

        public string Password
        {
            get
            {
                return edtPassword.Text;
            }
            set
            {
                edtPassword.Text = value;
            }
        }

        //private void LoginForm_Load(object sender, EventArgs e)
        //{

        //    try
        //    {
        //        //SqlConnection connection = new SqlConnection();
        //        //SqlCommand comand = new SqlCommand();
        //        //SqlDataAdapter adaptor = new SqlDataAdapter();
        //        //DataSet dataset = new DataSet();

        //        //connection.ConnectionString = (@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + Application.StartupPath + db_puth.Value + @";Integrated Security=True;Connect Timeout=30");
        //        //comand.CommandText = @"SELECT Login FROM [sys_users] order by Login;";
        //        //connection.Open();

        //        //comand.Connection = connection;

        //        //adaptor.SelectCommand = comand;
        //        //adaptor.Fill(dataset, "0");
        //        //int count = dataset.Tables[0].Rows.Count;

        //        //for (int i = 0; i < count; i++)
        //        //{
        //        //    string user = dataset.Tables[0].Rows[i].ItemArray[0].ToString();
        //        //    edtUserName.Items.Add(user);
        //        //    //MessageBox.Show("Логин " + user, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        //}
        //        //edtUserName.SelectedIndex = 0;



        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Ошибка " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }


        //}

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

 

        private void edtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOK.PerformClick();
            }
        }

      
    }
}
