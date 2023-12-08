using System;

namespace ECommerce.Api.Orders.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string OrderDate { get; set; } // https://www.tutorialsteacher.com/csharp/csharp-datetime
        public int Total { get; set; }
        // public OrderItem[] Item { get; set; } // https://stackoverflow.com/questions/7031299/array-property-syntax-in-c-sharp
    }
}
