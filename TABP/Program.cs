using Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddControllers();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler("/error");
app.UseSwagger();
app.UseSwaggerUI();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
//app.AddMigrate();
app.UseAuthentication();
app.UseAuthorization();
//app.UseRateLimiter();
app.MapControllers();

app.Run();
