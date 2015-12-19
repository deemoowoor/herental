using herental.BL.Interfaces;
using herental.BL.Model;
using System.Collections.Generic;

namespace herental.BL.Commands
{
    public class ListProductsArgument
    {
        public string Name { get; set; }

        public IEnumerable<ProductType> ProductTypes { get; set; }

        public IPagingInfo Paging { get; set; }
    }
}
