using Assignment1.Controller;
using Assignment1.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Assignment1
{
    class Customer
    {
        private DataManager dm = DataManager.GetDataManager();

        //Inventory Items avalable for the customer to purchase
        internal List<Inventory> ShopItems = new List<Inventory>();

        public Customer() { }

        //OPTION 1 - Step 1: Display products for customer to buy at a store
        //Retrieve from DB and added to ShopItems objects to a Inventory List - only when the list of items are displayed, the customer can make a purchase
        public void DisplayProducts(int storeID)
        {
            string query = "SELECT Product.ProductID, Product.Name,StoreInventory.StockLevel from Product JOIN StoreInventory ON Product.ProductID = StoreInventory.ProductID where StoreInventory.StoreID = @storeID;";
            SqlConnection conn = new SqlConnection(dm.ConnectionString);
            conn.Open();

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
                printPaginated(storeID);
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

        //OPTION 1 - Step 2: Display 3 Products per Page
        //Code Reference example from Matthew Bolger, Week 5
        //Takes in user input for viewing more products, return to menu or make a purchase
        public void printPaginated(int storeID)
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
                        checkAvailability(Int32.Parse(input), storeID);
                        return;
                    default:
                        Console.WriteLine("Invalid Input");
                        break;
                }
            }
        }
       
        //OPTION 1 - Step 3: Checks whether the product is available at that store
        public void checkAvailability(int productID, int storeID)
        {
            if (ShopItems.Exists(x => x.ProductID == productID))
            {
                purchaseItem(productID, storeID);
            }
            else if (!ShopItems.Exists(x => x.ProductID == productID))
            {
                Console.WriteLine("\nSorry, the selected product is not available at this store.\n\n");
                ShopItems.Clear();
                return;
            }
        }

        //OPTION 1 - Step 4: After validating selection, item can be purchase
        //First it selects the item, requires user for a purchase amount, checks available stock level at that store, and updates the the DB once product(s) is purchased.
        public void purchaseItem(int productID, int storeID)
        {
            var filtered = from item in ShopItems
                           where item.ProductID == productID
                           select item;

            Console.WriteLine("Enter quantity to purhcase: \n");
            var purchaseAmount = Int32.Parse(Console.ReadLine());

            foreach (var i in filtered)
            {
                if (purchaseAmount >= i.StockLevel)
                {
                    Console.WriteLine("\nSorry, we do not have enough stock.\n\n");
                    ShopItems.Clear();
                    break;
                }
                else
                { 
                    try
                    {
                        string query = "update StoreInventory set StockLevel = StockLevel - @purchaseAmount where ProductID = @productID AND StoreID = @storeID;";

                        SqlConnection conn = new SqlConnection(dm.ConnectionString);
                        conn.Open();

                        SqlCommand commd = new SqlCommand(query, conn);

                        commd.CreateParameter();
                        commd.Parameters.AddWithValue("productID", productID);
                        commd.Parameters.AddWithValue("purchaseAmount", purchaseAmount);
                        commd.Parameters.AddWithValue("storeID", storeID);

                        var affectedRow = dm.updateData(commd);

                        conn.Close();

                        ProductTypes prodName = (ProductTypes)productID;
                        Console.WriteLine($"Purchased {purchaseAmount} of {prodName}");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception: {0}", e.Message);
                    }
                    finally
                    {
                        Console.WriteLine("\n.............Thank you for shopping with us. \n\n");
                        ShopItems.Clear();
                    }
                }
            }
        }
   }
}
