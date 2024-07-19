using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using ECommerceMVC.DataAccess.Data;
using Microsoft.Extensions.Configuration;

namespace ECommerceMVC.DataAccess.Factories
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ECommerceContext>
    {
        public ECommerceContext CreateDbContext(string[] args)
        {

            var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            //.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) // Đọc từ appsettings.json
            .AddJsonFile("appsettingsLocal.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true) // Tệp cấu hình môi trường
            .AddEnvironmentVariables() // Biến môi trường
            .Build();
            var optionsBuilder = new DbContextOptionsBuilder<ECommerceContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("ECommerce"));
            return new ECommerceContext(optionsBuilder.Options);
        }
    }
}
