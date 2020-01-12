using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SpyStore.Models.Entities.Base
{
    public class OrderBase: EntityBase
    {
        [DataType(DataType.Date), Display(Name ="Date Ordered")]
        public DateTime OrderDate { get; set; }
        [DataType(DataType.Date), Display(Name = "Date Shipped")]
        public DateTime ShipDate { get; set; }
        [Display(Name ="Customer")]
        public int CustomerId { get; set; }
    }
}
