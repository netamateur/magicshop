using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Assignment1.Controller;
using Assignment1.Models;

namespace Assignment1
{
    public class Owner
    {
        //Tier1 System user
        private static DataManager dm = DataManager.GetDataManager();
        private static List<Inventory> OwnerItems = new List<Inventory>();

        //Display owner's inventory
        public static void checkOwnerInventory()
        {
            string query =
            "select product.ProductID, product.Name, OwnerInventory.StockLevel from product JOIN OwnerInventory ON Product.ProductID = OwnerInventory.ProductID;";

            try
            {
                var ownerTable = dm.fetchData(query, dm.ConnectionString);

                foreach (DataRow row in ownerTable.Rows)
                {
                    var pID = row["ProductID"].ToString();
                    var pName = row["Name"].ToString();
                    var pStock = row["StockLevel"].ToString();

                    Inventory item = new Inventory(Int32.Parse(pID), pName, Int32.Parse(pStock));

                    OwnerItems.Add(item);

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


        //Display all the stock request
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

                    item.requestItems.Add(item);

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
        /*
        public static List<StoreRequest> GetRequests()
        {
        }*/

        //reset item stock to 20 by taking productID
        public static void resetTo20(int productID)
        {
            string query = "update OwnerInventory set StockLevel = 20 where ProductID = @productID;";


            //match the input id by looping the list
            foreach (Inventory item in OwnerItems)
            {

                //check if the product ID is same
                if (productID == item.ProductID)
                {
                    //if true, check if stock level less than 20
                    if (item.StockLevel < 20)
                    {
                        try
                        {
                            SqlConnection conn = new SqlConnection(dm.ConnectionString);
                            conn.Open();

                            SqlCommand commd = new SqlCommand(query, conn);

                            //Parameterized SQL
                            commd.CreateParameter();
                            commd.Parameters.AddWithValue("productID", productID);

                            //update the stocklevel
                            var affectedRow = dm.updateData(commd);

                            conn.Close();

                            Console.WriteLine("The number of rows have been updated:" + affectedRow);
                            Console.WriteLine("{0} stock level has been reset to 20.", item.ProductName);

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Exception: {0}", e.Message);
                        }

                    }
                    else
                    {
                        Console.WriteLine("The stock level of selected product is already 20 or above.");
                    }//end of nested filter


                }//end of filter


            }//end of foreach loop




        }


        //process request and then delete request after processing
        //check availablity
        public static void process()
        {

        }



    }
}