namespace ConvertorToXmlByM.E.Doc.Models
{
    public class Store
    {
        /// <summary>
        /// Код склада
        /// </summary>
        public string StoreCode { get; set; }
        /// <summary>
        /// Код товара
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// Остаток
        /// </summary>
        public double Balance { get; set; } = 0;
        /// <summary>
        /// Получено
        /// </summary>
        public double Received { get; set; } = 0;
        /// <summary>
        /// Выбыло
        /// </summary>
        public double Implemented { get; set; } = 0;
        /// <summary>
        /// Заказ на пополнение
        /// </summary>
        public double ReplenishmentApplication { get; set; } = 0;
        /// <summary>
        /// Налоговая ставка
        /// </summary>
        public double TaxRate { get; set; }

        public Store(string storeCode, string productCode, double taxRate)
        {
            StoreCode = storeCode;
            ProductCode = productCode;
            TaxRate = taxRate;
        }
    }
}
