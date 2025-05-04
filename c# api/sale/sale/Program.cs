using ChneseSaleApi.buisness;
using ChneseSaleApi.repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using sale.buisness;
using sale.repositories;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TasksApi.Services;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<saleDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<DonatorDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// הגדרת Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug() // רמת הלוגים המינימלית שתיכתב
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) // הגדרה נפרדת לרכיבי Microsoft
    .Enrich.FromLogContext() // הוספת הקשר ללוגים
    .WriteTo.Console() // כתיבה לקונסול
    .WriteTo.File(
        path: "logs/log-.txt", // מיקום הקובץ
        rollingInterval: RollingInterval.Day, // קובץ חדש לכל יום
        retainedFileCountLimit: 7, // שמירת קבצים עד 7 ימים אחורה
        restrictedToMinimumLevel: LogEventLevel.Information // רמת לוגים מינימלית לקובץ
    )
    .CreateLogger();
builder.Host.UseSerilog();


// Register services for Dependency Injection
builder.Services.AddScoped<IDonatorRepository, DonatorRepository>();
builder.Services.AddScoped<IDonatorService, DonatorService>();

builder.Services.AddScoped<IGiftRepository, GiftRepository>();
builder.Services.AddScoped<IGiftService, GiftService>();

builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>();
builder.Services.AddScoped<IPurchaseService, PurchaseService>();

builder.Services.AddScoped<IRandomRepository, RandomRepository>();
builder.Services.AddScoped<IRandomService, RandomService>();

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddScoped<IPayPalRepository, PayPalRepository>();
builder.Services.AddScoped<IPayPalService, PayPalService>();


// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:4200") // עדכן לכתובת של אפליקציית Angular שלך
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});
// Add Swagger services to the container.
builder.Services.AddEndpointsApiExplorer(); // For exposing endpoints
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "מכירה סינית - API",
        Description = "A simple example ASP.NET Core API to manage books",
        Contact = new OpenApiContact
        {
            Name = "חני מירניק",
            Email = "c7631625@gmail.com",
            Url = new Uri("https://yourwebsite.com"),
        }
    });

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer {your_token}'"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
        };
    });

var app = builder.Build();

// Configure request pipeline
if (app.Environment.IsDevelopment())
{

    app.UseDeveloperExceptionPage(); // Detailed error page in Development
}
else
{
    app.UseExceptionHandler("/Home/Error"); // Error handling for Production
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    // Enable middleware to serve generated Swagger as a JSON    endpoint.
    app.UseSwagger();

    // Enable middleware to serve Swagger UI (HTML, JS, CSS, etc.)
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Books API V1");
        c.RoutePrefix = string.Empty; // Serve Swagger UI at the app's root
    });
}
//app.UseHttpsRedirection();
//app.UseRouting();
app.UseMiddleware<LoggingMiddleware>();
app.UseCors("AllowSpecificOrigin");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Start the app
app.Run();



