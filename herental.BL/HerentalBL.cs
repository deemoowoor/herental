using herental.BL.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace herental.BL
{
    public class HerentalBL : DbContext
    {
        public HerentalBL() : base()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<HerentalBL>());
        }

        #region static utilities

        public static void WarmUp()
        {
            using (var db = new HerentalBL())
            {
                db.ProductTypes.ToList();
                db.Products.ToList();
                db.OrderedProducts.ToList();
            }
        }

        public static void SeedWithTestData()
        {
            using (var db = new HerentalBL())
            {
                if (db.ProductTypes.ToList().Count != 0)
                {
                    return;
                }

                List<ProductType> productTypes = new List<ProductType>()
                {
                    new ProductType() { Name = "Heavy" },
                    new ProductType() { Name = "Regular" },
                    new ProductType() { Name = "Specialized" }
                };

                db.ProductTypes.AddRange(productTypes);
                db.SaveChanges();
                
                List<Product> products = new List<Product>()
                {
                    new Product() { Name = "Caterpillar bulldozer",
                        Type = db.ProductTypes.Where(i => i.Name == "Heavy").First() },
                    new Product() { Name = "KamAZ truck", Type = new ProductType() { Name ="Regular" } },
                    new Product() { Name = "Komatsu crane", Type = new ProductType() { Name = "Heavy" } },
                    new Product() { Name = "Volvo steamroller", Type = new ProductType() { Name = "Regular" } },
                    new Product() { Name = "Bosch jackhammer", Type = new ProductType() { Name = "Specialized" } }
                };
                
                db.Products.AddRange(products);
                db.SaveChanges();
            }
        }

        #endregion

        // virtual to allow mocking in tests
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductOrderItem> OrderedProducts { get; set; }
        public virtual DbSet<ProductType> ProductTypes { get; set; }
    }
}
