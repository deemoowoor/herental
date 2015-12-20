using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace herental.Models
{
    public class ProductListViewModel
    {
        public IList<ProductViewModel> Products { get; set; }
    }

    public class ProductViewModel
    {
        //[JsonConverter(typeof(Int32))]
        [JsonProperty(PropertyName = "Id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        //[JsonConverter(typeof(Int32))]
        [JsonProperty(PropertyName = "ProductTypeId")]
        public int ProductTypeId { get; set; }

        [JsonProperty(PropertyName = "Type")]
        public string ProductTypeName { get; set; }
    }
}