using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OfferManagementApi.Data;
using OfferManagementApi.Data.Entities;
using OfferManagementApi.Models;
using OfferManagementApi.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;
var AllowSpecificOrigin = configuration.GetSection("AllowSpecificOrigin");
var Origins = AllowSpecificOrigin["Origins"]!;

// 🔹 1. Register MainDBContext (used by Identity)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext")));

builder.Services.AddDbContext<MainDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MainDBContext")));

// Identity Configuration
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// 🔹 2. Register Services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IFrequencyService, FrequencyService>();
builder.Services.AddScoped<IListOfValueService, ListOfValueService>();
builder.Services.AddScoped<IInquiryService, InquiryService>();

builder.Services.AddScoped<IBaseService<BrandDto, Brand>, BrandService>();
builder.Services.AddScoped<IBaseService<CustomerDto, Customer>, CustomerService>();
builder.Services.AddScoped<IBaseService<FrequencyDto, Frequency>, FrequencyService>();
builder.Services.AddScoped<IBaseService<ListOfValueDto, ListOfValue>, ListOfValueService>();


// 🔹 3. Add AutoMapper configuration
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


// 🔹 3. JWT Authentication setup
// 🔹 JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? "YourSuperSecretKeyThatIsLongEnough"; // key must be > 256 bits
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
    };
});

// 🔹 4. Add Services
builder.Services.AddAuthorization();
builder.Services.AddMvc();
builder.Services.AddMvc(option => option.EnableEndpointRouting = false);
builder.Services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

builder.Services.AddCors(p => p.AddPolicy("AllowSpecificOrigin", builder =>
{
    builder.WithOrigins(Origins.Split(',').ToArray()).AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Offer Management API", Version = "v1" });

    // Add JWT Auth definition
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token like this: Bearer <your-token>"
    });

    // Apply JWT auth to all endpoints
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
var app = builder.Build();

// 🔹 5. Middleware
    app.UseSwagger();
    app.UseSwaggerUI();

app.UseCors("AllowSpecificOrigin");
app.UseHttpsRedirection();
app.UseAuthentication(); // Make sure this is before UseAuthorization
app.UseAuthorization();

app.MapControllers();
app.Run();
