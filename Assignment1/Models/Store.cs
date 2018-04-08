using Assignment1.Controller;
using Assignment1.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Assignment1
{
    public class Store
    {
        private DataManager dm = DataManager.GetDataManager();

        internal int StoreID { get; }
        internal string Name { get; }

        //Constructors
        public Store(int sID) => StoreID = sID;
        public Store() { }

        //Enum StoreFranchise List
        public enum StoreFranchise
        {
            MelbourneCBD = 1,
            NorthMelbourne = 2,
            EastMelbourne = 3,
            SouthMelbourne = 4,
            WestMelbourne = 5

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
