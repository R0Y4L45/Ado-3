using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace ADO_3
{
    public partial class Form1 : Form
    {
        DbConnection? conn = null;
        DbProviderFactory? factory = null;
        DbDataAdapter? adapter = null;
        DbCommand? cmd = null;
        DataSet? dataSet = null;
        List<DataTable>? lTable = null;
        int count = 0;
        string? connection = null;
        string? pName;
        public Form1()
        {
            InitializeComponent();

            connection = ConfigurationManager.AppSettings.Get("Key");

            comboBox1.Items.Add("Sql");
            comboBox1.Items.Add("OleDb");
        }

        private void ConnectionString()
        {
            if (string.IsNullOrWhiteSpace(pName))
                return;

            factory = DbProviderFactories.GetFactory(pName!);

            conn = factory.CreateConnection();
            conn!.ConnectionString = connection;
        }

        private void AdapterConfig()
        {
            try
            {
                adapter = factory!.CreateDataAdapter();

                cmd = factory.CreateCommand();

                cmd!.CommandText = textBox1.Text;
                cmd.Connection = conn;

                dataSet = new DataSet();

                adapter!.SelectCommand = cmd;
                adapter.Fill(dataSet!);

                foreach (DataTable dt in dataSet.Tables)
                {
                    TabPage page = new("Table " + (++count).ToString())
                    {
                        AutoScroll = true
                    };
                    DataGridView data = new()
                    {
                        AutoSize = true,
                    };

                    data.DataSource = dt;
                    page.Controls.Add(data); 
                    tabControl1.Controls.Add(page);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally 
            {
                cmd?.Dispose();
                adapter?.Dispose();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConnectionString();
            AdapterConfig();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "Sql")
            {
                pName = ConfigurationManager.AppSettings.Get("Sql")!;
                DbProviderFactories.RegisterFactory(pName, typeof(SqlClientFactory));
                textBox1.Enabled = true;
                button1.Enabled = true;
            }
            else if (comboBox1.SelectedItem.ToString() == "OleDb")
                MessageBox.Show("OleDb will be here soon..)");
        }
    }
}