using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Assignment1.Controller;
using System.Linq;


namespace Assignment1.Models
{
    public class FranchiseHolder
    {
        private static DataManager dm = DataManager.GetDataManager();

        //Available items in the store
        internal static List<Inventory> StoreItems = new List<Inventory>();

        //Additional new items that store could add to their inventory
        internal List<Inventory> optionalItems = new List<Inventory>();

        //Items that are under user input threshold
        internal List<Inventory> thresholdItems = new List<Inventory>();

        public FranchiseHolder() {}

        //OPTION 1: Check store inventory
        //Retrieve from DB of that store's inventory
        public void CheckStoreInventory(int storeID)
        {
            string query = "select Product.ProductID, Product.Name,StoreInventory.StockLevel from Product JOIN StoreInventory ON Product.ProductID = StoreInventory.ProductID where StoreInventory.StoreID = @storeID;";
            const string format = "{0,-5}{1,-25}{2}";

            try
            {
                SqlConnection conn = new SqlConnection(dm.ConnectionString);
                conn.Open();

                SqlCommand commd = new SqlCommand(query, conn);
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@storeID";
                param.SqlDbType = SqlDbType.Int;
                param.Direction = ParameterDirection.Input;
                param.Value = storeID;

                commd.Parameters.Add(param);

                var FranchiseHolderTable = dm.GetTable(commd);

                Console.WriteLine(format, "ID", "Product", "Current Stock");

                foreach (DataRow row in FranchiseHolderTable.Rows)
                {
                    var pID = row["ProductID"].ToString();
                    var pName = row["Name"].ToString();
                    var pStock = row["StockLevel"].ToString();

                    Inventory item = new Inventory(Int32.Parse(pID), pName, Int32.Parse(pStock));

                    StoreItems.Add(item);

                    Console.WriteLine("{0,-5} {1,-25} {2,-5}\n",
                                  row["ProductID"],
                                  row["Name"], row["StockLevel"]);
                }
                    conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
        }


        //OPTION 2 - STEP 2: View Stock Request Threshold
        //Retrieve from DB on the items with user input threshold
        //Then shows user whether there are items under that threshold, if not - returns to menu, otherwise process user input for item to send stock request
        public void GetStockThreshold(int threshold, int storeID)
        {
            string query = "select Product.ProductID, Product.Name, StoreInventory.StockLevel from Product JOIN StoreInventory ON Product.ProductID = StoreInventory.ProductID where StoreInventory.StoreID = @currentStoreId AND StockLevel <= @threshold;";

            const string format = "{0,-5}{1,-25}{2}";

            try
            {
                SqlConnection conn = new SqlConnection(dm.ConnectionString);
                conn.Open();

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
                param2.Value = storeID;

                commd.Parameters.Add(param);
                commd.Parameters.Add(param2);

                var ThresholdTable = dm.GetTable(commd);
                Console.WriteLine(format, "ID", "Product", "Current Stock");

                foreach (DataRow row in ThresholdTable.Rows)
                {
                    var pID = row["ProductID"].ToString();
                    var pName = row["Name"].ToString();
                    var pStock = row["StockLevel"].ToString();

                    Inventory item = new Inventory(Int32.Parse(pID), pName, Int32.Parse(pStock));

                    thresholdItems.Add(item);

                    Console.WriteLine("{0,-5} {1,-25} {2,-5}\n",
                                  row["ProductID"],
                                  row["Name"], row["StockLevel"]);
                }
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
            finally
            {
                if (!thresholdItems.Any())
                {
                    Console.WriteLine("No items under that threshold.");
                }
                else
                {
                    Console.WriteLine("\n Enter request to process: ");
                    var input = Int32.Parse(Console.ReadLine());
                    CheckAvailability(input, threshold, storeID);
                }
            }

        }

        //OPTION 2 - Step 3: Check user input for the correct Product ID
        public void CheckAvailability(int productID, int threshold, int storeID)
        {
            if (StoreItems.Exists(x => x.ProductID == productID))
            {
                AddStockRequest(productID, threshold, storeID);
            }
            else if (!StoreItems.Exists(x => x.ProductID == productID))
            {
                Console.WriteLine("\nSorry, the selected product does not match your stock.\n\n");
                thresholdItems.Clear();
                return;
            }
        }

        //OPTION 2 - Step 4: FranchiseHolder sends stock request for a product
        //OPTION 3 - Step 3: Franchise Holder sends stock request for new product
        //Adds new row to StockRequest table for owner to view and process later
        public void AddStockRequest(int productID, int quantity, int storeID)
        {
            string query = "INSERT INTO StockRequest (StoreID, ProductID, Quantity) Values(@currentStoreID, @productID, @quantity);";

            try
            {
                SqlConnection connect = new SqlConnection(dm.ConnectionString);
                connect.Open();

                SqlCommand cmd = new SqlCommand(query, connect);
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
        }

        //OPTION 3 - Step 1: Add new Product Item to Inventory
        //Checks whether Request is processed.
        public static void UpdateStoreInventory(StockRequest newRequest)
        {
            if (newRequest.process == true)
            {
                if (StoreItems.Exists(x => x.ProductID == newRequest.prodID))
                {
                    UpdateStockLevel(newRequest);
                }
                else if (!StoreItems.Exists(x => x.ProductID == newRequest.prodID))
                {
                    AddNewItem(newRequest);
                }
            }
        }

        //OPTION 3 - Step 2: Add New Inventory Item
        //after confirming the request, insert new inventory item to the store's inventory
        public static void AddNewItem(StockRequest newRequest)
        {
            string query1 = "INSERT INTO StoreInventory (StoreID, ProductID, StockLevel) Values(@storeID, @productID, @stockLevel);";

            try
            {
                SqlConnection conn = new SqlConnection(dm.ConnectionString);
                conn.Open();

                SqlCommand cmmd1 = new SqlCommand(query1, conn);
                cmmd1.CreateParameter();
                cmmd1.Parameters.AddWithValue("storeID", newRequest.storeID);
                cmmd1.Parameters.AddWithValue("productID", newRequest.prodID);
                cmmd1.Parameters.AddWithValue("stockLevel", 1);

                var affectedRow = dm.updateData(cmmd1);

                Console.WriteLine("\nSuccessfully add new item. {0} row has been inserted", affectedRow);

                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
        }

        //OPTION 3 - Step 1: Add New Inventory
        //Compares FranchiseHolder Inventory to Owner Inventory
        public void CheckOwnerItem(int storeID)
        {
            string selectQuery = "select OwnerInventory.ProductID, Product.Name,OwnerInventory.StockLevel from OwnerInventory LEFT JOIN Product ON OwnerInventory.ProductID = Product.ProductID where OwnerInventory.ProductID NOT IN (select ProductID from StoreInventory where StoreID = @storeID);";
            const string format = "{0,-5}{1,-25}{2}";

            try
            {
                SqlConnection conn = new SqlConnection(dm.ConnectionString);
                conn.Open();

                SqlCommand commd = new SqlCommand(selectQuery, conn);
                commd.CreateParameter();
                commd.Parameters.AddWithValue("storeID", storeID);

                var optionalItemTable = dm.GetTable(commd);

                foreach (DataRow row in optionalItemTable.Rows)
                {
                    var pID = row["ProductID"].ToString();
                    var pName = row["Name"].ToString();
                    var pStock = row["StockLevel"].ToString();

                    Inventory item = new Inventory(Int32.Parse(pID), pName, Int32.Parse(pStock));

                    optionalItems.Add(item);

                    if (optionalItems.Count >= 1)
                    {
                        Console.WriteLine(format, row["ProductID"], row["Name"], row["StockLevel"]);
                    } else
                    {
                        Console.WriteLine("No new products available.");
                    }
                }
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
            finally
            {
                // shows user whether there are new products to add to store
                if (!optionalItems.Any())
                {
                    Console.WriteLine("No new products available.");
                }
                else
                {
                    Console.WriteLine("\n Enter product to add: ");
                    var input = Int32.Parse(Console.ReadLine());
                }
            }
        }

        //When Owner processes stock request - Helper method to AddStock
        private static int AddStock(int x, int y) => x + y;

        //When Owner processes stock requests - Update Stock Level from Stock Request
        public static void UpdateStockLevel(StockRequest newRequest)
        {
            var currentStock = 0;

            foreach (Inventory item in StoreItems)
            {
                if (item.ProductID == newRequest.prodID)
                {
                    currentStock = item.StockLevel;
                }
            }

            var updateAmount = AddStock(newRequest.requestQuantity, currentStock);

            string query2 = "update StoreInventory set StockLevel = @updateStock where StoreID = @storeID AND ProductID = @productID;";

            try
            {
                SqlConnection conn = new SqlConnection(dm.ConnectionString);
                conn.Open();

                SqlCommand cmmd2 = new SqlCommand(query2, conn);

                cmmd2.CreateParameter();
                cmmd2.Parameters.AddWithValue("storeID", newRequest.storeID);
                cmmd2.Parameters.AddWithValue("productID", newRequest.prodID);
                cmmd2.Parameters.AddWithValue("updateStock", updateAmount);

                var affectedRow = dm.updateData(cmmd2);
                Console.WriteLine("\nSuccessfully update. {0} row has been changed", affectedRow);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }

        }
    }
}