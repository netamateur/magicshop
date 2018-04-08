using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Assignment1
{
    public static class Menu
    {
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
                        //DisplayOwnerMenu();
                        break;
                    case "2":
                        //DisplayFranchiseMenu();
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

        //public static void DisplayOwnerMenu()
        //{
        //    while (true)
        //    {

        //        Console.WriteLine("Welcome to Marvelous Magic (Owner)");
        //        Console.WriteLine("==========================");
        //        Console.WriteLine("1. Display ALl Stock Requests");
        //        Console.WriteLine("2. Display Owner Inventory");
        //        Console.WriteLine("3. Reset Inventory Item Stock");
        //        Console.WriteLine("4. Return to Main Menu");
        //        Console.WriteLine("Enter an option:");

        //        var input = Console.ReadLine();

        //        check if the input valid 
        //        if (!int.TryParse(input, out var option) || option < 1 || option > 3)
        //        {
        //            Console.WriteLine("Invalid input.");
        //            Console.WriteLine();
        //            continue;
        //        }

        //        switch (input)
        //        {
        //            case "1":
        //                DisplayAllStock();
        //                break;
        //            case "2":
        //                DisplayOwnerInventory();
        //                break;
        //            case "3":
        //                ResetInventoryItemStock();
        //                break;
        //            case "4":
        //                return;
        //            default:
        //                Console.WriteLine("Invalid Choice");
        //                break;
        //        }
        //    }
        //}

        //private static void ResetInventoryItemStock()
        //{
        //    throw new NotImplementedException();
        //}

        //private static void DisplayOwnerInventory()
        //{
        //    throw new NotImplementedException();
        //}

        //private static void DisplayAllStock()
        //{
        //    throw new NotImplementedException();
        //}



        public static void DisplayCustomerMenu()
        {
            //1 display store list
            //2 pick store
            //3 disply menu of that store

            var c = new Customer();

            //1 -2
            c.getStoreList();


            Console.WriteLine("Enter Store ID to shop at: ");
            var storeID = Int32.Parse(Console.ReadLine());



            Store.StoreFranchise storeLocation = (Store.StoreFranchise)storeID;

            //3
            //input id and then taken to that store menu
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

        public static void DisplayFranchiseMenu()
        {
            //to do:
            //store ID and name
            //input id and then taken that franchise storemenu

        }

    }
}