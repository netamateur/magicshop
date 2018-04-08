using Assignment1.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment1.Models
{
    public class Store
    {
        public int StoreID { get; set; }

        public string Name { get; set; }

        //private ProductTypes Products { get; set; }

        public Store(int sID) => StoreID = sID;

        public enum StoreFranchise
        {
            MelbourneCBD = 1,
            NorthMelbourne = 2,
            EastMelbourne = 3,
            SouthMelbourne = 4,
            Westmelbourne = 5

        }

    }
}
