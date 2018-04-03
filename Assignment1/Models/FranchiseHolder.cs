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
        private DataManager dm = DataManager.GetDataManager();

        private List<Inventory> StoreItems = new List<Inventory>();
        private List<Inventory> optionalItems = new List<Inventory>();

        public List<Inventory> thresholdItems = new List<Inventory>();

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


            try{
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
            string query = "select Product.ProductID, Product.Name, StoreInventory.StockLevel from Product JOIN StoreInventory ON Product.ProductID = StoreInventory.ProductID where StoreInventory.StoreID = @currentStoreId AND StockLevel > @threshold;";
            SqlConnection conn = new SqlConnection(dm.ConnectionString);
            conn.Open();

            //parameterized Sql
            SqlCommand commd = new SqlCommand(query, conn);

            //commd.Parameters.Add("@currentStoreId", SqlDbType.Int);
            //commd.Parameters.Add("@threshold", SqlDbType.Int);

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
        //Add Stock Request to Owner - Threshold
        public void addStockRequest(int productID, int quantity)
        {
            //add to stockrequest table
            string query = "INSERT INTO StockRequest (StoreID, ProductID, Quantity) Values(@currentStoreID, @productID, @threshold);";

            foreach (Inventory item in StoreItems)
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
                        cmd.Parameters.AddWithValue("storeID", store.StoreID);
                        cmd.Parameters.AddWithValue("quantity", quantity);

                        var affectedRow = dm.updateData(cmd);
                        connect.Close();

                        Console.WriteLine("Stock Request Created. {0} row has been inserted", affectedRow);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception: {0}", e.Message);
                    }
                }
            }
        }


        //after confirming the request, insert new inventory item to the store's inventory
        public void addNewItem(StockRequest newRequest)
        {
            string query = "INSERT INTO StoreInventory (StoreID, ProductID, StockLevel) Values(@storeID, @productID, @stockLevel);";

            //check if the stockrequest has been processed or not && it's a new-item request
            if(newRequest.process == true && newRequest.requestQuantity == 1)
            {

                //check if the input id match the one in optionalItem List
                foreach (Inventory item in optionalItems)
                {
                    if (newRequest.prodID == item.ProductID)
                    {
                        try{
                            //if true, execute the command to insert new row
                        SqlConnection conn = new SqlConnection(dm.ConnectionString);
                        conn.Open();

                        SqlCommand cmmd = new SqlCommand(query, conn);

                        //Parametized SQL
                        cmmd.CreateParameter();
                        cmmd.Parameters.AddWithValue("storeID", newRequest.storeID);
                        cmmd.Parameters.AddWithValue("productID", newRequest.prodID);
                        cmmd.Parameters.AddWithValue("stockLevel", newRequest.requestQuantity);

                        var affectedRow = dm.updateData(cmmd);
                        conn.Close();

                        Console.WriteLine("Successfully add new item. {0} row has been inserted", affectedRow);
                        //after add the new item into the store inventory, delete it from the optionalList

                        }
                        catch(Exception e)
                        {
                            Console.WriteLine("Exception: {0}", e.Message);
                        }
                       
                    }
                    else
                    {

                        Console.WriteLine("The item is already in the store inventory.");
                    }
                }

            }

        }





        //fetch table from db by taking storeID
        //list all the items not in the store but in owner's inventory

        public void checkOwnerItem(int storeID)
        {
            
            string selectQuery = "select * from OwnerInventory EXCEPT select OwnerInventory.ProductID, OwnerInventory.StockLevel from OwnerInventory JOIN StoreInventory ON OwnerInventory.ProductID = StoreInventory.ProductID where StoreInventory.StoreID = @storeID;";
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
