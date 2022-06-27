using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using RRS.Data;
using RRS.Services;
using System.Text;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<CustomerService>();

builder.Services.AddSession(options => options.IdleTimeout = TimeSpan.FromSeconds(60));

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = "JWT_OR_COOKIE";
    o.DefaultChallengeScheme = "JWT_OR_COOKIE";
})
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,

                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

                RequireExpirationTime = true,

                ClockSkew = TimeSpan.Zero
            };
        })
        .AddPolicyScheme("JWT_OR_COOKIE", null, o =>
        {
            o.ForwardDefaultSelector = c =>
            {
                string auth = c.Request.Headers[HeaderNames.Authorization];
                if (!string.IsNullOrWhiteSpace(auth) && auth.StartsWith("Bearer "))
                {
                    return JwtBearerDefaults.AuthenticationScheme;
                }

                return IdentityConstants.ApplicationScheme;
            };
        });

var app = builder.Build();

app.UseCors(configure => {
    configure.AllowAnyHeader();
    configure.AllowAnyMethod();
    configure.AllowAnyOrigin();
});

app.UseSession();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


//server/area/controller/action
app.MapControllerRoute(name: "areas", pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
//server/controller/action 
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
