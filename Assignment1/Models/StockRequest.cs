using System;

namespace Assignment1.Models
{
    public class StockRequest
    {
        private string requestID;
        private int requestQuantity;

        private Store store = new Store();
        private int prodID;

        private ProductTypes Products { get; set; }

        public StockRequest(string id, int storeID, int productID, int quantity)
        {
            requestID = id;
            store.StoreID = storeID;
            prodID = productID;
            requestQuantity = quantity;
            
        }


        public void addStockRequest()
        {


        }
    }
}
