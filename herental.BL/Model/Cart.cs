using System.Collections.Generic;

namespace herental.BL.Model
{
    public class Cart
    {
        public int Id { get; set; }

        public IList<ProductOrder> ProductOrders { get; set; }
    }
}
