using System.Collections.Generic;

namespace herental.Models
{
    public class ProductListViewModel
    {
        public IList<ProductViewModel> Products { get; set; }
    }

    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }
    }
}