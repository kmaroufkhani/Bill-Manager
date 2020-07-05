using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace BillTimeScheduler.Models
{
    public class Bill
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        [DisplayAttribute(Name = "Due Date (MM/DD/YYYY)")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; } = new DateTime(3000, 3, 3);
        [Range(1,29)]
        [DisplayAttribute(Name ="Day of Month")]
        public int Day { get; set; }
        public virtual string ApplicationUserId { get; set; }
    }
}