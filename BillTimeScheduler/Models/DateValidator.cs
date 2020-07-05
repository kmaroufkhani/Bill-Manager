using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BillTimeScheduler.Models
{
    public class DateValidator : ValidationAttribute
    {
        public override bool IsValid(object value)// Return a boolean value: true == IsValid, false != IsValid
        {
            DateTime date = Convert.ToDateTime(value);
            return date >= DateTime.Now.Date; // Dates Greater than or equal to today are valid (true)
        }
    }
}