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




        //add new stock request to owner





        //after confirming the request, insert new inventory item to the store's inventory
        public void addNewItem(int storeID, int productID)
        {
            //check if the input id match the one in optionalItem List

            //if true, execute the command to insert new row

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
