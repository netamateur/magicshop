using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Assignment1
{
    public class Menu
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
                        DisplayOwnerMenu();
                        break;
                    case "2":
                        DisplayFranchiseMenu();
                        break;
                    case "3":
                        DisplayCustomerMenu();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid Choice");
                        break;
                }
            }

        }

        public static void DisplayOwnerMenu()
        {
            while (true)
            {
                
                Console.WriteLine("Welcome to Marvelous Magic (Owner)");
                Console.WriteLine("==========================");
                Console.WriteLine("1. Display ALl Stock Requests");
                Console.WriteLine("2. Display Owner Inventory");
                Console.WriteLine("3. Reset Inventory Item Stock");
                Console.WriteLine("4. Return to Main Menu");
                Console.WriteLine("Enter an option:");
        
                var input = Console.ReadLine();

                //check if the input valid 
                if (!int.TryParse(input, out var option) || option < 1 || option >3 )
                {
                    Console.WriteLine("Invalid input.");
                    Console.WriteLine();
                    continue;
                }

                switch (input)
                {
                    case "1":
                        DisplayAllStock();
                        break;
                    case "2":
                        DisplayOwnerInventory();
                        break;
                    case "3":
                        ResetInventoryItemStock();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid Choice");
                        break;
                }
            }
        }

        private static void ResetInventoryItemStock()
        {
            throw new NotImplementedException();
        }

        private static void DisplayOwnerInventory()
        {
            throw new NotImplementedException();
        }

        private static void DisplayAllStock()
        {
            throw new NotImplementedException();
        }

        public static void DisplayCustomerMenu()
        {
            //to do:
            //display store id and name
            //input id and then taken to that store menu
            while (true)
            {
                Console.WriteLine("Welcome to Marvelous Magic (Retail - " { storename});
                Console.WriteLine("==========================");
                Console.WriteLine("1. Display ALl Stock Requests");
                Console.WriteLine("2. Return to Main Menu");
                Console.WriteLine("Enter an option:");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        DisplayProducts();
                        break;
                    case "2":
                        return;
                    default:
                        Console.WriteLine("Invalid Choice");
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

        public List<Inventory> Items { get; set; }




        public static DataTable GetDataTable(this SqlCommand command)
        {
            var table = new DataTable();
            new SqlDataAdapter(command).Fill(table);

            return table;
        }


        public void connectDB()
        {
            using (var connection = new SqlConnection("server=wdt2018.australiaeast.cloudapp.azure.com;uid=s3419529;database=s3419529;pwd=abc123"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "select * from "; //Inventory of specific store

                var table = new DataTable();
                var adapter = new SqlDataAdapter(command);

                adapter.Fill(table);

                foreach(var x in table.Select())
                {
                    Console.WriteLine($"Product ID: {x["Product ID"]}, Name: {x["Name"]}");
                }

                //Items = command.GetDataTable().Select().Select(x =>
                //    new Inventory((int)x["InventoryID"], (string)x["Name"], (int)x["CurrentStock"])).ToList();
            }
        }

        public Inventory GetItem(int inventoryID) => Items.FirstOrDefault(x => x.InventoryID == inventoryID);


    }
}
