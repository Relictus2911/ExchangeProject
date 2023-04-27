using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeProject.Database.Models
{
    public class ExchangeRate
    {
        public int Id { get; set; }
        public decimal Rate { get; set; }
        public virtual Currency Currency { get; set; }

        public virtual ExchangeDate ExchangeDate { get; set; }

        public ExchangeRate() { }
    }
}
