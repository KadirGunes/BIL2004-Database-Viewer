using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SQL_FORM
{
    public partial class Form1 : Form
    {
        private SqlConnection connection;
        private bool isConnected = false;

        public Form1()
        {
            InitializeComponent();
            SelectTable.Visible = false;
            dataGridView1.ReadOnly = true;
        }

        private void Connect_Click(object sender, EventArgs e)
        {
            connection = 
                new SqlConnection("server=(local)\\SQLExpress;database=" + ConnectText.Text + ";integrated Security=SSPI;");

            try
            {
                connection.Open();
                isConnected = true;
                SelectTable.Visible = true;
                MessageBox.Show("Bağlantı gerçekleşti");
            }
            catch
            {
                MessageBox.Show("Bağlantı kurulamadı.");
                SelectTable.Visible = false;
                dataGridView1.DataSource = null;
                return;
            }
            
            DataTable tableNames = connection.GetSchema("Tables", new string[] {null, null, null, "BASE TABLE"});

            DataTableReader reader = tableNames.CreateDataReader();

            while(reader.Read())
            {
                string tableName = reader["TABLE_NAME"].ToString();
                if (tableName == "sysdiagrams") continue;

                SelectTable.Items.Add(tableName);
            }
        }

        private void SelectTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlDataAdapter adapter;

            SqlCommand command = new SqlCommand();
            command.CommandText = "select * from " + SelectTable.Text;

            adapter = new SqlDataAdapter(command.CommandText, connection);

            DataSet data = new DataSet();
            adapter.Fill(data, SelectTable.Text);
            dataGridView1.DataSource = data.Tables[SelectTable.Text];
        
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            connection.Close();
        }
    }
}