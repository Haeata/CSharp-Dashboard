using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using DBUtil;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.WinForms;

namespace Dashboard
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            angularGauge1.Hide();
            angularGauge2.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        private OracleConnection conn;

        private void button1_Click(object sender, EventArgs e)
        {
            string usr = textBox1.Text;
            string pwd = textBox2.Text;
            try
            {
                conn = new OracleConnection();
                conn = DBUtils.GetDBConnection(usr, pwd);
                conn.Open(); //Check connection status
                label4.Text = "Status Connected....";
                panel4.Visible = true;
                tabControl1.Visible = true;
                panel3.Visible = true;

            }
            catch (Exception ex)
            {
                label4.Text = "Status Not Connected....";
            }
            conn.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "SELECT * FROM products;";    //Create sql query
                OracleCommand cmd = new OracleCommand(sql, conn);
                OracleDataAdapter pAdap = new OracleDataAdapter();
                pAdap.SelectCommand = cmd;

                DataSet ds = new DataSet("dsProds");
                pAdap.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Query Failed :( " + ex.ToString());
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "SELECT * FROM Orders;";    //Create sql query
                OracleCommand cmd = new OracleCommand(sql, conn);
                OracleDataAdapter pAdap = new OracleDataAdapter();
                pAdap.SelectCommand = cmd;

                DataSet ds = new DataSet("dsOrders");
                pAdap.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Query Failed :( " + ex.ToString());
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Creat a live chart 
            cartesianChart1.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "2016",
                    Values = new ChartValues<double> { 42, 6, 5, 2 ,4 }
                },
                new LineSeries
                {
                    Title = "2017",
                    Values = new ChartValues<double> { 6, 7, 3, 4 ,6 },
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "2018",
                    Values = new ChartValues<double> { 4,2,7,2,7 },
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 15
                }
            };
        }

        private void pieChart1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            Func<ChartPoint, string> labelPoint = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

            pieChart1.Series = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Maria",
                    Values = new ChartValues<double> {3},
                    PushOut = 15,
                    DataLabels = true,
                    LabelPoint = labelPoint
                },
                new PieSeries
                {
                    Title = "Charles",
                    Values = new ChartValues<double> {4},
                    DataLabels = true,
                    LabelPoint = labelPoint
                },
                new PieSeries
                {
                    Title = "Frida",
                    Values = new ChartValues<double> {6},
                    DataLabels = true,
                    LabelPoint = labelPoint
                },
                new PieSeries
                {
                    Title = "Frederic",
                    Values = new ChartValues<double> {2},
                    DataLabels = true,
                    LabelPoint = labelPoint
                }
            };

            pieChart1.LegendLocation = LegendLocation.Bottom;

        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {   
                //Create new data set
                DataSet ds = new DataSet();
                string sql = "SELECT product_name, units_in_stock, units_on_order FROM products";

                // Use DBUtils to get query
                ds = DBUtils.GetSQLDataSet(sql, conn);
                dataGridView1.DataSource = ds.Tables[0];

                label5.Text = "Products In Stock";
                cartesianChart1 = DBUtils.InitialiseCartesianChart(cartesianChart1);
                cartesianChart1 = DBUtils.GetLineChartSingleDataSeries(cartesianChart1, 0, "In Stock", 2, ds, "15");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Query Failed :(" + ex.ToString());
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                //CREATE DATA SET
                DataSet ds = new DataSet();

                //Create Sql Query
                string sql = "SELECT product_name, units_in_stock, units_on_order FROM products";

                // Use DBUtils to get query
                ds = DBUtils.GetSQLDataSet(sql, conn);
                dataGridView1.DataSource = ds.Tables[0];

                //create cartesian double line chart for display
                label5.Text = "Products In Stock vs On Order";
                cartesianChart1 = DBUtils.InitialiseCartesianChart(cartesianChart1);

                cartesianChart1 = DBUtils.GetLineChartDoubleDataSeries(cartesianChart1, 0, 1, 2, "In Stock", "On Order", ds, "10");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Query Failed :(" + ex.ToString());
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();

                //Create Sql Query
                string sql = "SELECT product_name, units_in_stock, units_on_order FROM products";

                // Use DBUtils to get query. Connects and pulls across
                ds = DBUtils.GetSQLDataSet(sql, conn);

                //Output query results to datagrid
                dataGridView1.DataSource = ds.Tables[0];

                //Create column chart
                label5.Text = "Products In Stock";
                cartesianChart2 = DBUtils.InitialiseCartesianChart(cartesianChart2);
                cartesianChart2 = DBUtils.GetColumnChartSingleDataSeries(cartesianChart2,0, "In Stock", 1, ds, "10");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Query Failed :(" + ex.ToString());
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();

                //Create Sql Query
                string sql = "SELECT product_name, units_in_stock, units_on_order FROM products";

                // Use DBUtils to get query. Connects and pulls across
                ds = DBUtils.GetSQLDataSet(sql, conn);

                //Output query results to datagrid
                dataGridView1.DataSource = ds.Tables[0];

                //Create column chart
                label5.Text = "Products In Stock vs On Order";
                cartesianChart2 = DBUtils.InitialiseCartesianChart(cartesianChart2);
                cartesianChart2 = DBUtils.GetColumnChartDoubleDataSeries(cartesianChart2, 0, 1, 2, "In Stock", "On Order", ds, "10");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Query Failed :(" + ex.ToString());
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();

                //Create Sql Query
                string sql = "SELECT product_name, units_in_stock, units_on_order FROM products";

                // Use DBUtils to get query. Connects and pulls across
                ds = DBUtils.GetSQLDataSet(sql, conn);

                //Output query results to datagrid
                dataGridView1.DataSource = ds.Tables[0];

                //Create column chart
                label5.Text = "Products In Stock";
                cartesianChart2 = DBUtils.InitialiseCartesianChart(cartesianChart2);
                cartesianChart2 = DBUtils.GetRowChartSingleDataSeries(cartesianChart2, 0, "InStock", 1, ds, "5");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Query Failed :(" + ex.ToString());
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();

                //Create Sql Query
                string sql = "SELECT product_name, units_in_stock, units_on_order FROM products";

                // Use DBUtils to get query. Connects and pulls across
                ds = DBUtils.GetSQLDataSet(sql, conn);

                //Output query results to datagrid
                dataGridView1.DataSource = ds.Tables[0];

                //Create column chart
                label5.Text = "Products In Stock vs On Order";
                cartesianChart2 = DBUtils.InitialiseCartesianChart(cartesianChart2);
                cartesianChart2 = DBUtils.GetRowChartDoubleDataSeries(cartesianChart2, 0, 1, 2, "In Stock", "On Order", ds, "5");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Query Failed :(" + ex.ToString());
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();

                //Create Sql Query
                string sql = "SELECT product_name, units_in_stock, units_on_order FROM products WHERE product_name = 'Chang'";

                // Use DBUtils to get query. Connects and pulls across
                ds = DBUtils.GetSQLDataSet(sql, conn);

                //Output query results to datagrid
                dataGridView1.DataSource = ds.Tables[0];
                DataTable dt = ds.Tables[0]; //Row 0, col 0
                String title = dt.Rows[0][0].ToString(); //Title is the product name
                int inStock = int.Parse(dt.Rows[0][1].ToString()); //Row 0, col 1
                int onOrder = int.Parse(dt.Rows[0][2].ToString()); //Row 0, col 2

                //Create angular chart
                label6.Text = " Amount In Stock for: " + title;
                angularGauge1 = DBUtils.SetAngularChart(angularGauge1, inStock, 200);
                angularGauge1.Show();

                label7.Text = " Amount On Order for: " + title;
                angularGauge2 = DBUtils.SetAngularChart(angularGauge2, onOrder, 200);
                angularGauge2.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Query Failed :(" + ex.ToString());
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            
        }
    }
}
