﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Assignment1.Contorller;


namespace Assignment1.Models
{
    public class FranchiseHolder
    {
        public Store store { get; set; }
        private DataManager dm = DataManager.GetDataManager();
        public List<Inventory> StoreItems = new List<Inventory>();
        public enum storeList{MelbourneCBD=1,EastMelbourne,NorthMelbourne,SouthMelbourne,WestMelbourne }



        //Franchise Holder should take store obj as parameters to construct
        public FranchiseHolder()
        {
            //store.StoreID = storeID;
          
        }


        //check store inventory

        public void checkStoreInventory(int storeID)
        {
            string query = "select Product.ProductID, Product.Name,StoreInventory.StockLevel from Product JOIN StoreInventory ON Product.ProductID = StoreInventory.ProductID where StoreInventory.StoreID = @storeID;";
            SqlConnection conn = new SqlConnection(dm.ConnectionString);
            conn.Open();

            //parameterized Sql
            SqlCommand commd = new SqlCommand(query, conn);
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@storeID";
            param.SqlDbType = SqlDbType.Int;
            param.Direction = ParameterDirection.Input;
            param.Value = storeID;

            commd.Parameters.Add(param);

            try{
                var FranchiseHolderTable = dm.GetTable(commd);

                foreach (DataRow row in FranchiseHolderTable.Rows)
                {
                    var pID = row["ProductID"].ToString();
                    var pName = row["Name"].ToString();
                    var pStock = row["StockLevel"].ToString();

                    Inventory item = new Inventory(Int32.Parse(pID), pName, Int32.Parse(pStock));

                    StoreItems.Add(item);

                    Console.WriteLine("{0} {1} {2}\n",
                                  row["ProductID"],
                                  row["Name"], row["StockLevel"]);

                }



            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
            finally{
                conn.Close();
            }

        }




        //add new stock request to owner



        //add new inventory item



    }
}
