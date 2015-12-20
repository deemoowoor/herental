using System;
using System.Collections.Generic;

namespace herental.BL.Model
{
    public class ProductType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IList<Product> Products { get; set; }

        public decimal CalculatePrice()
        {
            throw new NotImplementedException();
        }
    }
}