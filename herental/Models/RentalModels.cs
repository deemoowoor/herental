using Newtonsoft.Json;

namespace herental.Models
{
    public class Product
    {
        [JsonProperty()]
        public int Id { get; set; }

        [JsonProperty()]
        public string Name { get; set; }
        
        [JsonProperty()]
        public ProductType Type { get; set; }
    }

    public class ProductType
    {
        [JsonProperty()]
        public int Id { get; set; }

        [JsonProperty()]
        public string Name { get; set; }
    }

    public class ProductOrder
    {
        [JsonProperty()]
        public int Id { get; set; }

        [JsonProperty()]
        public Product Product { get; set; }

        [JsonProperty()]
        public int Period { get; set; }

        [JsonProperty()]
        public decimal PriceQuote { get; set; }
    }
}