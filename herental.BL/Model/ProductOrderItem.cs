using herental.BL.Interfaces;
using System;

namespace herental.BL.Model
{
    public class ProductOrderItem
    {
        public int Id { get; set; }

        public Guid OrderId { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Period { get; set; }

        public bool Deleted { get; set; }

        public decimal PriceQuote { get; set; }

        public void UpdatePriceQuote(HerentalBL context, IPriceFormulaManager formulaMgr)
        {
            if (Product == null)
            {
                Product = context.Products.Find(ProductId);
            }

            if (Product.Type == null)
            {
                Product.Type = context.ProductTypes.Find(Product.ProductTypeId);
            }

            PriceQuote = formulaMgr.CalculatePrice(Product.Type.Name, Period);
        }
    }
}
