using System;

namespace Assignment1.Models
{
    /*The Inventory class actually indicates the OwnerInventory*/
    public class Inventory
    {
        internal int ProductID { get; }
        internal string ProductName { get; }
        internal int StockLevel { get; }

        public Inventory(int productID, string pName, int stockLevel)
        {
            ProductID = productID;
            ProductName = pName;
            StockLevel = stockLevel;
        }
    }
}