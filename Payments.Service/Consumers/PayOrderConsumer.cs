using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Payments.Contracts;

namespace Payments.Service.Consumers
{
	public class PayOrderConsumer : IConsumer<PayOrder>
	{
		private readonly ILogger<PayOrderConsumer> _logger;

		public PayOrderConsumer(ILogger<PayOrderConsumer> logger)
		{
			_logger = logger;
		}

		public async Task Consume(ConsumeContext<PayOrder> context)
		{
			string cardNumber = context.Message.CardNumber;
			if (string.IsNullOrEmpty(cardNumber))
				throw new ArgumentNullException(nameof(cardNumber));

			await Task.Delay(1000);
			if (cardNumber.StartsWith("7777"))
			{
				throw new InvalidOperationException("The card number was invalid");
			}

			var paymentId = Guid.NewGuid();

			_logger.LogInformation($"Payment was success. Payment id = {paymentId}");

			// бросить событие об оплате


			await context.RespondAsync<OrderPaid>(new
			{
				PaymentId = paymentId
			});
		}
	}
}