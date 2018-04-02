using Assignment1.Controller;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Assignment1.Models
{
    public class StockRequest
    {
        internal int requestID;
        internal int requestQuantity;
        internal int storeID;
        internal int prodID;

        public StockRequest(int rID, int sID, int pID, int rQuantity)
        {
            requestID = rID;
            storeID = sID;
            prodID = pID;
            requestQuantity = rQuantity;
        }

        internal IEnumerable<Inventory> StoreInventory { get; }

        private DataManager dm = DataManager.GetDataManager();
        public List<StockRequest> requestItems = new List<StockRequest>();


        public void addStockRequest(int productID, int threshold)
        {
            //add to stockrequest table
            string query = "INSERT INTO StockRequest (StoreID, ProductID, Quantity) Values(@currentStoreID, @productID, @threshold);";

            foreach (Inventory item in StoreInventory)
            {
                //check if inserted id is same as productID in that store
                if (productID == item.ProductID)
                {
                    try
                    {
                        SqlConnection connect = new SqlConnection(dm.ConnectionString);
                        connect.Open();

                        SqlCommand cmd = new SqlCommand(query, connect);

                        //Parametized SQL
                        cmd.CreateParameter();
                        cmd.Parameters.AddWithValue("productID", productID);
                        cmd.Parameters.AddWithValue("storeID", storeID);
                        cmd.Parameters.AddWithValue("quantity", threshold);

                        var affectedRow = dm.updateData(cmd);
                        connect.Close();

                        Console.WriteLine("Stock Request Created.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception: {0}", e.Message);
                    }
                    }
                }
            }

        public void getStockRequestTable()
        {
            string query = "SELECT * FROM StockRequest;";

            try
            { 
                var requestTable = dm.fetchData(query, dm.ConnectionString);

                foreach (DataRow row in requestTable.Rows)
                {
                    var rID = row["StockRequesetID"].ToString();
                    var sID = row["StoreID"].ToString();
                    var pID = row["ProductID"].ToString();
                    var pQuantity = row["Quantity"].ToString();

                    StockRequest item = new StockRequest(Int32.Parse(rID), Int32.Parse(sID), Int32.Parse(pID), Int32.Parse(pQuantity));

                    requestItems.Add(item);

                    Console.WriteLine("{0} {1} {2}\n",
                                  row["ProductID"],
                                  row["Name"], row["StockLevel"]);
                }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: {0}", e.Message);
                }
            }
        }
    
}

