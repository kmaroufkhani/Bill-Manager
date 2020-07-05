using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillTimeScheduler.Models
{
    public class BillsAndIncome
    {
        public List<BillTimeScheduler.Models.Bill> Bills {get; set;}
        public List<BillTimeScheduler.Models.Income> Income { get; set; }
    }
}