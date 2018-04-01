using System;
namespace Assignment1.Models
{
    public class StoreRequest
    {
        private string requestID;
        private int quantity;

        private Store store = new Store();
        private ProductTypes Products { get; set; }

        public StoreRequest(string id, int storeID, int productID, int quantity)
        {
            requestID = id;


        }
    }
}
