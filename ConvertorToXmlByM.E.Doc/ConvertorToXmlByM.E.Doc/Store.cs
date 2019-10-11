namespace ConvertorToXmlByM.E.Doc
{
    public class Store
    {
        public string StoreCode;
        public string ProductCode;
        public double Balance = 0;
        public double Received = 0;
        public double Implemented = 0;
        public double ReplenishmentApplication = 0;

        public Store() { }

        public Store(string storeCode, string productCode)
        {
            StoreCode = storeCode;
            ProductCode = productCode;
        }
    }
}
