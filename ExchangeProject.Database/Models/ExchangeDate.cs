using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeProject.Database.Models
{
    public class ExchangeDate
    {
        [Key]
        public DateTime Date { get; set; }

        public ExchangeDate(string date)
        {
            Date = DateTime.Parse(date);
        }

        public ExchangeDate() { }
    }
}
