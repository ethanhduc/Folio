using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    [Table("Portfolios")]
    public class Portfolio
    {
        public string AppUserId { get; set; } //foreign key linking stock and user table
        public int StockId { get; set; }
        public AppUser AppUser { get; set; } //for dev
        public Stock Stock { get; set; } //for dev
    }
}