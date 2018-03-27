using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment1
{
    /*The Inventory class actually indicates the OwnerInventory*/
    public class Inventory
    {
        private int ProductID { get; }
        private string ProductName { get; }
        private int StockLevel { get; }

        public Inventory(int productID, string productName, int stockLevel)
        {
            ProductID = productID;
            ProductName = productName;
            StockLevel = stockLevel;
        }
    }
}