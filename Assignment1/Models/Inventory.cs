using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment1
{
    /*The Inventory class actually indicates the OwnerInventory*/
    public class Inventory
    {
        private int ProductID { get; }
        public string productName { get; }
        private int StockLevel { get; }

        public Inventory(int productID, string pName, int stockLevel)
        {
            ProductID = productID;
            productName = pName;
            StockLevel = stockLevel;
        }
    }
}