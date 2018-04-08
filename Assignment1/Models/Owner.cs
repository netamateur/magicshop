using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Assignment1.Controller;
using static Assignment1.Store;

namespace Assignment1.Models
{
    public class Owner
    {
        //Tier1 System user
        private static DataManager dm = DataManager.GetDataManager();

        private static List<Inventory> OwnerItems = new List<Inventory>();
        public static List<OwnerRequest> DisplayedRequest = new List<OwnerRequest>();

        public Owner() { }

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

        //OPTION 1 - Step 1: Retrieve Owner Inventory, table not printed
        //OPTION 2: Display owner's inventory, table printed
        //OPTION 3 - Step 1:
        //Retrieve from DB of Owner Inventory
        public void GetOwnerInventory(int i)
        {
            const string format = "{0,-5}{1,-25}{2}";

            string query = "select product.ProductID, product.Name, OwnerInventory.StockLevel from product JOIN OwnerInventory ON Product.ProductID = OwnerInventory.ProductID;";

            try
            {
                var ownerTable = dm.FetchData(query, dm.ConnectionString);

                if (i != 1)
                {
                    Console.WriteLine(format, "ID", "Product", "Current Stock");
                }
                foreach (DataRow row in ownerTable.Rows)
                {
                    var pID = row["ProductID"].ToString();
                    var pName = row["Name"].ToString();
                    var pStock = row["StockLevel"].ToString();

                    Inventory item = new Inventory(Int32.Parse(pID), pName, Int32.Parse(pStock));

                    OwnerItems.Add(item);

                    if (i != 1)
                    {
                        Console.WriteLine(format,
                                  row["ProductID"],
                                  row["Name"], row["StockLevel"]);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }

        }

        //OPTION 1 - Step 2: Retrieve Stock Request Table
        //Fetch all the stock request from db, and add them to the stockRequest list
        public List<StockRequest> GetStockRequestTable()
        {
            string query = "SELECT * FROM StockRequest;";

            try
            {
                var requestTable = dm.FetchData(query, dm.ConnectionString);

                foreach (DataRow row in requestTable.Rows)
                {
                    var rID = row["StockRequestID"].ToString(); //throw exception: Column 'StockRequesetID' does not belong to table Table.
                    var sID = row["StoreID"].ToString();
                    var pID = row["ProductID"].ToString();
                    var pQuantity = row["Quantity"].ToString();

                    StockRequest item = new StockRequest(Int32.Parse(rID), Int32.Parse(sID), Int32.Parse(pID), Int32.Parse(pQuantity));

                    StockRequest.requestItems.Add(item);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
            finally
            {
                DisplayOwnerRequest();
            }
                return StockRequest.requestItems;
        }

        //OPTION 1 - Step 3: Display all owner's received requests(StockRequest obj + currentStock + Availablity)
        public void DisplayOwnerRequest()
        {
            Console.WriteLine("{0,-3} {1,-15} {2,-25} {3,-10} {4,-10} {5,10}\n",
                                          "ID", "Store", "Product", "Quantity", "Current Stock", "Availability");

            //for each item in OwnerInventory, loop the StockRequest list
            foreach (Inventory item in OwnerItems)
            {
                foreach (StockRequest request in StockRequest.requestItems/*getStockRequestTable()*/)
                {
                    //compare the productID of 2 lists, assign the productName & currentStock 
                    if (item.ProductID == request.prodID)
                    {
                        var currentStock = item.StockLevel;
                        var productName = item.ProductName;
                        bool availbility = CompareStock(currentStock, request.requestQuantity);

                        DisplayedRequest.Add(new OwnerRequest(request, productName, currentStock, availbility));

                        StoreFranchise storeList = (StoreFranchise)request.storeID;

                        Console.WriteLine("{0,-3} {1,-15} {2,-25} {3,-10} {4,-10} {5,10}\n",
                                          request.requestID,
                                          storeList,
                                          productName,
                                          request.requestQuantity,
                                          currentStock,
                                          availbility);
                    }
                }
            }

            Console.WriteLine("Enter request to process");
            var input = Console.ReadLine();
            ProcessRequest(Int32.Parse(input));
        }

        //Helper Lambda expression used to get the Availability of request
        private static bool CompareStock(int x, int y) => x >= y;

        //Helper Lambda expression used to remove stock
        private static int ReduceStock(int x, int y) => x - y;


        //OPTION 1 - Step 4: Process Stock Request
        //1.check if the availability of request is True
        //2.deal with the request-> update owner's inventory & update store's inventory in db -> set the request.process = true 
        public static void ProcessRequest(int requestID)
        {
            string query = "update OwnerInventory set StockLevel = @updateStock where ProductID = @productID;";

            foreach (OwnerRequest item in DisplayedRequest)
            {
                if (item.request.requestID == requestID)
                {
                    if (item.availability == true)
                    {
                        var updateStockLevel = ReduceStock(item.currentOwnerStock, item.request.requestQuantity);

                        try
                        {
                            SqlConnection conn = new SqlConnection(dm.ConnectionString);
                            conn.Open();

                            SqlCommand commd = new SqlCommand(query, conn);
                            commd.CreateParameter();
                            commd.Parameters.AddWithValue("productID", item.request.prodID);
                            commd.Parameters.AddWithValue("updateStock", updateStockLevel);

                            var affectedRow = dm.UpdateData(commd);

                            conn.Close();

                            Console.WriteLine("\nThe number of rows have been updated: " + affectedRow);

                            item.request.process = true;

                            FranchiseHolder.UpdateStoreInventory(item.request);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Exception: {0}", e.Message);
                        }
                        finally
                        {
                            deleteRequest(requestID);
                        }
                    }
                }
            }
        }

        //OPTION 1 - Step 5: After stock request has been processed, the row will be removed from the table
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

                var affectedRow = dm.UpdateData(command);
                conn.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
            finally
            {
                OwnerItems.Clear();
            }
        }

        //OPTION 3: Reset Inventory Item Stock
        //reset item stock to 20 given productID
        public void ResetTo20(int productID)
        {
            string query = "update OwnerInventory set StockLevel = 20 where ProductID = @productID;";

            foreach (Inventory item in OwnerItems)
            {
                if (productID == item.ProductID)
                {
                    if (item.StockLevel < 20)
                    {
                        try
                        {
                            SqlConnection conn = new SqlConnection(dm.ConnectionString);
                            conn.Open();

                            SqlCommand commd = new SqlCommand(query, conn);
                            commd.CreateParameter();
                            commd.Parameters.AddWithValue("productID", productID);

                            var affectedRow = dm.UpdateData(commd);
                            conn.Close();

                            Console.WriteLine("\nThe number of rows have been updated:" + affectedRow);
                            Console.WriteLine("\n{0} stock level has been reset to 20.", item.ProductName);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Exception: {0}", e.Message);
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nThe stock level of selected product is already 20 or above.\n");
                        OwnerItems.Clear();
                    }
                }
            }
        }
    }
}