using Api.Middleware;
using Application.Commands;
using Application.FluentValidations;
using Application.Interfaces.Repositories;
using FluentValidation;
using Infrastructure.Logging;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Validations;
using Microsoft.EntityFrameworkCore;
using Serilog;

StaticLogger.EnsureInitialized();
Log.Information("Server Booting Up...");

try
{
    var builder = WebApplication.CreateBuilder(args);
    var configuration = builder.Configuration;

    builder.Host.UseSerilog((context, services, configuration) =>
    {
        configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services) 
            .Enrich.FromLogContext();
    });
    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddMemoryCache();
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<IAccountRepository, AccountRepository>();
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateAccount).Assembly));
    builder.Services.AddValidatorsFromAssemblyContaining<CreateAccountValidator>();
    builder.Services.AddValidators();
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddLogging(configure => configure.AddSerilog());

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseSerilogRequestLogging();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex) when (!ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
{
    StaticLogger.EnsureInitialized();
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    StaticLogger.EnsureInitialized();
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}
