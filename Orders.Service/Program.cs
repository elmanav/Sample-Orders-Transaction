using System.Threading.Tasks;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Order.Contracts;
using Orders.Service.Courier;
using Serilog;
using Serilog.Events;

namespace Orders.Service
{
	internal class Program
	{
		private static async Task Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
				.Enrich.FromLogContext()
				.WriteTo.Console()
				.CreateLogger();

			var builder = new HostBuilder()
				.ConfigureAppConfiguration((hostingContext, config) =>
				{
					config.AddJsonFile("appsettings.json", true);
					config.AddEnvironmentVariables();

					if (args != null)
						config.AddCommandLine(args);
				})
				.ConfigureServices((hostContext, services) =>
				{
					services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
					services.AddMassTransit(cfg =>
					{
						cfg.AddConsumersFromNamespaceContaining<FulfillOrderConsumer>();
						cfg.AddActivitiesFromNamespaceContaining<AllocateProductActivity>();

						cfg.UsingRabbitMq((context, configurator) =>
						{
							var rabbitMqConfig = hostContext.Configuration.GetSection("RabbitMq");
							configurator.Host(rabbitMqConfig["HostAddress"], hostConfigurator =>
							{
								hostConfigurator.Username(rabbitMqConfig["Username"]);
								hostConfigurator.Password(rabbitMqConfig["Password"]);
							});
							configurator.ConfigureEndpoints(context);
						});
						cfg.AddRequestClient<AllocateProductsCommand>();
					});

					services.AddHostedService<MassTransitHostedService>();
				})
				.ConfigureLogging((hostingContext, logging) =>
				{
					logging.AddSerilog(dispose: true);
					logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
				});

			await builder.RunConsoleAsync();

			Log.CloseAndFlush();
		}
	}
}