using System.Threading.Tasks;
using MassTransit;
using Orders.Contracts;

namespace Orders.Service.Consumers
{
	public class AcceptOrderConsumer : IConsumer<AcceptOrderCommand>
	{
		public Task Consume(ConsumeContext<AcceptOrderCommand> context)
		{
			// accept with data process
			return context.Publish<OrderAccepted>(new
			{
				context.Message.OrderId,
				context.Message.CardNumber
			});
		}
	}
}