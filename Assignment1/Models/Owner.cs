using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Assignment1.Controller;

namespace Assignment1.Models
{
    public class Owner
    {
        //Tier1 System user


        private static DataManager dm = DataManager.GetDataManager();
        private static List<Inventory> OwnerItems = new List<Inventory>();
        public static List<OwnerRequest> displayedRequest = new List<OwnerRequest>();


        //a struct type represents the received request that is displayed to Owner
        public struct OwnerRequest
        {
            public StockRequest request;
            public string productName;
            public int currentOwnerStock;
            public bool availability;

            public OwnerRequest(StockRequest myRequest, string name, int currentStk, bool stockAvail)
            {
                request = myRequest;
                productName = name;
                currentOwnerStock = currentStk;
                availability = stockAvail;
            }


        }





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


        //a Lambda expression used to get the Availability of request
        private static bool compareStock(int x, int y) => x >= y;



        //Display all owner's received requests(StockRequest obj + currentStock + Availablity)
        public static void displayOwnerRequest()
        {

            //for each item in OwnerInventory, loop the StockRequest list
            foreach(Inventory item in OwnerItems)
            {

                foreach (StockRequest request in StockRequest.requestItems/*getStockRequestTable()*/)
                {
                    //compare the productID of 2 lists, assign the productName & currentStock 
                    if(item.ProductID == request.prodID)
                    {
                        var currentStock = item.StockLevel;
                        var productName = item.ProductName;
                        bool availbility = compareStock(currentStock, request.requestQuantity);

                        displayedRequest.Add(new OwnerRequest(request, productName,currentStock, availbility));

                        Console.WriteLine("{0} {1} {2} {3} {4} {5}\n",
                                          request.requestID,
                                          request.storeID,
                                          productName,
                                          request.requestQuantity,
                                          currentStock,
                                          availbility);
                    }//end of filter 

                }//end of nested foreach loop

            }//end of outter loop
            //return displayedRequest;


        }






        //Fetch all the stock request from db, and add them to the stockRequest list
        //Notice:getStockRequestTable() must be called before displayOwnerRequest()
        public static List<StockRequest> getStockRequestTable()
        {
            string query = "SELECT * FROM StockRequest;";

            try
            {
                var requestTable = dm.fetchData(query, dm.ConnectionString);

                foreach (DataRow row in requestTable.Rows)
                {
                    var rID = row["StockRequestID"].ToString(); //throw exception: Column 'StockRequesetID' does not belong to table Table.
                    var sID = row["StoreID"].ToString();
                    var pID = row["ProductID"].ToString();
                    var pQuantity = row["Quantity"].ToString();

                    StockRequest item = new StockRequest(Int32.Parse(rID), Int32.Parse(sID), Int32.Parse(pID), Int32.Parse(pQuantity));

                    StockRequest.requestItems.Add(item);

                    /*
                    Console.WriteLine("{0} {1} {2} {3}\n",
                                  row["StockRequestID"],
                                  row["StoreID"],
                                  row["ProductID"],
                                  row["Quantity"]);*/
                }

                return StockRequest.requestItems;


            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
                return null;
            }



        }



        private static int reduceStock(int x, int y) => x - y;

        //process request and then delete request after processing
        //check availablity
        public static void processRequest(int requestID)
        {
            string query = "update OwnerInventory set StockLevel = @updateStock where ProductID = @productID;";

            foreach(OwnerRequest item in displayedRequest)
            {
                if(item.request.requestID == requestID)
                {
                    //1.check if the availability of request is True
                    if(item.availability == true)
                    {
                        //2.deal with the request-> update owner's inventory & 
                        //update store's inventory in db -> set the request.process = true 
                        var updateStockLevel = reduceStock(item.currentOwnerStock, item.request.requestQuantity);

                        try
                        {
                            SqlConnection conn = new SqlConnection(dm.ConnectionString);
                            conn.Open();

                            SqlCommand commd = new SqlCommand(query, conn);

                            commd.CreateParameter();
                            commd.Parameters.AddWithValue("productID", item.request.prodID);
                            commd.Parameters.AddWithValue("updateStock", updateStockLevel);

                            var affectedRow = dm.updateData(commd);

                            conn.Close();

                            Console.WriteLine("The number of rows have been updated:" + affectedRow);

                            //update the store's inventory
                            item.request.process = true;

                            //The this FranchiseHolder contains nothing inside
                            //var user = new FranchiseHolder();
                            FranchiseHolder.updateStoreInventory(item.request);

                            //remove the processed request
                            //displayedRequest.Remove(item);

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Exception: {0}", e.Message);
                        }

                    }
                    
                }

            }//end of foreach loop
            //3.delete the stockRequest in db - create a new method?
            deleteRequest(requestID);

        }

        //delete the stockRequest in db
        public static void deleteRequest(int requestID)
        {
            string query = "Delete from StockRequest Where StockRequestID = @requestID;";

            try
            {
                SqlConnection conn = new SqlConnection(dm.ConnectionString);
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("requestID", requestID);

                var affectedRow = dm.updateData(command);
                conn.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }

        }

        //reset item stock to 20 by taking productID
        public static void resetTo20(int productID)
        {
            string query = "update OwnerInventory set StockLevel = 20 where ProductID = @productID;";

          
            //match the input id by looping the list
            foreach(Inventory item in OwnerItems)
            {
                
                //check if the product ID is same
                if(productID == item.ProductID)
                {
                    //if true, check if stock level less than 20
                    if(item.StockLevel<20)
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

                    }else
                    {
                        Console.WriteLine("The stock level of selected product is already 20 or above.");

                    }//end of nested filter
                   

                }//end of filter


            }//end of foreach loop
                  
        }






    }
}
