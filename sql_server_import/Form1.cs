using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sql_server_import
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static DataTable RetrieveSourceData(String filename)


        {
            //connection string changes depending on the operation  
            //system you are running  
            string sourceConnString = @"Provider=Microsoft.Jet.OLEDB.4.0; 
                                        Data Source=e:\; 
                                        Extended Properties=text;";
            DataTable sourceData = new DataTable();
            using (OleDbConnection conn =
                           new OleDbConnection(sourceConnString))
            {
                conn.Open();
                // Get the data from the source table as a SqlDataReader. 
                OleDbCommand command = new OleDbCommand(
                                                        @"SELECT * from "+filename, conn);
                OleDbDataAdapter adapter = new OleDbDataAdapter(command);

                adapter.Fill(sourceData);

                conn.Close();
            }
            return sourceData;
        }


        public static void CopyData(DataTable sourceData)
            
        {
            string destConnString =  @"Password=123456;  
                                     Persist Security Info=True; 
                                     User ID=etl;Initial Catalog=RBN; 
                                     Data Source=INTEL\OH2BBT";

            // Set up the bulk copy object.  
               using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destConnString))
            {
                bulkCopy.DestinationTableName = "dbo.rbn_data";
                // Guarantee that columns are mapped correctly by 
                // defining the column mappings for the order. 
                bulkCopy.ColumnMappings.Add("callsign", "callsign");
                bulkCopy.ColumnMappings.Add("de_pfx", "de_pfx");
                bulkCopy.ColumnMappings.Add("de_cont", "de_cont");
                bulkCopy.ColumnMappings.Add("freq", "freq");
                bulkCopy.ColumnMappings.Add("band", "band");
               bulkCopy.ColumnMappings.Add("dx", "dx");
                bulkCopy.ColumnMappings.Add("dx_pfx", "dx_pfx");
                bulkCopy.ColumnMappings.Add("dx_cont", "dx_cont");
                bulkCopy.ColumnMappings.Add("mode", "mode");
                bulkCopy.ColumnMappings.Add("db", "db");
                bulkCopy.ColumnMappings.Add("date", "date");
              bulkCopy.ColumnMappings.Add("speed", "speed");
                bulkCopy.ColumnMappings.Add("tx_mode", "tx_mode");
                //  Write from the source to the destination. 
                bulkCopy.WriteToServer(sourceData);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            DataTable data = RetrieveSourceData("20140821.csv");
                   CopyData(data);

        }
    }
}
