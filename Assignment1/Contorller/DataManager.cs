using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Assignment1.Models;


namespace Assignment1.Contorller
{


    public class DataManager
    {


        public string ConnectionString = "server=wdt2018.australiaeast.cloudapp.azure.com;uid=s3419529;database=s3419529;pwd=abc123;";

        //Constructor
        private DataManager()
        {
            //SqlConnection conn = new SqlConnection(ConnectionString);
            //conn.Open();

        }



        /* R -- retrive */
        //fetch table by query and connectionString
        public DataTable fetchData(string query, string connString){
            try
            {
                
                SqlConnection conn = new SqlConnection(connString);
                SqlCommand comd = new SqlCommand(query, conn);
                conn.Open();

                DataSet ds = new DataSet();
                DataTable table = new DataTable("Table");//need to register a new table for next fetch
                SqlDataAdapter da = new SqlDataAdapter(comd);
                da.Fill(ds,"Table");
                table = ds.Tables["Table"];

                conn.Close();
                conn.Dispose();

                return table;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
                return null;
            }

        }

        //A method fetch the table BY Command obj
        public DataTable GetTable(SqlCommand cmmd){

            var table = new DataTable();
  
            new SqlDataAdapter(cmmd).Fill(table);

            return table;
        }



        /* U - update */
        public int updateData(string query, string connString)
        {
            int update = 0;
            try
            {
                SqlConnection conn = new SqlConnection(connString);
                SqlCommand comd = new SqlCommand(query, conn);
                conn.Open();
                update = comd.ExecuteNonQuery();
                conn.Close();

                return update;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
                return -1;
            }

        }




        //Singleton pattern signature method
        public static DataManager GetDataManager(){
            
            DataManager dm = new DataManager();
            return dm;

        }

    }
}
