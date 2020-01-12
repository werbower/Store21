using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SpyStore.Models.Entities.Base;
using Newtonsoft.Json;

namespace SpyStore.Models.Entities
{
    [Table("Products", Schema ="Store")]
    public class Product: EntityBase
    {

        public ProductDetails Details { get; set; } = new ProductDetails();
        public bool IsFeatured { get; set; }
        [DataType(DataType.Currency)]
        public decimal UnitCost { get; set; }
        [DataType(DataType.Currency)]
        public decimal CurrentPrice { get; set; }
        public int UnitsInStock { get; set; }

        [Required]
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId)), JsonIgnore]
        public Category CategoryNavigation { get; set; }
        [NotMapped]
        public string CategoryName => CategoryNavigation?.CategoryName;

        [InverseProperty(nameof(OrderDetail.ProductNavigation))]
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        [InverseProperty(nameof(ShoppingCartRecord.ProductNavigation))]
        public List<ShoppingCartRecord> ShoppingCartRecords { get; set; } = new List<ShoppingCartRecord>();
    }
}
