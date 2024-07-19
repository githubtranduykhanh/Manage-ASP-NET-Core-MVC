using ECommerceMVC.Infrastructure.Configuration;
using ECommerceMVC.UI.Areas.Admin.AutoMapperProfile.Caterory;
using ECommerceMVC.UI.Areas.Admin.AutoMapperProfile.Group;
using ECommerceMVC.UI.Areas.Admin.AutoMapperProfile.Product;


var builder = WebApplication.CreateBuilder(args);


builder.Configuration
    //.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) // Đọc từ appsettings.json
    .AddJsonFile("appsettingsLocal.json", optional: true, reloadOnChange: true) // Đọc từ appsettingsLoca.json
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true) // Đọc từ appsettings.{EnvironmentName}.json
    .AddEnvironmentVariables(); // Đưa các biến môi trường vào cấu hình


// Add services to the container.
builder.Services.AddControllersWithViews().AddSessionStateTempDataProvider();
var mvcBuilder = builder.Services.AddRazorPages();
builder.Services.AddSession();


//Đăng ký chuổi kết nối
builder.Services.RegisterDb(builder.Configuration);

//Đăng ký Identity
builder.Services.RegisterIdentity(builder.Configuration);


//Đăng ký Cors
builder.Services.RegisterCors(builder.Configuration);

//Đăng ký Notyf
builder.Services.RegisterNotyf();


//Đăng ký Configure Option
builder.Services.AddConfigureOption(builder.Configuration);



//Đăng ký Dependency Injection
builder.Services.AddDependencyInjection();


//AddAutoMapper
builder.Services.AddAutoMapper(typeof(CateroryCreateProfile));

builder.Services.AddAutoMapper(typeof(GroupCreateProfile));

builder.Services.AddAutoMapper(typeof(ProductCreateProfile));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    mvcBuilder.AddRazorRuntimeCompilation();
}

app.UseHttpsRedirection();

app.UseSession(); // Kích hoạt middleware Session

app.UseStaticFiles();

app.UseRouting();



app.UseCors("CorsPolicy");


app.UseAuthentication();
app.UseAuthorization();

app.MapAreaControllerRoute(
    name: "MyAreaAdmin",
    areaName: "Admin",
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
);


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.MapRazorPages();


app.Run();


