using DataLayer;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ApplicationContext = DataLayer.ApplicationContext;
using rdkey;
using DicGoods;
using DataLayer;

namespace TerraClient
{
    public partial class MainForm : Form
    {
        public string path_db;
        public string user;
        public string pass;

        FbConnection fb;

        ToolStripLabel infolabel;
        ToolStripLabel datelabel;
        ToolStripLabel timelabel;
        ToolStripLabel typelabel;
        Timer timer;

        public MainForm()
        {
            InitializeComponent();
            StatusLable();
        }

        private bool CheckLic()
        {
            if (libtaryMy.CalculateMD5Hash(libtaryMy.GetProcessorIdAndGetOSSerialNumberID()) != Key.Value)
                return false;
            else
                return true;

        }

       // public delegate void ChangeRangeDateHandler(object sender);

        //public event ChangeRangeDateHandler ChangeRangeDate;

        private void MainForm_Load(object sender, EventArgs e)
        {
            #region write ini
            try
            {
                //Создание объекта, для работы с файлом
                INIManager manager = new INIManager(Application.StartupPath + @"\settings\set.ini");
                //Получить значение по ключу name из секции main
                path_db = manager.GetPrivateString("connection", "db");
                db_puth.Value = path_db;

                path_db = manager.GetPrivateString("workstation", "Key");
                Key.Value = path_db;
                //MessageBox.Show("бд - " + db_puth.Value, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                File.AppendAllText(Application.StartupPath + @"\new_file.txt", "путь к db:" + path_db + "\n");
                //Записать значение по ключу age в секции main
                // manager.WritePrivateString("main", "age", "21");
            }
            catch (Exception ex)
            {
                MessageBox.Show("ini не прочтен" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            #endregion


            var dialog = new LoginFormadd();
            //dialog.UserName = "";
            //dialog.Password = "";

            if ((dialog.ShowDialog() == DialogResult.OK))
            {
                //MessageBox.Show("Не корректный логин или пароль " + dialog.UserName + '/' + dialog.Password, "", MessageBoxButtons.OK, MessageBoxIcon.Error);


                ApplicationContext dbContext = new ApplicationContext();

                try
                {
                    //string s = dbContext.Database.Connection.ConnectionString;
                    //var builder = new FbConnectionStringBuilder(s);
                    //builder.UserID = dialog.UserName;
                    //builder.Password = dialog.Password;

                    //dbContext.Database.Connection.ConnectionString = builder.ConnectionString;

                    //// пробуем подключится
                    // dbContext.Database.Connection.Open();

                    // формируем connection string для последующего соединения с нашей базой данных
                    FbConnectionStringBuilder fb_con = new FbConnectionStringBuilder();
                    fb_con.Charset = "UTF8"; //используемая кодировка
                    fb_con.UserID = "SYSDBA"; //логин
                    fb_con.Password = "masterkey"; //пароль
                    fb_con.Database = db_puth.Value; //путь к файлу базы данных
                    fb_con.ServerType = 0; //указываем тип сервера (0 - "полноценный Firebird" (classic или super server), 1 - встроенный (embedded))
                    fb = new FbConnection(fb_con.ToString()); //передаем нашу строку подключения объекту класса FbConnection

                    DataSet dataset = new DataSet();

                    fb.Open();

                    FbCommand fbcommand = fb.CreateCommand();
                    fbcommand.CommandType = CommandType.Text;
                    fbcommand.Connection = fb;

                    fbcommand.CommandText = @"SELECT login, pass FROM sec_users WHERE Login='" + dialog.UserName + "' AND Pass='" + libtaryMy.CalculateMD5Hash(dialog.Password) + "';";

                    FbDataAdapter adaptor = new FbDataAdapter(fbcommand.CommandText, fbcommand.Connection);
                    adaptor.Fill(dataset, "0");
                    int count = dataset.Tables[0].Rows.Count;

                    FbDatabaseInfo fb_inf = new FbDatabaseInfo(fb); //информация о БД
                    typelabel.Text = "connect Info: " + fb_inf.ServerClass + "; " + fb_inf.ServerVersion + $"; Rows:{count}";
                    statusStrip1.Items.Add(typelabel);

                    if (count <= 0) 
                    {
                        MessageBox.Show("Не корректный логин или пароль " + dialog.UserName + '/' + dialog.Password, "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        MessageBox.Show("Не корректный логин или пароль " + dialog.UserName + '/' + dialog.Password, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MessageBox.Show(fbcommand.CommandText, "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        //Application.Exit();
                    }

                    if (!CheckLic())
                    {
                        MessageBox.Show(Key.Value+":"+ libtaryMy.CalculateMD5Hash(libtaryMy.GetProcessorIdAndGetOSSerialNumberID()), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                catch (Exception ex)
                {
                    // отображаем ошибку
                    MessageBox.Show(ex.Message, "Error");
                    Application.Exit();
                }

            }
            else
                Application.Exit();

        }

        private void MainForm_Load_(object sender, EventArgs e)
        {
            var dialog = new LoginFormadd();
            dialog.UserName = "sysdba";
            dialog.Password = "masterkey";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var dbContext = AppVariables.getDbContext();

                try
                {
                    string s = dbContext.Database.Connection.ConnectionString;
                    var builder = new FbConnectionStringBuilder(s);
                    builder.UserID = dialog.UserName;
                    builder.Password = dialog.Password;

                    dbContext.Database.Connection.ConnectionString = builder.ConnectionString;

                    //// пробуем подключится
                    
                    //dbContext.Database.Connection.Open();

                    //MessageBox.Show("DataLayer " + DataLayer.Sec_users. + '/' + dialog.Password, "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //edtCustomer.Text = customerForm.CurrentCustomer.NAME
                    ////////
                    //fb.Open();

                    //FbCommand fbcommand = fb.CreateCommand();
                    //fbcommand.CommandType = CommandType.Text;
                    //fbcommand.Connection = fb;

                    //fbcommand.CommandText = @"SELECT login, pass FROM sec_users WHERE Login='" + dialog.UserName + "' AND Pass='" + libtaryMy.CalculateMD5Hash(dialog.Password) + "';";

                    //FbDataAdapter adaptor = new FbDataAdapter(fbcommand.CommandText, fbcommand.Connection);
                    //adaptor.Fill(dataset, "0");
                    //int count = dataset.Tables[0].Rows.Count;

                    //FbDatabaseInfo fb_inf = new FbDatabaseInfo(fb); //информация о БД
                    //typelabel.Text = "connect Info: " + fb_inf.ServerClass + "; " + fb_inf.ServerVersion + $"; Rows:{count}";
                    //statusStrip1.Items.Add(typelabel);

                    //if (count <= 0)
                    //{
                    //    MessageBox.Show("Не корректный логин или пароль " + dialog.UserName + '/' + dialog.Password, "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //    MessageBox.Show("Не корректный логин или пароль " + dialog.UserName + '/' + dialog.Password, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    MessageBox.Show(fbcommand.CommandText, "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //    //Application.Exit();
                    //}

                    //if (!CheckLic())
                    //{
                    //    MessageBox.Show(Key.Value + ":" + libtaryMy.CalculateMD5Hash(libtaryMy.GetProcessorIdAndGetOSSerialNumberID()), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //}
                    ///////////

                }
                catch (Exception ex)
                {
                    // отображаем ошибку
                    MessageBox.Show(ex.Message, "Error");
                    // Application.Exit();
                }
            }
            else
                Application.Exit();
        }





        #region timerStrip
        private void StatusLable()
        {
            // toolStripStatusLabel1.Text = "Текущие дата и время";

            infolabel = new ToolStripLabel();
            infolabel.Text = "Текущие дата и время:";

            datelabel = new ToolStripLabel();
            timelabel = new ToolStripLabel();

            typelabel = new ToolStripLabel();

            statusStrip1.Items.Add(infolabel);
            statusStrip1.Items.Add(datelabel);
            statusStrip1.Items.Add(timelabel);

            timer = new Timer() { Interval = 1000 };
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            datelabel.Text = DateTime.Now.ToLongDateString();
            timelabel.Text = DateTime.Now.ToLongTimeString();
        }

        #endregion

        private void заказыКлиентовToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // getTabPage("Orders");
        }

        /// <summary>
        /// Добавление модальной вкладки
        /// </summary>
        /// <param name="caption"></param>
        /// <returns></returns>
       
     

        private Form createForm(string name)
        {
            Form form = null;
            switch (name)
            {
                case "Orders":
                    form = new Orders();
                    break;
                case "customers":
                    form = new Orders();
                    break;
                case "invoices":
                    form = new Orders();
                    //ChangeRangeDate += delegate (object sender) //выбор даты в зависимости от даты выставленной
                    //{
                    //    ((InvoiceForm)form).LoadInvoicesData();
                    //};

                    break;
            }
            return form;
        }

        /// <summary>
        /// форма Заказы клиентов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
     

        /// <summary>
        /// форма "о программе"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            About formAbout = new About();
            formAbout.ShowDialog(this);
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            DicGoods.DicGoods newMDIChild = new DicGoods.DicGoods();
            // Set the Parent Form of the Child window.
            newMDIChild.MdiParent = this;

            // Display the new form.
            newMDIChild.Show();
        }
    }
}
