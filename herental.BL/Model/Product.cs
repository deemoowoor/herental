﻿using herental.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herental.BL.Model
{
    public class Product
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        public ProductType Type { get; set; }

        public decimal GetPriceQuote()
        {
            return 0.0M;
        }
    }
}