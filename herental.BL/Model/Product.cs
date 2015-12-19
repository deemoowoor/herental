using System.Collections.Generic;

namespace herental.BL.Model
{
    public class Product
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public int ProductTypeId { get; set; }
        public ProductType Type { get; set; }

        public decimal GetPriceQuote()
        {
            return 0.0M;
        }

        public IList<ProductOrder> ProductOrders { get; set; }
    }
}
