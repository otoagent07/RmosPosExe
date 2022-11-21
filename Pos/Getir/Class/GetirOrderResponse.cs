using System;
using System.Collections.Generic;

namespace Pos.Getir.Class
{
    public class GetirOrderResponse
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class Location
        {
            public string lat { get; set; }
            public string lon { get; set; }
        }

        public class DeliveryAddress
        {
            public string id { get; set; }
            public string address { get; set; }
            public string aptNo { get; set; }
            public string floor { get; set; }
            public string doorNo { get; set; }
            public string city { get; set; }
            public string district { get; set; }
            public string description { get; set; }
        }

        public class Client
        {
            public string id { get; set; }
            public string name { get; set; }
            public Location location { get; set; }
            public string clientPhoneNumber { get; set; }
            public string contactPhoneNumber { get; set; }
            public string clientUnmaskedPhoneNumber { get; set; }
            public DeliveryAddress deliveryAddress { get; set; }
        }

        public class Courier
        {
            public string id { get; set; }
            public int status { get; set; }
            public string name { get; set; }
            public Location location { get; set; }
        }

        public class Name
        {
            public string tr { get; set; }
            public string en { get; set; }
        }

        public class Option
        {
            public string option { get; set; }
            public string product { get; set; }
            public Name name { get; set; }
            public decimal price { get; set; }
            public List<string> tr { get; set; }
            public List<string> en { get; set; }
            public List<OptionCategory> optionCategories { get; set; }
        }

        public class OptionCategory
        {
            public string optionCategory { get; set; }
            public Name name { get; set; }
            public List<Option> options { get; set; }
        }

        public class Title
        {
            public string tr { get; set; }
            public string en { get; set; }
        }

        public class DisplayInfo
        {
            public Title title { get; set; }
            public Option options { get; set; }
        }

        public class Product
        {
            public string id { get; set; }
            public string imageURL { get; set; }
            public string wideImageURL { get; set; }
            public string count { get; set; }
            public string product { get; set; }
            public string chainProduct { get; set; }
            public Name name { get; set; }
            public decimal price { get; set; }
            public decimal optionPrice { get; set; }
            public decimal priceWithOption { get; set; }
            public decimal totalPrice { get; set; }
            public decimal totalOptionPrice { get; set; }
            public decimal totalPriceWithOption { get; set; }
            public List<OptionCategory> optionCategories { get; set; }
            public DisplayInfo displayInfo { get; set; }
            public string note { get; set; }
        }

        public class Restaurant
        {
            public string id { get; set; }
        }

        public class PaymentMethodText
        {
            public string en { get; set; }
            public string tr { get; set; }
        }

        public class Root
        {
            public string id { get; set; }
            public int status { get; set; }
            public bool isScheduled { get; set; }
            public string confirmationId { get; set; }
            public Client client { get; set; }
            public Courier courier { get; set; }
            public List<Product> products { get; set; }
            public string clientNote { get; set; }
            public decimal totalPrice { get; set; }
            public DateTime checkoutDate { get; set; }
            public DateTime? scheduledDate { get; set; }
            public int deliveryType { get; set; }
            public bool doNotKnock { get; set; }
            public bool isEcoFriendly { get; set; }
            public Restaurant restaurant { get; set; }
            public int paymentMethod { get; set; }
            public PaymentMethodText paymentMethodText { get; set; }
            public decimal totalDiscountedPrice { get; set; }
        }
    }
}