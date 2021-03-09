using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Courier;
using Orders.Contracts;

namespace Orders.Service
{
	public class FulfillOrderConsumer : IConsumer<FulfillOrderCommand>
	{
		/// <inheritdoc />
		public Task Consume(ConsumeContext<FulfillOrderCommand> context)
		{
			var builder = new RoutingSlipBuilder(NewId.NextGuid());

			builder.AddActivity("AllocateProduct", new Uri("queue:allocate-product_execute"), new
			{
				ItemNumber = "ITEM123",
				Quantity = 10
			});
			builder.AddActivity("Payment", new Uri("queue:payment_execute"), new
			{
				context.Message.Customer,
				context.Message.CardNumber
			});

			//builder.AddVariable("OrderId", context.Message.OrderId);

			//await builder.AddSubscription(context.SourceAddress,
			//	RoutingSlipEvents.Faulted | RoutingSlipEvents.Supplemental,
			//	RoutingSlipEventContents.None, x => x.Send<OrderFulfillmentFaulted>(new { context.Message.OrderId }));

			//await builder.AddSubscription(context.SourceAddress,
			//	RoutingSlipEvents.Completed | RoutingSlipEvents.Supplemental,
			//	RoutingSlipEventContents.None, x => x.Send<OrderFulfillmentCompleted>(new { context.Message.OrderId }));

			var routingSlip = builder.Build();

			return context.Execute(routingSlip);
		}
	}
}