using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Orders.Contracts;

namespace Orders.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class OrderController : ControllerBase
	{
		private readonly ISendEndpointProvider _sendEndpointProvider;

		public OrderController(ISendEndpointProvider sendEndpointProvider)
		{
			_sendEndpointProvider = sendEndpointProvider;
		}

		[HttpPost("FulfillOrder")]
		public async Task FulfillOrder(Guid orderId, string customer, string cardNumber)
		{
			var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:fulfill-order"));
			await sendEndpoint.Send<FulfillOrderCommand>(new
			{
				OrderId = orderId,
				Customer = customer,
				CardNumber = cardNumber
			});
		}
	}
}