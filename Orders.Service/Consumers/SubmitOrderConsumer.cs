using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Orders.Contracts;

namespace Orders.Service.Consumers
{
	public class SubmitOrderConsumer :
		IConsumer<SubmitOrderCommand>
	{
		private readonly ILogger<SubmitOrderConsumer> _logger;

		public SubmitOrderConsumer(ILogger<SubmitOrderConsumer> logger)
		{
			_logger = logger;
		}

		public async Task Consume(ConsumeContext<SubmitOrderCommand> context)
		{
			await context.Publish<OrderSubmitted>(new
			{
				context.Message.OrderId
			});

			
			if (context.RequestId != null)
				await context.RespondAsync<SubmitOrderSuccessResponse>(new
				{
					InVar.Timestamp,
					context.Message.OrderId
				});
		}
	}
}