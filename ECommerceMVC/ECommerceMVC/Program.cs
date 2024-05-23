using ECommerceMVC.AutoMapperProfile;
using ECommerceMVC.Config;
using ECommerceMVC.Data;
using ECommerceMVC.Helper.Excel;
using ECommerceMVC.Helper.Jwts;
using ECommerceMVC.Helper.Responses;
using ECommerceMVC.Repositorys.User;
using ECommerceMVC.Services.User;
using ECommerceMVC.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
builder.Services.AddScoped<IRepositoryUser, RepositoryUser>();
builder.Services.AddScoped<IServiceUser<DbUser, UserVM>, ServiceUser>();
builder.Services.AddScoped<IExcel<DbUser>, UserExcel>();
builder.Services.AddScoped<JwtAuthenticationManager>();

builder.Services.AddAutoMapper(typeof(UserRegisterProfile));

// Đọc giá trị CORS từ cấu hình
var corsSettings = builder.Configuration.GetSection("CorsSettings").Get<CorsSettings>();
// Đọc giá trị Jwt từ cấu hình
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

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


// Cấu hình JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer( options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,    
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,  
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = jwtSettings?.Issuer,
        ValidAudience = jwtSettings?.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings?.SecretKey ?? "ECommerceAuthorizationInformationWarningGETPOSTPUTPUT"))
    };

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
});



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


app.MapAreaControllerRoute(
    name: "MyAreaAccount",
    areaName: "Account",
    pattern: "Account/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);



app.Run();
