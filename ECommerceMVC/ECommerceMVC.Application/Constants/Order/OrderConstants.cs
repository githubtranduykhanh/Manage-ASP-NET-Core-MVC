using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceMVC.Application.Constants.Order
{
    public class OrderConstants
    {
        public static readonly StatusOptions StatusOptions = new StatusOptions();
        public static readonly PaymentTypeOptions PaymentTypeOptions = new PaymentTypeOptions();      
    }

    public class StatusOptions
    {
        public readonly string Pending = "Pending";
        public readonly string Processing = "Processing";
        public readonly string Completed = "Completed";
        public readonly string Cancelled = "Cancelled";
    }


    public class PaymentTypeOptions
    {
        public readonly string CreditCard = "CreditCard";
        public readonly string DebitCard = "DebitCard";
        public readonly string PayPal = "PayPal";
        public readonly string BankTransfer = "Bank Transfer";
    }
}
