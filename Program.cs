using aspDotNetCore.Data;
using aspDotNetCore.Filters;
using aspDotNetCore.Middlewares;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<LogActivityFilter>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(cfg => cfg.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ProfilingMiddleware>();
app.UseMiddleware<RateLimitingMiddleware>();

app.MapControllers();

app.Run();
