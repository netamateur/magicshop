﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Assignment1.Contorller;
using Assignment1.Models;



namespace Assignment1
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



            //Owner.checkOwnerInventory();

            var admin = new FranchiseHolder();

            Console.WriteLine("Enter threshold for re-stocking: ");
            var threshold = Console.ReadLine();

            Console.WriteLine("All inventory stock levels are equal to or above: " + threshold);

            admin.getStockThreshold(Int32.Parse(threshold), 1);


            Console.WriteLine("Enter request to process: ");
            var productRequest = Console.ReadLine();




            Console.Read();

            //Console.WriteLine("Please input the store id: ");

            //var id1 = Console.ReadLine();

            //Console.WriteLine("1- Melbourne CBD");
            //admin.checkStoreInventory(Int32.Parse(id1));

            //Console.WriteLine("Please input the store id: ");

            //var id2 = Console.ReadLine();
            //Console.WriteLine("2- East Melbourne");

            //admin.checkStoreInventory(Int32.Parse(id2));

        }

        }


    }

