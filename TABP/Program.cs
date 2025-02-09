using Application;
using Infrastructure;
using Serilog;
using TABP;

var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
builder.Services
    .AddPresentation()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);


var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();

app.Run();