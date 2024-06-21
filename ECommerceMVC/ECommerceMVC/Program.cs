using ECommerceMVC.AutoMapperProfile;
using ECommerceMVC.AutoMapperProfile.User;
using ECommerceMVC.Config;
using ECommerceMVC.Data;
using ECommerceMVC.Helper.Email;
using ECommerceMVC.Helper.Excel;
using ECommerceMVC.Helper.Jwts;
using ECommerceMVC.Helper.Responses;
using ECommerceMVC.Repositorys.User;
using ECommerceMVC.Services.Store;
using ECommerceMVC.Services.User;
using ECommerceMVC.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


//Đăng ký chuổi kết nối
builder.Services.AddDbContext<ECommerceContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("E_Commerce"));
});
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Add services to the container.
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddTransient<IEmailSender, EmailSender>();

//builder.Services.AddScoped<IRepositoryUser, RepositoryUser>();
//builder.Services.AddScoped<IServiceUser<DbUser, UserVM>, ServiceUser>();
//builder.Services.AddScoped<IServiceUser<DbUser, UserFirstVM>, ServiceUserFirst>();
builder.Services.AddSingleton<IServiceStore, ServiceStore>();
builder.Services.AddScoped<IExcel<DbUser>, UserExcel>();

builder.Services.AddScoped<JwtAuthenticationManager>();

//AddAutoMapper
builder.Services.AddAutoMapper(typeof(UserRegisterProfile));

builder.Services.AddAutoMapper(typeof(UserVMDbUserProfile));

builder.Services.AddAutoMapper(typeof(UserInformationcClient));

// Đọc giá trị CORS từ cấu hình
var corsSettings = builder.Configuration.GetSection("CorsSettings").Get<CorsSettings>();
// Đọc giá trị Jwt từ cấu hình
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();


//Cấu hình Cors
builder.Services.AddCors(options =>
{
    //Cấu hình cho req tùy trỉnh
    //options.AddPolicy("CorsPolicy",
    //        builderCors => builderCors
    //            .WithOrigins(corsSettings.AllowedOrigins.ToArray())
    //            .WithMethods(corsSettings.AllowedMethods.ToArray())
    //            .WithHeaders(corsSettings.AllowedHeaders.ToArray())
    //);



    //Cấu hình cho bất kỳ req nào 
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins(corsSettings.AllowedOrigins.ToArray())
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

builder.Services.AddRazorPages();

//builder.Services.AddDefaultIdentity<DbUser>()
//    .AddEntityFrameworkStores<ECommerceContext>()
//    .AddDefaultTokenProviders();


builder.Services.AddDefaultIdentity<DbUser>()
    .AddEntityFrameworkStores<ECommerceContext>()
    .AddDefaultTokenProviders();

//Truy cập IdentityOptions
builder.Services.Configure<IdentityOptions>(options => {
    // Thiết lập về Password
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

    // Cấu hình Lockout - khóa user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
    options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lầ thì khóa
    options.Lockout.AllowedForNewUsers = true;

    // Cấu hình về User.
    options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;  // Email là duy nhất

    // Cấu hình đăng nhập.
    options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
    options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
    options.SignIn.RequireConfirmedAccount = true;  // Email phải được xác thực trước khi đăng nhặp
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

builder.Services.AddControllersWithViews();







// Cấu hình JWT authentication
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer( options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,    
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,  
//        ClockSkew = TimeSpan.Zero,
//        ValidIssuer = jwtSettings?.Issuer,
//        ValidAudience = jwtSettings?.Audience,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings?.SecretKey ?? "ECommerceAuthorizationInformationWarningGETPOSTPUTPUT"))
//    };

    //options.Events = new JwtBearerEvents
    //{
    //    OnAuthenticationFailed = context =>
    //    {
    //        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
    //        {
    //            if (!context.Response.HasStarted)
    //            {
    //                context.Response.WriteJsonResponseAsync(
    //                  new AuthenticationFailedResponse()
    //                  {
    //                      mes = "Token expired"
    //                  },
    //                  StatusCodes.Status401Unauthorized,
    //                  "Token expired"
    //                ).Wait();
    //            }
    //        }
    //        else if (context.Exception.GetType() == typeof(SecurityTokenInvalidSignatureException))
    //        {
    //            if (!context.Response.HasStarted)
    //            {
    //                // Token has invalid signature
    //                context.Response.WriteJsonResponseAsync(
    //                  new AuthenticationFailedResponse()
    //                  {
    //                      mes = "Token has invalid signature"
    //                  },
    //                  StatusCodes.Status401Unauthorized,
    //                  "Token has invalid signature"
    //                ).Wait();
    //            }               
    //        }
    //        else
    //        {
    //            if (!context.Response.HasStarted)
    //            {
    //                // Token invalid
    //                context.Response.WriteJsonResponseAsync(
    //                   new AuthenticationFailedResponse()
    //                   {
    //                       mes = "Token invalid"
    //                   },
    //                   StatusCodes.Status401Unauthorized, "Token invalid"
    //                ).Wait();
    //            }
                
    //        }
    //        return Task.CompletedTask;
    //    },
    //    OnTokenValidated = context =>
    //    {
    //        Console.WriteLine("Token validated successfully");
    //        return Task.CompletedTask;
    //    },
    //    OnChallenge = context =>
    //    {
    //        context.HandleResponse();
    //        return Task.CompletedTask;
    //    }
    //};
//});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{ 
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();



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


app.MapAreaControllerRoute(
    name: "MyAreaMember",
    areaName: "Member",
    pattern: "Member/{controller=Home}/{action=Index}/{id?}"
);


//app.MapAreaControllerRoute(
//    name: "MyAreaAccount",
//    areaName: "Account",
//    pattern: "Account/{controller=Home}/{action=Index}/{id?}"
//);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.MapRazorPages();


app.Run();
