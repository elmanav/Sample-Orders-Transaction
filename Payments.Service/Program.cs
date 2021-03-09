using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Payments.Service.Consumers;
using Serilog;
using Serilog.Events;

namespace Payments.Service
{
	class Program
	{
		static async Task Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
				.Enrich.FromLogContext()
				.WriteTo.Console()
				.CreateLogger();

			var builder = new HostBuilder()
				.ConfigureAppConfiguration((hostingContext, config) => { config.AddJsonFile("appsettings.json", true); })
				.ConfigureServices((hostContext, services) =>
				{
					services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
					services.AddMassTransit(cfg =>
					{
						cfg.AddConsumersFromNamespaceContaining<PayOrderConsumer>();
						cfg.UsingRabbitMq((context, configurator) =>
						{
							var rabbitMqConfig = hostContext.Configuration.GetSection($"RabbitMq");
							configurator.Host(rabbitMqConfig["HostAddress"], hostConfigurator =>
							{
								hostConfigurator.Username(rabbitMqConfig["Username"]);
								hostConfigurator.Password(rabbitMqConfig["Password"]);
							});
							configurator.ConfigureEndpoints(context);
						});
					});

					services.AddHostedService<MassTransitHostedService>();
				})
				.ConfigureLogging((hostingContext, logging) =>
				{
					logging.AddSerilog(dispose: true);
					logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
				});

			await builder.RunConsoleAsync();
		}
	}
}
