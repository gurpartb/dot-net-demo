using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var serviceName = "RazorPagesMovie";

var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "unknown";

// Traces
var tracingExporter = builder.Configuration.GetValue<string>("UseTracingExporter").ToLowerInvariant();
var resourceBuilder = tracingExporter switch
{
    "otlp" => ResourceBuilder.CreateDefault().AddService(builder.Configuration.GetValue<string>("Otlp:ServiceName"), serviceVersion: assemblyVersion, serviceInstanceId: Environment.MachineName),
    _ => ResourceBuilder.CreateDefault().AddService(serviceName, serviceVersion: assemblyVersion, serviceInstanceId: Environment.MachineName),
};
builder.Services.AddOpenTelemetryTracing(options =>
{
    options
        .SetResourceBuilder(resourceBuilder)
        .SetSampler(new AlwaysOnSampler())
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation();

    switch (tracingExporter)
    {
        case "otlp":
            options.AddOtlpExporter(otlpOptions =>
            {
                otlpOptions.Endpoint = new Uri(builder.Configuration.GetValue<string>("Otlp:Endpoint"));
            });
            break;

        default:
            options.AddConsoleExporter();

            break;
    }
});

// Logs
builder.Logging.ClearProviders();
builder.Logging.AddOpenTelemetry(options =>
{
    options.SetResourceBuilder(resourceBuilder);
    var logExporter = builder.Configuration.GetValue<string>("UseLogExporter").ToLowerInvariant();
    switch (logExporter)
    {
        case "otlp":
            options.AddOtlpExporter(otlpOptions =>
            {
                otlpOptions.Endpoint = new Uri(builder.Configuration.GetValue<string>("Otlp:Endpoint"));
            });
            break;
        default:
            options.AddConsoleExporter();
            break;
    }
});
builder.Services.Configure<OpenTelemetryLoggerOptions>(opt =>
{
    opt.IncludeScopes = true;
    opt.ParseStateValues = true;
    opt.IncludeFormattedMessage = true;
});


// Metrics
builder.Services.AddOpenTelemetryMetrics(options =>
{
    options
        .SetResourceBuilder(resourceBuilder)
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation();
    var metricsExporter = builder.Configuration.GetValue<string>("UseMetricsExporter").ToLowerInvariant();
    switch (metricsExporter)
    {
        case "otlp":
            options.AddOtlpExporter(otlpOptions =>
            {
                otlpOptions.Endpoint = new Uri(builder.Configuration.GetValue<string>("Otlp:Endpoint"));
            });
            break;
        default:
            options.AddConsoleExporter();
            break;
    }
});

builder.Services.AddRazorPages();
builder.Services.AddDbContext<RazorPagesMovieContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("RazorPagesMovieContext")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}

if (!app.Environment.IsDevelopment())
{
    //logger.Info("develop environment, use ExceptionsHandler /Error");
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();