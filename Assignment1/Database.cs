//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Text;

//namespace Assignment1
//{
//    class Database
//    {

//        public List<Inventory> Items { get; set; }

//        public static DataTable GetDataTable(this SqlCommand command)
//        {
//            var table = new DataTable();
//            new SqlDataAdapter(command).Fill(table);

//            return table;
//        }


//        public void connectDB()
//        {
//            using (var connection = new SqlConnection("server=wdt2018.australiaeast.cloudapp.azure.com;uid=s3419529;database=s3419529;pwd=abc123"))
//            {
//                connection.Open();

//                var command = connection.CreateCommand();
//                command.CommandText = "select * from "; //Inventory of specific store

//                var table = new DataTable();
//                var adapter = new SqlDataAdapter(command);

//                adapter.Fill(table);

//                foreach (var x in table.Select())
//                {
//                    Console.WriteLine($"Product ID: {x["Product ID"]}, Name: {x["Name"]}");
//                }

//                //Items = command.GetDataTable().Select().Select(x =>
//                //    new Inventory((int)x["InventoryID"], (string)x["Name"], (int)x["CurrentStock"])).ToList();
//            }
//        }

//        public Inventory GetItem(int productID) => Items.FirstOrDefault(x => x.ProductID == productID);
//    }
//}
