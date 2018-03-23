using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment1
{
    public class Inventory
    {
        public int ProductID { get; }
        public string ProductName { get; }
        public int StockLevel { get; }

        public Inventory(int productID, string productName, int stockLevel)
        {
            ProductID = productID;
            ProductName = productName;
            StockLevel = stockLevel;
        }
    }
}