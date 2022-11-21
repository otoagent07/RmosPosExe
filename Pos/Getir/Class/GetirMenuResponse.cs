using System.Collections.Generic;

namespace Pos.Getir.Class
{
    public class GetirMenuResponse
    {
        public class Name
        {
            public string tr { get; set; }
            public string en { get; set; }
        }

        public class Option
        {
            public string id { get; set; }
            public Name name { get; set; }
            public int type { get; set; }
            public decimal price { get; set; }
            public int weight { get; set; }
            public int status { get; set; }
            public string product { get; set; }
        }

        public class OptionCategory
        {
            public string id { get; set; }
            public Name name { get; set; }
            public int minCount { get; set; }
            public int maxCount { get; set; }
            public int weight { get; set; }
            public int status { get; set; }
            public List<Option> options { get; set; }
        }

        public class Description
        {
            public string tr { get; set; }
            public string en { get; set; }
        }

        public class Product
        {
            public string id { get; set; }
            public string productCategory { get; set; }
            public List<OptionCategory> optionCategories { get; set; }
            public Name name { get; set; }
            public Description description { get; set; }
            public decimal price { get; set; }
            public decimal struckPrice { get; set; }
            public int weight { get; set; }
            public int status { get; set; }
            public bool isApproved { get; set; }
            public string imageURL { get; set; }
            public string wideImageURL { get; set; }
            public string chainProduct { get; set; }
        }

        public class ProductCategory
        {
            public string id { get; set; }
            public Name name { get; set; }
            public string restaurant { get; set; }
            public List<Product> products { get; set; }
            public bool isApproved { get; set; }
            public int weight { get; set; }
            public int status { get; set; }
            public string chainProductCategory { get; set; }
        }


        public class Root
        {
            public List<ProductCategory> productCategories { get; set; }
        }
    }
}