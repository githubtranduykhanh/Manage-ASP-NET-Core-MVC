using AutoMapper;
using ECommerceMVC.Domain.Abstract.Cloudinary;
using ECommerceMVC.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceMVC.Application.Interfaces;
using ECommerceMVC.Application.Dtos.DataTable;
using ECommerceMVC.Application.Dtos.NewCategories;
using ECommerceMVC.Application.Dtos;
using Microsoft.EntityFrameworkCore;
using ECommerceMVC.Application.Dtos.Order;
using ECommerceMVC.Domain.Entities;
using System.Linq.Expressions;

namespace ECommerceMVC.Application.Services
{
    public class DashboardsService : IDashboardsService
    {
        private readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        protected readonly ICloudinaryService _cloudinaryService;
        public DashboardsService(IUnitOfWork unitOfWork, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<ResponseService<DashboardsChart>> GetChartAsync(int year)
        {

            int previousYear = year - 1;
            var orders = await _unitOfWork.OrderRepositorie.GetOrdersWithDetailsAndImagesAsync();



            // Lấy dữ liệu từ cơ sở dữ liệu
            var revenueData = await orders
                .Where(o => o.CreatedAt.Year == year)
                .GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalRevenue = g.Sum(o => o.TotalAmount)
                })
                .ToListAsync();

            var listTotalRevenue = revenueData
                 .GroupBy(g => g.Year)
                 .Select(g => new TotalRevenueChart
                 {
                     Name = g.Key,
                     Data = Enumerable.Range(1, 12)
                         .Select(month => (int)Math.Round(g
                             .Where(x => x.Month == month)
                             .Sum(x => x.TotalRevenue)))
                         .ToList()
                 })
                 .ToList();

            // Lấy dữ liệu doanh thu theo năm
            var yearlyData = await orders
                .GroupBy(o => o.CreatedAt.Year)
                .Select(g => new
                {
                    Year = g.Key,
                    TotalAmount = g.Sum(o => o.TotalAmount)
                })
                .OrderBy(g => g.Year)
                .ToListAsync();


            var salesData = await orders
                .Where(o => o.CreatedAt.Year == year)
                .GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalAmount = g.Sum(o => o.TotalAmount),
                    OrderCount = g.Count()

                })
                .OrderBy(d => d.Year)
                .ThenBy(d => d.Month)
                .ToListAsync();


            // Lấy doanh thu của năm trước
            var previousYearSales = await orders
                .Where(o => o.CreatedAt.Year == previousYear)
                .GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalAmount = g.Sum(o => o.TotalAmount),
                    OrderCount = g.Count()
                })
                .OrderBy(d => d.Year)
                .ThenBy(d => d.Month)
                .ToListAsync();

            // Lấy tổng doanh thu từ tất cả các đơn hàng trong năm được chỉ định
            var totalRevenue = await orders
                .Where(o => o.CreatedAt.Year == year)
                .SelectMany(o => o.DbOrderDetails)
                .Select(od => od.Quantity * od.UnitPrice)
                .SumAsync();

            // Truy vấn dữ liệu về các sản phẩm đã đặt hàng trong năm được chỉ định và tính toán
            var statisticsChart = orders
                .Where(o => o.CreatedAt.Year == year)
                .SelectMany(o => o.DbOrderDetails)
                 .AsEnumerable() // Chuyển đổi sang client-side
                .GroupBy(od => new
                {
                    od.IdProduct,
                    Product = od.IdProductNavigation
                })
                .Select(g => new 
                {
                  
                    Title = g.Key.Product.Name,            
                    Data = (int)Math.Round((totalRevenue == 0 ? 0 : (g.Sum(od => od.Quantity * od.UnitPrice) / totalRevenue) * 100))
                })
                .OrderByDescending(ps => ps.Data)
                .ToList();


            var listOrderCountMonth = salesData.Select(p => p.OrderCount).ToList();
            int growthPercent = 100;
        

            // Kiểm tra nếu có ít hơn hai năm dữ liệu thì không thể tính toán tăng trưởng
            if (yearlyData.Count > 2)
            {
                // Tính toán tỷ lệ phần trăm tăng trưởng giữa năm cuối cùng và năm trước đó
                var lastYearData = yearlyData.Last();
                var previousYearData = yearlyData[yearlyData.Count - 2];
                growthPercent = (int)Math.Round((lastYearData.TotalAmount - previousYearData.TotalAmount) / previousYearData.TotalAmount * 100);
            }

            var dashboardsChart = new DashboardsChart
            {
                ProfileReportChart = listOrderCountMonth,
                GrowthChart = growthPercent,
                TotalRevenue = listTotalRevenue,
                StatisticsChart = new StatisticsChart
                {
                    Labels = statisticsChart.Select(l => l.Title).ToList(),
                    Data = statisticsChart.Select(l => l.Data).ToList(),
                },
                TotalBalance = new TotalBalanceChart
                {
                    IncomeChart = listOrderCountMonth,
                    WeeklyExpensesChart = growthPercent
                }
            };


            return new ResponseService<DashboardsChart>
            {
                success = true,
                message = "Get chart successfully.",
                data = dashboardsChart
            };
        }

        public async Task<ResponseService<Dashboards>> GetDataAsync(int year)
        {
            int previousYear = year - 1;

            var orders = await _unitOfWork.OrderRepositorie.GetOrdersWithDetailsAndImagesAsync();


            // Lấy tổng doanh thu của từng năm
            var listTotalGrowth = await orders
                .GroupBy(o => o.CreatedAt.Year)
                .Select(g => new Growth
                {
                    Year = g.Key,
                    Money = g.Sum(o => o.TotalAmount)
                })
                .OrderBy(d => d.Year)
                .ToListAsync();


            var salesData = await orders
                .Where(o => o.CreatedAt.Year == year)
                .GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month })
                .Select(g => new 
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Sales = g.Sum(o => o.TotalAmount),
                    Profit = g.Sum(o => o.TotalAmount) * 0.1, // Giả sử lợi nhuận là 10% doanh thu
                    Payments = g.Sum(o => o.TotalAmount) * 0.2, // Giả sử thanh toán là 20% doanh thu
                    Transactions = g.Count(), // Giả sử mỗi đơn hàng là một giao dịch                  
                    OrderCount = g.Count()

                })
                .OrderBy(d => d.Year)
                .ThenBy(d => d.Month)
                .ToListAsync();

            // Lấy doanh thu của năm trước
            var previousYearSales = await orders
                .Where(o => o.CreatedAt.Year == previousYear)
                .GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalAmount = g.Sum(o => o.TotalAmount),
                    OrderCount = g.Count()
                })
                .OrderBy(d => d.Year)
                .ThenBy(d => d.Month)
                .ToListAsync();


            // Truy vấn dữ liệu về các sản phẩm đã đặt hàng trong năm được chỉ định và tính toán
            var orderStatisticsTable = orders
                .Where(o => o.CreatedAt.Year == year)
                .SelectMany(o => o.DbOrderDetails)
                .AsEnumerable() // Chuyển đổi sang client-side
                .GroupBy(od => new
                {
                    od.IdProduct,
                    Product = od.IdProductNavigation
                })
                .Select(g => new OrderStatisticsTableItem
                {
                    Image = g.Key.Product.DbProductImages
                                .Select(pi => pi.IdImageNavigation.Url) // Truy cập URL của hình ảnh
                                .OrderBy(url => url) // Sắp xếp URL để đảm bảo lấy hình ảnh đầu tiên (hoặc chọn cách sắp xếp phù hợp)
                                .FirstOrDefault() ?? string.Empty, // Trả về chuỗi rỗng nếu không có hình ảnh
                    Title = g.Key.Product.Name,
                    Price = g.Key.Product.Price,
                    TotalAmount = g.Sum(od => od.Quantity * od.UnitPrice),                   
                })
                .OrderByDescending(ps => ps.TotalAmount)
                .ToList();


            var lastMonthData = salesData[^1];
            var previousMonthData = salesData[^2];


            var totalSales = salesData.Sum(d => d.Sales);
            var totalProfit = salesData.Sum(d => d.Profit);
            var totalPayments = salesData.Sum(d => d.Payments);
            var totalTransactions = salesData.Sum(d => d.Transactions);

            double totalSalesLastMonth = 0.0,             
                totalpreviousYearSalesLastMonth = 0.0;

            if (salesData != null && salesData.Any())
            {
                 totalSalesLastMonth = salesData?.Last()?.Sales ?? 0.0;      
            }

            if (previousYearSales != null && previousYearSales.Any())
            {
                totalpreviousYearSalesLastMonth = previousYearSales?.Last()?.TotalAmount ?? 0.0;
            }

           
            var totalpreviousYearSales = previousYearSales.Sum(d => d.TotalAmount);
           


         


             var dashboards = new Dashboards
             {
                Sales = new Sales
                {
                    Money = totalSales,
                    Percent = PercentToDouble(lastMonthData.Sales, previousMonthData.Sales)
                },
                Profit = new Profit
                {
                    Money = totalProfit,
                    Percent = PercentToDouble(lastMonthData.Profit, previousMonthData.Profit)
                },
                Payments = new Payments
                {
                    Money = totalPayments,
                    Percent = PercentToDouble(lastMonthData.Payments, previousMonthData.Payments)
                },
                Transactions = new Transactions
                {
                    Money = totalTransactions,
                    Percent = PercentToDouble(lastMonthData.Transactions, previousMonthData.Transactions)
                },
                ProfileReport = new ProfileReport
                {
                    Year = year,
                    Money = totalSales,
                    Percent = PercentToDouble(lastMonthData.Sales, previousMonthData.Sales)
                },
                OrderStatistics = new OrderStatistics
                {
                    TotalSales = totalSales,
                    TotalOrders = salesData.Sum(d => d.OrderCount),
                    OrderStatisticsTable = orderStatisticsTable

                },
                TotalBalance = new TotalBalance
                {
                    Money = totalSales - (totalSales * 0.2), // Giả sử chi phí là 20% doanh thu
                    Percent = PercentToInt(totalSales, totalSalesLastMonth),
                    PreviousTotalPercent = PercentToInt(totalpreviousYearSales, totalpreviousYearSalesLastMonth)// Giá trị giả định cho phần trăm thay đổi
                },              
                Growth = listTotalGrowth

             };

            return new ResponseService<Dashboards>
            {
                success = true,
                message = "Get data successfully.",
                data = dashboards
            };
        }


       

        private double PercentToDouble(double lastMonthData, double previousMonthData)
        {
            // Kiểm tra trường hợp totalAmountAllMonths bằng 0
            if (lastMonthData == 0)
            {
                // Nếu totalAmountAllMonths bằng 0 và totalAmountLastMonth cũng bằng 0, trả về 0
                if (previousMonthData == 0)
                {
                    return 0;
                }

                // Nếu totalAmountAllMonths bằng 0 nhưng totalAmountLastMonth không bằng 0, có thể xử lý theo cách khác, ví dụ:
                return lastMonthData > 0 ? double.PositiveInfinity : double.NegativeInfinity;
            }

            // Thực hiện phép chia nếu điều kiện hợp lệ
            return Math.Round(((lastMonthData - previousMonthData) / previousMonthData) * 100 , 2);
        }

        private int PercentToInt(double totalAmountAllMonths, double totalAmountLastMonth)
        {
            // Kiểm tra trường hợp totalAmountAllMonths bằng 0
            if (totalAmountAllMonths == 0)
            {
                // Nếu totalAmountAllMonths bằng 0 và totalAmountLastMonth cũng bằng 0, trả về 0
                if (totalAmountLastMonth == 0)
                {
                    return 0;
                }

                // Nếu totalAmountAllMonths bằng 0 nhưng totalAmountLastMonth không bằng 0, có thể xử lý theo cách khác
                // Ví dụ: trả về một giá trị int lớn hoặc nhỏ, tùy thuộc vào tình huống
                return totalAmountLastMonth > 0 ? int.MaxValue : int.MinValue;
            }

            // Thực hiện phép chia và làm tròn kết quả thành int
            return (int)Math.Round(((totalAmountLastMonth - totalAmountAllMonths) / totalAmountAllMonths) * 100,2);
        }
    }
}
