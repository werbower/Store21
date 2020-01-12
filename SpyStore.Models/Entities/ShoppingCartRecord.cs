using Newtonsoft.Json;
using SpyStore.Models.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SpyStore.Models.Entities
{
    [Table("ShoppingCartRecords", Schema ="Store")]
    public class ShoppingCartRecord: ShoppingCartRecordBase
    {
        [ForeignKey(nameof(CustomerId)), JsonIgnore]
        public Customer CustomerNavigation { get; set; }
        [ForeignKey(nameof(ProductId)), JsonIgnore]
        public Product ProductNavigation { get; set; }
    }
}
