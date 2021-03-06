﻿using Assignment1.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Assignment1.Models
{
    public class Store
    {
        internal int StoreID { get; set; }

        internal string Name { get; set; }

        public Store(int sID) => StoreID = sID;
        public Store() { }

        public enum StoreFranchise
        {
            MelbourneCBD = 1,
            NorthMelbourne = 2,
            EastMelbourne = 3,
            SouthMelbourne = 4,
            Westmelbourne = 5

        }

        //Displays Store List for Users: FranchiseHolder and Customer
        public void GetStoreList()
        {
            string query = "SELECT * from Store;";
            SqlConnection conn = new SqlConnection(dm.ConnectionString);
            conn.Open();

            SqlCommand commd = new SqlCommand(query, conn);

            try
            {
                var StoreListTable = dm.GetTable(commd);

                Console.WriteLine("\n\n Stores \n");

                foreach (DataRow row in StoreListTable.Rows)
                {
                    var storeID = row["StoreID"].ToString();
                    var storeName = row["Name"].ToString();

                    Console.WriteLine("{0} {1} \n",
                    row["StoreID"],
                    row["Name"]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
            finally
            {
                conn.Close();
            }
        }

       
    }
}
