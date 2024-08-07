using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceMVC.Application.Dtos
{
    public class DashboardsChart
    {
        public List<int> ProfileReportChart { get; set; } = new List<int>();

        public int GrowthChart {  get; set; } = 0;

        public List<TotalRevenueChart> TotalRevenue { get; set; } = new List<TotalRevenueChart>();


        public StatisticsChart StatisticsChart { get; set; } = new StatisticsChart();


        public TotalBalanceChart TotalBalance { get; set; } = new TotalBalanceChart();
    }

    public class TotalRevenueChart
    {
        public int Name { get; set; }
        public List<int> Data { get; set; } = new List<int>();

    }


    public class StatisticsChart
    {

        public List<string> Labels { get; set; } = new List<string>();

        public List<int> Data { get; set; } = new List<int>();
    }

    public class TotalBalanceChart
    {

        public List<int> IncomeChart { get; set; } = new List<int>();

        public int WeeklyExpensesChart { get; set; } = 0;
    }
}
