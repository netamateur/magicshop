using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
//using Assignment1.Controller;
using Assignment1.Models;



namespace Assignment1.Controller
{
    class Program
    {
        

        static void Main(string[] args)
        {
            /*
            var dm = DataManager.GetDataManager();

            string ConnServer = dm.ConnectionString;

            var table = dm.fetchData("select * from store;", ConnServer);


            foreach (DataRow row in table.Rows)
            {
                Console.WriteLine("{0}\n{1}\n",
                                  row["StoreID"],
                                  row["Name"]);
            }*/





            var admin = new FranchiseHolder();

            Console.WriteLine("Please input the store id: ");

            var storeId = Console.ReadLine();

            Console.WriteLine("Display the optional list");
            admin.checkOwnerItem(Int32.Parse(storeId)); //Exception: Column 'Name' does not belong to table .

            Console.WriteLine("Please input the product you want to increase: ");

            var productID = Console.ReadLine();
            admin.addStockRequest(Int32.Parse(productID), 1, Int32.Parse(storeId)); 

            Console.WriteLine("\nOwner: I would like to see my Inventory.\n");

            Owner.checkOwnerInventory();

            Console.WriteLine("\nOwner: I would like to see all the stock request.\n");

            Owner.getStockRequestTable();
            Owner.displayOwnerRequest();
            /*
            foreach (Owner.OwnerRequest temp in Owner.displayedRequest)
            {
                Console.WriteLine("{0} {1} {2} {3} {4} {5}\n",
                                          temp.request.requestID,
                                          temp.request.storeID,
                                          temp.productName,
                                          temp.request.requestQuantity,
                                          temp.currentOwnerStock,
                                          temp.availability);

            }*/
            //Exception: Column 'StockRequesetID' does not belong to table Table.
            //Owner.getStockRequestTable();

            /* test the fetchData() work well
            string query = "select * from StockRequest;";


            string Connection = DataManager.GetDataManager().ConnectionString;

            var table = DataManager.GetDataManager().fetchData(query, Connection);

            foreach (DataRow row in table.Rows)
            {
                Console.WriteLine("{0} {1} {2} {3}\n",
                                  row["StockRequestID"],
                                  row["StoreID"],
                                  row["ProductID"],
                                  row["Quantity"]);
            } */

            /*
            Console.WriteLine("Please input the store id: ");

            var id2 = Console.ReadLine();
            Console.WriteLine("2- East Melbourne");

            admin.checkStoreInventory(Int32.Parse(id2)); */

            /*
            Owner.checkOwnerInventory();

            Console.WriteLine("Please enter the productID:"); */
           /*
            var productID = Console.ReadLine();
            Owner.resetTo20(Int32.Parse(productID)); */





            }

        }


    }

