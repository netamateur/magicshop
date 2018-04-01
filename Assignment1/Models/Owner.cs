using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Assignment1.Contorller;

namespace Assignment1
{
    public class Owner
    {
        //Tier1 System user
        private static DataManager dm = DataManager.GetDataManager();
        private static List<Inventory> OwnerItems = new List<Inventory>(); 

        //get data from Inventory table

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

        /*
        public static List<StoreRequest> GetRequests()
        {


        }*/

        //reset item stock to 20 
        public static void resetTo20()
        {
           

        }

        //process request and then delete request after processing
        //check availablity
        public static void process()
        {
            
        }



    }
}
