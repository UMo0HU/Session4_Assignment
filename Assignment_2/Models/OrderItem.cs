﻿using System.ComponentModel.DataAnnotations;

namespace Assignment_2.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        [Range(1, 10)]
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
