using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DicGoods
{
    public partial class DicGoods : Form
    {
        public DicGoods()
        {
            InitializeComponent();
            DriveTreeInit();
        }

        /// <summary>
        /// первоначальное заполнение
        /// </summary>
        public void DriveTreeInit()
        {
            //ArrayList[] drivesArray = getNom("0");

            treeView1.BeginUpdate();
            treeView1.Nodes.Clear();

            foreach (string s in getNomID("0"))
            {
               TreeNode drive = new TreeNode(s, 0, 0);
                //treeView1.Nodes.Add(getNomNameFromID(drive.Text.ToString()));
                treeView1.Nodes.Add(drive);

                //MessageBox.Show(drive.Text.ToString());
                // MessageBox.Show(getNomNameFromID(drive.Text.ToString()));
                GetGoods(drive);
            }

            treeView1.EndUpdate();
        }

        /// <summary>
        /// Получение списка подпапок
        /// </summary>
        public void GetGoods(TreeNode node)
        {

            node.Nodes.Clear();

            //foreach (string s in getNomID(node.Text.ToString()))
            //{
            //    TreeNode drive = new TreeNode(s, 1, 2);
            //    node.Nodes.Add(getNomNameFromID(drive.Text.ToString()));
            //}

            foreach (string s in getNomID(node.Text.ToString()))
            {
                TreeNode drive = new TreeNode(s, 1, 2);
                node.Nodes.Add(drive);
            }
        }


        /// <summary>
        /// последующее открытие
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            treeView1.BeginUpdate();

            foreach (TreeNode node in e.Node.Nodes)
            {
                GetGoods(node);
            }

            treeView1.EndUpdate();
        }



        public static ArrayList getNomID(string ID)
        {
            ArrayList Nom = new ArrayList(); ;

            // Описание: ExecuteScalar — получение единственного значения. Firebird, InterBase .Net провайдер (c#)
            string connString = "User=SYSDBA;" +
                                "Password=masterkey;" +
                                "Charset = UTF8;" +
                                "Database=127.0.0.1:terra;" +
                                "DataSource=localhost;" +
                                "Port=3050;";
            FbConnection fb = new FbConnection(connString);

            fb.Open();
            FbCommand SelectSQL = new FbCommand("SELECT id FROM dic_goods_grp where PARENT_ID = @cust_no ORDER BY name", fb);
            //add one IN parameter                     

            FbParameter nameParam = new FbParameter("@cust_no", ID);
            // добавляем параметр к команде
            SelectSQL.Parameters.Add(nameParam);

            FbTransaction fbt = fb.BeginTransaction();
            SelectSQL.Transaction = fbt;
            FbDataReader reader = SelectSQL.ExecuteReader();
            // SelectSQL.Parameters["cust_no"].Value = reader["0"];
            try
            {

                while (reader.Read())
                {
                    Nom.Add(reader.GetString(0));
                }
            }
            finally
            {
                fbt.Commit();
                reader.Close();
                SelectSQL.Dispose();
            }

            return Nom;

        }

        public static string getNomNameFromID(string ID)
        {
            string Nom = "n/a"; 

            // Описание: ExecuteScalar — получение единственного значения. Firebird, InterBase .Net провайдер (c#)
            string connString = "User=SYSDBA;" +
                                "Password=masterkey;" +
                                "Charset = UTF8;" +
                                "Database=127.0.0.1:terra;" +
                                "DataSource=localhost;" +
                                "Port=3050;";
            FbConnection fb = new FbConnection(connString);

            fb.Open();
            FbCommand SelectSQL = new FbCommand("SELECT Name FROM dic_goods_grp where ID = @cust_no ORDER BY name", fb);
            //add one IN parameter                     
            FbParameter nameParam = new FbParameter("@cust_no", ID);
            // добавляем параметр к команде
            SelectSQL.Parameters.Add(nameParam);


            FbTransaction fbt = fb.BeginTransaction();
            SelectSQL.Transaction = fbt;
            FbDataReader reader = SelectSQL.ExecuteReader();
            // SelectSQL.Parameters["cust_no"].Value = reader["0"];
            try
            {

                while (reader.Read()) { Nom = reader.GetString(0); }
              
            }
            finally
            {
                fbt.Commit();
                reader.Close();
                SelectSQL.Dispose();
            }

            return Nom;

        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

    }
}
