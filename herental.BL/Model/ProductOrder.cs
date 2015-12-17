using System.Collections.Generic;

namespace herental.BL.Model
{
    public class ProductOrder
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public IList<Product> Products { get; set; }
    }
}
