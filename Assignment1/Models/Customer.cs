using Assignment1.Controller;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Assignment1
{
    class Customer
    {
        internal Store store;
        private DataManager dm = DataManager.GetDataManager();
        internal List<Inventory> ShopItems = new List<Inventory>();

        //display 3 items at a item
        public void DisplayProducts(int storeID)
        {
            string query = "SELECT Product.ProductID, Product.Name,StoreInventory.StockLevel from Product JOIN StoreInventory ON Product.ProductID = StoreInventory.ProductID where StoreInventory.StoreID = @storeID;";
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

            try
            {
                var FranchiseHolderTable = dm.GetTable(commd);

                foreach (DataRow row in FranchiseHolderTable.Rows)
                {
                    var pID = row["ProductID"].ToString();
                    var pName = row["Name"].ToString();
                    var pStock = row["StockLevel"].ToString();

                    Inventory item = new Inventory(Int32.Parse(pID), pName, Int32.Parse(pStock));

                    ShopItems.Add(item);

                }
                printPaginated();
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


        public void printPaginated()
        {
            var pageOffset = 0;
            var pageSize = 3;

            while (true)
            {
                Console.WriteLine("{0,-5}{1,-25}{2,-5}", "ID", "Product", "Current Stock");

                foreach (var x in ShopItems.Skip(pageOffset).Take(pageSize).ToList())
                {
                    Console.WriteLine("{0,-5}{1,-25}{2,-5}", x.ProductID, x.ProductName, x.StockLevel);
                }                
                Console.Write("\n [Legend: 'N' for Next Page | 'R' Return to Menu ]" +
                    "\n Enter product ID to purchase or function: \n\n");
                var input = Console.ReadLine();

                switch (input.ToUpper())
                {
                    case "N":
                        pageOffset += pageSize;
                        if (pageOffset >= ShopItems.Count)
                        {
                            pageOffset = 0;
                        }
                        break;
                    case "R":
                        //return to menu();
                        Console.WriteLine("return to menu");
                        return;
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                    case "10":
                        //buy item
                        purchaseItem(Int32.Parse(input));
                        Console.WriteLine("buy item");
                        return;
                    default:
                        Console.WriteLine("Invalid Input");
                        break;
                }
            }
        }

        public void purchaseItem(int input)
        {

        }
    }
}
