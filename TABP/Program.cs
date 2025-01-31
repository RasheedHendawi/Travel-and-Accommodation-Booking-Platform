using Application;
using Domain.Interfaces;
using Infrastructure;
using Serilog;
using TABP.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IHttpUserContextAccessor, HttpUserContextAccessor>();


builder.Services.AddSwaggerGen();
var app = builder.Build();

app.UseExceptionHandler("/error");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
//app.UseRateLimiter();
app.MapControllers();

app.Run();
