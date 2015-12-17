using herental.BL.Model;
using System.Data.Entity;
using System.Linq;

namespace herental.BL
{
    public class HerentalBL : DbContext
    {
        public HerentalBL() : base() { }

        public void WarmUp()
        {
            Products.ToList();
            ProductOrders.ToList();
            Carts.ToList();
        }

        // virtual to allow mocking in tests
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductOrder> ProductOrders { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
    }
}
