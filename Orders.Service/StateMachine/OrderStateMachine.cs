using System;
using Automatonymous;
using MassTransit.Definition;
using Microsoft.Extensions.Logging;
using Orders.Contracts;

namespace Orders.Service.StateMachine
{
	public class OrderStateMachine : MassTransitStateMachine<OrderState>
	{
		public OrderStateMachine(ILogger<OrderStateMachine> logger)
		{
			InstanceState(state => state.CurrentState);
			InitEvents();
			Initially(
				When(OrderSubmitted)
					.Then(context =>
					{
						logger.LogDebug($"{context.Event} with transit from {context.Instance.CurrentState}.");
						context.Instance.Customer = context.Data.Customer;
					})
					.TransitionTo(Submitted));
			During(Submitted,
				When(OrderAccepted)
					.Then(context =>
					{
						logger.LogDebug($"{context.Event} with transit from {context.Instance.CurrentState}.");
						context.Instance.CardNumber = context.Data.CardNumber;
					})
					.ThenAsync(async context =>
					{
						var endpoint = await context.GetSendEndpoint(new Uri($"queue:{KebabCaseEndpointNameFormatter.Instance.Consumer<FulfillOrderConsumer>()}"));
						await endpoint.Send<FulfillOrderCommand>(new
						{
							OrderId = context.Data.OrderId,
							Customer = context.Instance.Customer,
							CardNumber = context.Instance.CardNumber

						});
					})
					.TransitionTo(Accepted));
		}

		public Event<OrderSubmitted> OrderSubmitted { get; }
		public Event<OrderAccepted> OrderAccepted { get; }

		public State Submitted { get; }
		public State Accepted { get; }

		private void InitEvents()
		{
			Event(() => OrderSubmitted, @event => @event.CorrelateById(context => context.Message.OrderId));
			Event(() => OrderAccepted, @event => @event.CorrelateById(context => context.Message.OrderId));
		}
	}
}