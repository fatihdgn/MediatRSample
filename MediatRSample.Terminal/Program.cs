using MediatR;
using MediatRSample.Terminal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((context, services) => {
    services.Configure<TickerOptions>(context.Configuration.GetSection(TickerOptions.Name));
    services.AddMediatR(typeof(Program));
    services.AddHostedService<TickerHostedService>();
});

await builder.RunConsoleAsync();