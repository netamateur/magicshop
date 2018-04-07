﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Assignment1.Controller;


namespace Assignment1.Models
{
    public class FranchiseHolder
    {
        public Store store { get; set; }
        private DataManager dm = DataManager.GetDataManager();
        public List<Inventory> StoreItems = new List<Inventory>();
        public List<Inventory> OptionalItems = new List<Inventory>();

        public enum storeList{MelbourneCBD=1,EastMelbourne=2,NorthMelbourne=3,SouthMelbourne=4,WestMelbourne=5 }

        public List<Inventory> thresholdItems = new List<Inventory>();

        //Franchise Holder should take store obj as parameters to construct
        public FranchiseHolder()
        {
            //store.StoreID = storeID;
          
        }


        //check store inventory
        public void checkStoreInventory(int storeID)
        {
            string query = "SELECT Product.ProductID, Product.Name,StoreInventory.StockLevel from Product JOIN StoreInventory ON Product.ProductID = StoreInventory.ProductID where StoreInventory.StoreID = @storeID;";
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

            try
            {
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
            finally
            {
                conn.Close();
            }

        }


        //View Stock Request Threshold
        public void getStockThreshold(int threshold, int currentStoreId)
        {
            string query = "select Product.ProductID, Product.Name, StoreInventory.StockLevel from Product JOIN StoreInventory ON Product.ProductID = StoreInventory.ProductID where StoreInventory.StoreID = @currentStoreId AND StockLevel < @threshold;";
            SqlConnection conn = new SqlConnection(dm.ConnectionString);
            conn.Open();

            //parameterized Sql
            SqlCommand commd = new SqlCommand(query, conn);

            SqlParameter param = new SqlParameter();
            param.ParameterName = "@threshold";
            param.SqlDbType = SqlDbType.Int;
            param.Direction = ParameterDirection.Input;
            param.Value = threshold;

            SqlParameter param2 = new SqlParameter();
            param2.ParameterName = "@currentStoreId";
            param2.SqlDbType = SqlDbType.Int;
            param2.Direction = ParameterDirection.Input;
            param2.Value = currentStoreId;

            commd.Parameters.Add(param);
            commd.Parameters.Add(param2);

            try
            {
                var ThresholdTable = dm.GetTable(commd);

                foreach (DataRow row in ThresholdTable.Rows)
                {
                    var pID = row["ProductID"].ToString();
                    var pName = row["Name"].ToString();
                    var pStock = row["StockLevel"].ToString();

                    Inventory item = new Inventory(Int32.Parse(pID), pName, Int32.Parse(pStock));

                    thresholdItems.Add(item);

                    Console.WriteLine("{0} {1} {2}\n",
                                  row["ProductID"],
                                  row["Name"], row["StockLevel"]);
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

        //Add Stock Request to Owner - Threshold--- remove storeID
        public void addStockRequest(int productID, int quantity, int storeID)
        {
            //add to stockrequest table
            string query = "INSERT INTO StockRequest (StoreID, ProductID, Quantity) Values(@currentStoreID, @productID, @quantity);";

            //foreach (Inventory item in StoreItems)
            //{
            //    check if inserted id is same as productID in that store
            //    if (productID == item.ProductID)
            //    {
                    try
                    {
                        SqlConnection connect = new SqlConnection(dm.ConnectionString);
                        connect.Open();

                        SqlCommand cmd = new SqlCommand(query, connect);

                        //Parametized SQL
                        cmd.CreateParameter();
                        cmd.Parameters.AddWithValue("productID", productID);
                        cmd.Parameters.AddWithValue("currentstoreID", storeID);
                        cmd.Parameters.AddWithValue("quantity", quantity);

                        var affectedRow = dm.updateData(cmd);
                        connect.Close();

                        Console.WriteLine("Stock Request Created.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception: {0}", e.Message);
                    }
            //    }
            //}
        }


        //add new inventory item



    }
}
