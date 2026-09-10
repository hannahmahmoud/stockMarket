using backend.Authorization;
using backend.Data;
using backend.Endpoints;
using backend.Repository;
using backend.Service;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System.Text;

var builder = WebApplication.CreateBuilder(args);


// ========================================
// OpenAPI
// ========================================

builder.Services.AddOpenApi();


// ========================================
// Database
// ========================================

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration
            .GetConnectionString("DefaultConnection")
    );
});


// ========================================
// Repositories
// ========================================

builder.Services.AddScoped<StockRepo>();

builder.Services.AddScoped<ICommentRepo, CommentRepo>();

builder.Services.AddScoped<IUserRepo, UserRepo>();

builder.Services.AddScoped<IRoleRepo, RoleRepo>();

builder.Services.AddScoped<
    IPermissionRepo,
    PermissionRepo
>();


// ========================================
// Services
// ========================================

builder.Services.AddScoped<StockService>();
builder.Services.AddScoped<CommentService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<PermissionService>();


// ========================================
// Authentication - JWT
// ========================================

builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme
)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuer = true,

            ValidateAudience = true,

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,

            ValidIssuer =
                builder.Configuration["Jwt:Issuer"],

            ValidAudience =
                builder.Configuration["Jwt:Audience"],

            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        builder.Configuration["Jwt:Key"]!
                    )
                )
        };
});


// ========================================
// Authorization
// ========================================

builder.Services.AddSingleton<
    IAuthorizationHandler,
    PermissionAuthorizationHandler
>();

builder.Services.AddAuthorization(options =>
{
    // Create Stock
    options.AddPolicy(
        "CreateStock",
        policy =>
        {
            policy.Requirements.Add(
                new PermissionRequirement(
                    "CreateStock"
                )
            );
        }
    );


    // Update Stock
    options.AddPolicy(
        "UpdateStock",
        policy =>
        {
            policy.Requirements.Add(
                new PermissionRequirement(
                    "UpdateStock"
                )
            );
        }
    );


    // Delete Stock
    options.AddPolicy(
        "DeleteStock",
        policy =>
        {
            policy.Requirements.Add(
                new PermissionRequirement(
                    "DeleteStock"
                )
            );
        }
    );


    // Manage Users
    options.AddPolicy(
        "ManageUsers",
        policy =>
        {
            policy.Requirements.Add(
                new PermissionRequirement(
                    "ManageUsers"
                )
            );
        }
    );
});


// ========================================
// Controllers
// ========================================

builder.Services.AddControllers();


var app = builder.Build();


// ========================================
// Global Exception Handler
// ========================================

app.UseExceptionHandler(options =>
{
    options.Run(async context =>
    {
        context.Response.StatusCode =
            StatusCodes.Status500InternalServerError;

        context.Response.ContentType =
            "application/json";

        var exceptionFeature =
            context.Features
                .Get<IExceptionHandlerPathFeature>();

        if (exceptionFeature is not null)
        {
            var error = new
            {
                message =
                    exceptionFeature.Error.Message
            };

            await context.Response
                .WriteAsJsonAsync(error);
        }
    });
});


// ========================================
// OpenAPI
// ========================================

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


// ========================================
// Middleware
// ========================================

app.UseHttpsRedirection();


// Authentication must come BEFORE Authorization
app.UseAuthentication();

app.UseAuthorization();


// ========================================
// Controllers
// ========================================

app.MapControllers();


// ========================================
// Minimal API endpoints
// ========================================

app.MapCommentEndpoints();


app.Run();