using ECommerceMVC.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceMVC.Application.Interfaces
{
    public interface IDashboardsService
    {
        Task<ResponseService<DashboardsChart>> GetChartAsync(int year);
        Task<ResponseService<Dashboards>> GetDataAsync(int year);
    }
}
