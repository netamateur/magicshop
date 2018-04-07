using Assignment1.Controller;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Assignment1.Models
{
    public class StockRequest
    {
        internal int requestID;
        internal int requestQuantity;
        internal int storeID;
        internal int prodID;

        public StockRequest(int rID, int sID, int pID, int rQuantity)
        {
            requestID = rID;
            storeID = sID;
            prodID = pID;
            requestQuantity = rQuantity;
        }

        private DataManager dm = DataManager.GetDataManager();
        internal List<StockRequest> requestItems = new List<StockRequest>();

        }
    
}

