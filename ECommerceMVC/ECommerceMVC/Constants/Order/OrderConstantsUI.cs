using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceMVC.UI.Constants.Order
{
    public static class OrderConstantsUI
    {
        public static readonly List<SelectListItem> StatusOptions = new List<SelectListItem>
        {
            new SelectListItem { Value = "Pending", Text = "Pending" },
            new SelectListItem { Value = "Processing", Text = "Processing" },
            new SelectListItem { Value = "Completed", Text = "Completed" },
            new SelectListItem { Value = "Cancelled", Text = "Cancelled" }
        };


        public static readonly List<SelectListItem> PaymentTypeOptions = new List<SelectListItem>
        {
            new SelectListItem { Value = "CreditCard", Text = "Credit Card" },
            new SelectListItem { Value = "DebitCard", Text = "Debit Card" },
            new SelectListItem { Value = "PayPal", Text = "PayPal" },
            new SelectListItem { Value = "BankTransfer", Text = "Bank Transfer" }
        };
    }
}
