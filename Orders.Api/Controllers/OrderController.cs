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
		private readonly IRequestClient<SubmitOrderCommand> _clientSubmitOrder;

		public OrderController(ISendEndpointProvider sendEndpointProvider, IRequestClient<SubmitOrderCommand> clientSubmitOrder)
		{
			_sendEndpointProvider = sendEndpointProvider;
			_clientSubmitOrder = clientSubmitOrder;
		}

		[HttpPost("SubmitOrder")]
		public async Task<IActionResult> SubmitOrder(Guid orderId)
		{
			var response = await _clientSubmitOrder.GetResponse<SubmitOrderSuccessResponse>(new
			{
				OrderId = orderId
			});
			return Accepted(response);
		}

		[HttpPost("AcceptOrder")]
		public async Task<IActionResult> AcceptOrder(Guid orderId, string cardNumber)
		{
			var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:accept-order"));
			await sendEndpoint.Send<AcceptOrderCommand>(new
			{
				OrderId = orderId,
				CardNumber = cardNumber
			});
			return Accepted();
		}

		[HttpPost("FulfillOrder")]
		public async Task<IActionResult> FulfillOrder(Guid orderId, string customer, string cardNumber)
		{
			var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:fulfill-order"));
			await sendEndpoint.Send<FulfillOrderCommand>(new
			{
				OrderId = orderId,
				Customer = customer,
				CardNumber = cardNumber
			});
			return Accepted();
		}
	}
}