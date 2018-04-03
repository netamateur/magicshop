using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Assignment1.Controller;


namespace Assignment1.Models
{
    public class StockRequest
    {
        internal string requestID;
        internal int requestQuantity;
        internal int storeID;
        internal int prodID;

        internal IEnumerable<Inventory> StoreInventory { get; }

        private DataManager dm = DataManager.GetDataManager();
        //public List<StockRequest> requestItems = new List<StockRequest>();

        public StockRequest()
        {
            //requestID = id;
            //store.StoreID = storeID;
            //prodID = productID;
            //requestQuantity = quantity;
        }

        public void addStockRequest(int productID)
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
                        cmd.Parameters.AddWithValue("storeID", currentStoreID);
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

    }
       

    }
}

