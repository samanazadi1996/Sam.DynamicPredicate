namespace Sam.DynamicPredicateAPI.Models
{
    public class Product
    {
        private Product()
        {
        }
        public Product(string name, double price, string barCode)
        {
            Name = name;
            Price = price;
            BarCode = barCode;
        }
        public long Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string BarCode { get; set; }

    }
}
