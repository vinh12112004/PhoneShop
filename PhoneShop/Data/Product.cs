﻿using System.Text.Json.Serialization;

namespace PhoneShop.Data
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double Rate { get; set; }
        public string Category { get; set; }
        public ICollection<ImageProduct> ImageProducts { get; set; }
        [JsonIgnore]
        public ICollection<RateProduct> RateProducts { get; set; }
    }
}
