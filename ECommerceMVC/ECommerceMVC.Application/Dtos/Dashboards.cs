using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceMVC.Domain.Entities;

namespace ECommerceMVC.Application.Dtos
{
    public class Dashboards
    {

        public Profit Profit { get; set; } = new Profit();
        public Sales Sales { get; set; } = new Sales();
        public Payments Payments { get; set; } = new Payments();
        public Transactions Transactions { get; set; } = new Transactions();
        public ProfileReport ProfileReport { get; set; } = new ProfileReport();    
        public List<Growth> Growth { get; set; } = new List<Growth>();
        public OrderStatistics OrderStatistics { get; set; } = new OrderStatistics();
        public TotalBalance TotalBalance { get; set; } = new TotalBalance();
      
    }


    public class Profit
    {
        public double Money { get; set; }

        public double Percent { get; set; }
    }

    public class Sales
    {
        public double Money { get; set; }

        public double Percent { get; set; }
    }

    public class Payments
    {
        public double Money { get; set; }

        public double Percent { get; set; }
    }

    public class Transactions
    {
        public double Money { get; set; }

        public double Percent { get; set; }
    }



    public class ProfileReport
    {
        public int Year { get; set; }

        public double Money { get; set; }

        public double Percent { get; set; }
    }


    public class Growth
    {
        public int Year { get; set; }
        public double Money { get; set; }
    }


    public class OrderStatistics
    {
        public double TotalSales { get; set; }

        public double TotalOrders { get; set; }
        public List<OrderStatisticsTableItem> OrderStatisticsTable { get; set; } = new List<OrderStatisticsTableItem>();
    }

    public class OrderStatisticsTableItem
    {
        public string Image { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public double Price { get; set; }

        public double TotalAmount { get; set; }
    }

    public class TotalBalance
    {

        public double Money { get; set; }
        public int Percent { get; set; }
        public int PreviousTotalPercent { get; set; }
    }


   
}
