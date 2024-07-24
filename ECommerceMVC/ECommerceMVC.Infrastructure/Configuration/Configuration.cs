
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication;
using AspNetCoreHero.ToastNotification;
using Microsoft.AspNetCore.Identity.UI.Services;
using ECommerceMVC.DataAccess.Data;
using ECommerceMVC.Domain.Entities;
using ECommerceMVC.DataAccess.Repositorys;
using ECommerceMVC.Domain.Abstract;
using ECommerceMVC.Infrastructure.Config;
using ECommerceMVC.Application.Mappings;
using ECommerceMVC.Infrastructure.Services.Email;
using ECommerceMVC.Infrastructure.Services.Cloudinary;
using ECommerceMVC.Application.Interfaces;
using ECommerceMVC.Application.Services.NewCategories;
using ECommerceMVC.Domain.Abstract.Cloudinary;
using ECommerceMVC.Application.Services.Product;

namespace ECommerceMVC.Infrastructure.Configuration
{
    public static class Configuration
    {
        public static void RegisterDb(this IServiceCollection services , IConfiguration configuration)
        {
            //Đăng ký chuổi kết nối
            services.AddDbContext<ECommerceContext>(options => options.UseSqlServer(configuration.GetConnectionString("ECommerce")));         
        }


        public static void RegisterIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            // Đọc giá trị AuthenticationSettings từ cấu hình
            var authenticationSettings = configuration.GetSection("AuthenticationSettings").Get<AuthenticationSettings>();
            //Cấu hình Identity
            services.AddIdentity<DbUser, IdentityRole>()
                .AddEntityFrameworkStores<ECommerceContext>()
                .AddDefaultTokenProviders();

            //Truy cập IdentityOptions
            services.Configure<IdentityOptions>(options => {
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


            //Khai báo path
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });


            //Add 
            services.AddAuthentication()
                .AddGoogle(googleOptions =>
                {
                    // Thiết lập ClientID và ClientSecret để truy cập API google
                    googleOptions.ClientId = authenticationSettings.Google.ClientId;
                    googleOptions.ClientSecret = authenticationSettings.Google.ClientSecret;
                    // Cấu hình Url callback lại từ Google (không thiết lập thì mặc định là /signin-google)
                    googleOptions.CallbackPath = authenticationSettings.Google.CallbackPath;
                    googleOptions.Scope.Add("profile");
                    googleOptions.ClaimActions.MapJsonKey("urn:google:picture", "picture");
                    googleOptions.Events = new OAuthEvents()
                    {
                        OnRedirectToAuthorizationEndpoint = c =>
                        {
                            c.RedirectUri += "&prompt=consent";
                            c.Response.Redirect(c.RedirectUri);
                            return Task.CompletedTask;
                        }
                    };
                })
                .AddFacebook(facebookOptions => {
                    facebookOptions.AppId = authenticationSettings.Facebook.AppId;
                    facebookOptions.AppSecret = authenticationSettings.Facebook.AppSecret;
                    // Thiết lập đường dẫn Facebook chuyển hướng đến
                    facebookOptions.CallbackPath = authenticationSettings.Facebook.CallbackPath;
                    facebookOptions.SaveTokens = true;
                });
        }



        public static void RegisterCors(this IServiceCollection services, IConfiguration configuration)
        {
            // Đọc giá trị CORS từ cấu hình
            var corsSettings = configuration.GetSection("CorsSettings").Get<CorsSettings>();

            //Cấu hình Cors
            services.AddCors(options =>
            {
                //Cấu hình cho bất kỳ req nào 
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.WithOrigins(corsSettings.AllowedOrigins.ToArray())
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
            });
        }


        public static void RegisterNotyf (this IServiceCollection services){

            //Config Notyf
            services.AddNotyf(config =>
            {
                config.DurationInSeconds = 4;
                config.IsDismissable = true;
                config.Position = NotyfPosition.TopRight;
            });

        }


        public static void AddConfigureOption(this IServiceCollection services, IConfiguration configuration)
        {
            // Add services to the container.
            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));
            services.Configure<AuthenticationSettings>(configuration.GetSection("AuthenticationSettings"));
        }

        public static void AddDependencyInjection(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Đăng ký NewCategoriesService sau khi IUnitOfWork đã được đăng ký
            services.AddScoped<INewCategoriesService, NewCategoriesService>();

            // Đăng ký OrderService sau khi IUnitOfWork đã được đăng ký
            services.AddScoped<IOrderService, OrderService>();

            services.AddTransient<IEmailSender, EmailSender>();   
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            //services.AddScoped<JwtAuthenticationManager>();
        }
    }
}
