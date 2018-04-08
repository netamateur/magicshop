using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Assignment1.Controller;


namespace Assignment1.Models
{
    public class FranchiseHolder
    {

        internal Store store { get; set; }
        private static DataManager dm = DataManager.GetDataManager();

        internal static List<Inventory> StoreItems = new List<Inventory>();
        internal List<Inventory> optionalItems = new List<Inventory>();

        public List<Inventory> thresholdItems = new List<Inventory>();

        public enum storeList { MelbourneCBD = 1, EastMelbourne, NorthMelbourne, SouthMelbourne, WestMelbourne }




        //Franchise Holder should take store obj as parameters to construct
        public FranchiseHolder()
        {
            //store.StoreID = storeID;

        }


        //check store inventory
        public void checkStoreInventory(int storeID)
        {
            string query = "SELECT Product.ProductID, Product.Name,StoreInventory.StockLevel from Product JOIN StoreInventory ON Product.ProductID = StoreInventory.ProductID where StoreInventory.StoreID = @storeID;";


            try
            {
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

                conn.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }


        }



        //View Stock Request Threshold
        public void getStockThreshold(int threshold, int currentStoreId)
        {
            /* double check the query: the "Stocklevel > threshold" is not correct? */
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

        //Add new stock request to owner or
        //Add Stock Request to Owner - Threshold --- update by Rita
        public void addStockRequest(int productID, int quantity, int storeID)
        {
            //add to stockrequest table
            string query = "INSERT INTO StockRequest (StoreID, ProductID, Quantity) Values(@currentStoreID, @productID, @quantity);";

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



        public static void updateStoreInventory(StockRequest newRequest)
        {
            if(newRequest.process == true)
            {
                if(StoreItems.Exists(x => x.ProductID == newRequest.prodID))
                {
                    updateStockLevel(newRequest);
                }
                else if(!StoreItems.Exists(x => x.ProductID == newRequest.prodID))
                {
                    addNewItem(newRequest);
                }


            }

        }


        /* !!!! */
        //Exception: Violation of PRIMARY KEY constraint 'PK_StoreInventory'. 
        //Cannot insert duplicate key in object 'dbo.StoreInventory'. The duplicate key value is (5, 2).

        //after confirming the request, insert new inventory item to the store's inventory
        public static void addNewItem(StockRequest newRequest)
        {
            string query1 = "INSERT INTO StoreInventory (StoreID, ProductID, StockLevel) Values(@storeID, @productID, @stockLevel);";

            try
            {
                SqlConnection conn = new SqlConnection(dm.ConnectionString);
                conn.Open();

                    SqlCommand cmmd1 = new SqlCommand(query1, conn);

                    //Parametized SQL
                    cmmd1.CreateParameter();
                    cmmd1.Parameters.AddWithValue("storeID", newRequest.storeID);
                    cmmd1.Parameters.AddWithValue("productID", newRequest.prodID);
                    cmmd1.Parameters.AddWithValue("stockLevel", 1);

                    var affectedRow = dm.updateData(cmmd1);

                    Console.WriteLine("Successfully add new item. {0} row has been inserted", affectedRow);
                /*
                foreach (Inventory item in optionalItems)
                {
                        //check if the item already in the storeInventory
                    if (newRequest.prodID == item.ProductID)
                    {
                            //after add the new item into the store inventory, delete it from the optionalList
                            optionalItems.Remove(item);
                     }
                    
                }*/

            //end of outter filter
                conn.Close();
            } 
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
           


        }

        private static int AddStock(int x, int y) => x + y;
        //The number of rows have been updated:1

        public static void updateStockLevel(StockRequest newRequest)
        {
            var currentStock = 0 ;
            //get the current stock ammount from StoreInventory
            foreach(Inventory item in StoreItems)
            {
                if(item.ProductID == newRequest.prodID)
                {
                    currentStock = item.StockLevel;
                }
                
            }

            var updateAmount = AddStock(newRequest.requestQuantity,currentStock);


            string query2 = "update StoreInventory set StockLevel = @updateStock where StoreID = @storeID AND ProductID = @productID;";

            try{

                SqlConnection conn = new SqlConnection(dm.ConnectionString);
                conn.Open();

                    SqlCommand cmmd2 = new SqlCommand(query2, conn);

                    cmmd2.CreateParameter();
                    cmmd2.Parameters.AddWithValue("storeID", newRequest.storeID);
                    cmmd2.Parameters.AddWithValue("productID", newRequest.prodID);
                    cmmd2.Parameters.AddWithValue("updateStock", updateAmount);


                    var affectedRow = dm.updateData(cmmd2);
                    Console.WriteLine("Successfully update. {0} row has been changed", affectedRow);



            }catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }

        }


       



        //fetch table from db by taking storeID
        //list all the items not in the store but in owner's inventory
        //The SQL has been revised!

        public void checkOwnerItem(int storeID)
        {
            
            string selectQuery = "select OwnerInventory.ProductID, Product.Name,OwnerInventory.StockLevel from OwnerInventory LEFT JOIN Product ON OwnerInventory.ProductID = Product.ProductID where OwnerInventory.ProductID NOT IN (select ProductID from StoreInventory where StoreID = @storeID);";
            try{

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

                    Console.WriteLine("{0} {1} {2}\n",
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


    }
}
