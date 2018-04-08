using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Assignment1.Controller;


namespace Assignment1.Models
{
    public class StockRequest
    {
        internal int requestID;
        internal int requestQuantity;
        internal int storeID;
        internal int prodID;

        //the process will equal to true after being processed by owner
        internal bool process { get; set; }

        //threshold request
        public StockRequest(int rID, int sID, int pID, int rQuantity)
        {
            requestID = rID;
            storeID = sID;
            prodID = pID;
            requestQuantity = rQuantity;
            process = false;
        }

        //add new item request
        public StockRequest(int rID, int sID, int pID)
        {
            requestID = rID;
            storeID = sID;
            prodID = pID;
            requestQuantity = 1;
            process = false;
        }

        private DataManager dm = DataManager.GetDataManager();

        //the stockRequest item should be move to Owner & Franchise Holder
        internal static List<StockRequest> requestItems = new List<StockRequest>();



    }
}