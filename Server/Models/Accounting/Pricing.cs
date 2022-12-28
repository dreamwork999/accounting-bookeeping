using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AccountingBookkeeping.Server.Models.accounting
{
    [Table("pricings", Schema = "dbo")]
    public partial class Pricing
    {
        [Key]
        [Required]
        public string Id { get; set; }

        public string name { get; set; }

    }
}