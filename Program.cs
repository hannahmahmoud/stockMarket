using backend.Data;
using backend.Endpoints;
using backend.Repository;
using backend.Service;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    
});
builder.Services.AddScoped<StockService>();
builder.Services.AddScoped<CommentService>();
builder.Services.AddScoped<ICommentRepo, CommentRepo>();

builder.Services.AddScoped<StockRepo>();

builder.Services.AddControllers();

var app = builder.Build();
app.UseExceptionHandler(options =>
{
    options.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        var exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        if (exceptionFeature is not null)
        {
            var error = new { message = exceptionFeature.Error.Message };
            await context.Response.WriteAsJsonAsync(error);
        }
    });
}); 



if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();
app.MapCommentEndpoints();
// app.UseExceptionHandler(options =>
// {
//     options.Run(async context =>
//     {
//         context.Response.StatusCode = StatusCodes.Status500InternalServerError;
//         context.Response.ContentType = "application/json";

//         var exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();
//         if (exceptionFeature is not null)
//         {
//             var error = new { message = exceptionFeature.Error.Message };
//             await context.Response.WriteAsJsonAsync(error);
//         }
//     });
// }); 


app.Run();


