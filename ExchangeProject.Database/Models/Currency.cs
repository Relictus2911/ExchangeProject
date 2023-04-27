using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeProject.Database.Models
{
    public class Currency
    {
        [Key]
        public string Code { get; set; }
        public int Amount { get; set; }

        public Currency()
        {
        }
    }
}
