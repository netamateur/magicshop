using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment1
{
    /*The Inventory class actually indicates the OwnerInventory*/
    public class Inventory
    {
        internal int ProductID { get; }
        internal string ProductName { get; }
        internal int StockLevel { get; set; }

        public Inventory(int pID, string pName, int stockLevel)
        {
            ProductID = pID;
            ProductName = pName;
            StockLevel = stockLevel;
        }
    }
}