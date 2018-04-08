using Assignment1.Models;
using System;

namespace Assignment1
{
    public static class Menu
    {
        //Main Menu
        public static void DisplayMainMenu()
        {
            while (true)
            {
                Console.WriteLine("Welcome to Marvelous Magic");
                Console.WriteLine("==========================");
                Console.WriteLine("1. Owner");
                Console.WriteLine("2. Franchise Holder");
                Console.WriteLine("3. Customer");
                Console.WriteLine("4. Quit");
                Console.WriteLine("Enter an option:");

                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        DisplayOwnerMenu();
                        break;
                    case "2":
                        DisplayFranchiseMenu();
                        break;
                    case "3":
                        DisplayCustomerMenu();
                        break;
                    case "4":
                        Console.WriteLine("See ya.");
                        return;
                    default:
                        Console.WriteLine("Invalid Choice");
                        break;
                }
            }

        }

        //1. Owner Menu
        public static void DisplayOwnerMenu()
        {
            var o = new Owner();
            

            while (true)
            {
                Console.WriteLine("Welcome to Marvelous Magic (Owner)");
                Console.WriteLine("=================================");
                Console.WriteLine("1. Display All Stock Requests");
                Console.WriteLine("2. Display Owner Inventory");
                Console.WriteLine("3. Reset Inventory Item Stock");
                Console.WriteLine("4. Return to Main Menu");
                Console.WriteLine("Enter an option:");

                var input = Console.ReadLine();

                //check if the input valid
                //if (!int.TryParse(input, out var option) || option < 1 || option > 3)
                //{
                //    Console.WriteLine("Invalid input.");
                //    Console.WriteLine();
                //    continue;
                //}

                switch (input)
                {
                    case "1":
                        o.GetOwnerInventory(Int32.Parse(input));
                        o.GetStockRequestTable();
                        break;
                    case "2":
                        o.GetOwnerInventory(Int32.Parse(input));
                        break;
                    case "3":
                        Console.WriteLine("\nReset Stock \nProduct stock will be reset to 20.");
                        o.GetOwnerInventory(Int32.Parse(input));
                        Console.WriteLine("\nEnter product ID to reset.");
                        var reset = Int32.Parse(Console.ReadLine());
                        o.ResetTo20(reset);
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid Choice");
                        break;
                }
            }
        }

        //2. Franchisee Menu
        //Retrieves Store List for user to locate, takes in user input
        //3 Franchise Holder Options: Display Inventory, Stock Request (Threshold), Add New Inventory Item
        public static void DisplayFranchiseMenu()
        {
            var f = new FranchiseHolder();
            var s = new Store();
            const int newItemStock = 1;

            s.GetStoreList();

            Console.WriteLine("Enter your store to use: ");
            var storeID = Int32.Parse(Console.ReadLine());

            Store.StoreFranchise storeLocation = (Store.StoreFranchise)storeID;

            while (true)
            {
                Console.WriteLine($"\nWelcome to Marvelous Magic (Retail - {storeLocation})");
                Console.WriteLine("==========================");
                Console.WriteLine("1. Display Inventory");
                Console.WriteLine("2. Stock Request (Threshold)");
                Console.WriteLine("3. Add New Inventory Item");
                Console.WriteLine("4. Return to Main Menu\n");
                Console.WriteLine("Enter an option:\n");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        f.CheckStoreInventory(storeID);
                        break;
                    case "2":
                        //OPTION 2 - STEP 1: View Stock Request Threshold - requires user input for threshold amount
                        Console.WriteLine("\nEnter threshold for re-stocking: ");
                        var v = Int32.Parse(Console.ReadLine());
                        f.GetStockThreshold(v, storeID);
                        break;
                    case "3":
                        f.CheckOwnerItem(storeID);
                        Console.WriteLine("\nEnter product to add: ");
                        var id2 = Int32.Parse(Console.ReadLine());
                        f.AddStockRequest(id2, newItemStock, storeID);

                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid Choice \n");
                        break;
                }
            }
        }

        //3. Customer Menu
        //Retrieves Store List for user to locate, takes in user input
        //1 Customer Option: Display Products to Purchase
        public static void DisplayCustomerMenu()
        {
            var c = new Customer();
            var s = new Store();

            s.GetStoreList();

            Console.WriteLine("Enter Store ID to shop at: ");
            var storeID = Int32.Parse(Console.ReadLine());

            Store.StoreFranchise storeLocation = (Store.StoreFranchise)storeID;

            while (true)
            {
                Console.WriteLine($"Welcome to Marvelous Magic (Retail - {storeLocation})");
                Console.WriteLine("==========================");
                Console.WriteLine("1. Display Products");
                Console.WriteLine("2. Return to Main Menu \n");
                Console.WriteLine("Enter an option:\n");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        c.DisplayProducts(storeID);
                        break;
                    case "2":
                        return;
                    default:
                        Console.WriteLine("Invalid Choice \n");
                        break;
                }
            }
        }

    }
}