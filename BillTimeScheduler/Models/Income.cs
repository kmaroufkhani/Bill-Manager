using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace BillTimeScheduler.Models
{
    public class Income
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        [DisplayAttribute(Name = "Next Paycheck (MM/DD/YYYY)")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [DateValidator(ErrorMessage = "Past dates are invalid!")]
        public DateTime PayDate { get; set; }
        [DisplayAttribute(Name = "Frequency (Days)")]
        public int Frequency { get; set; }
        [DisplayAttribute(Name = "Current Balance")]
        public decimal Balance { get; set; }
        public virtual string ApplicationUserId { get; set; }
    }
}