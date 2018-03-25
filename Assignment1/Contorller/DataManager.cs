using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Assignment1.Models;


namespace Assignment1.Contorller
{


    public class DataManager
    {
        public List<Inventory> Items = new List<Inventory>(); 

        //private constructor: Singleton Pattern
        private DataManager()
        {
            //create a connection object
            string ConnectonString = "server=wdt2018.australiaeast.cloudapp.azure.com;uid=s3419529;database=s3419529;pwd=abc123;";

            SqlConnection conn = new SqlConnection(ConnectonString);

            //open the connection
            conn.Open();

            //create a DataAdpater for Inventory table
            SqlDataAdapter InventoryAdapter = new SqlDataAdapter("select * from Inventory", conn);

            // the CommandBuilder object
            SqlCommandBuilder commandBld = new SqlCommandBuilder(InventoryAdapter);

            //create a Dataset object
            DataSet dataset = new DataSet("InventorySet");

            try
            {
                // InventoryTable is the name of the DataTable object within Dataset
                InventoryAdapter.Fill(dataset, "InventoryTable");

            }
            catch (SqlException se)
            {
                Console.WriteLine("SQL Exception: {0}", se.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
            finally
            {
            }

            //create a table object
            DataTable InventoryTable = dataset.Tables["InventoryTable"];

            //fetch data into the List
            foreach (DataRow row in InventoryTable.Rows)
            {
                string pName = row["Name"].ToString();
                string pID = row["InventoryID"].ToString();
                string pStock = row["CurrentStock"].ToString();

                Inventory item = new Inventory((Int32.Parse(pID), pName, Int32.Parse(pStock));
                Items.Add(item);

            }

        }

        //Singleton pattern signature method
        public static DataManager GetDataManager(){
            
            DataManager dm = new DataManager();
            return dm;

        }

    }
}
