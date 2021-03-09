using System.Threading.Tasks;
using MassTransit;
using MassTransit.Courier;
using Order.Contracts;
using Payments.Contracts;
using Product.Contracts;

namespace Orders.Service.Courier
{
	public class PaymentActivity : IActivity<PaymentArguments, PaymentLog>
	{
		private readonly IRequestClient<PayOrder> _client;

		public PaymentActivity(IRequestClient<PayOrder> client)
		{
			_client = client;
		}

		public async Task<ExecutionResult> Execute(ExecuteContext<PaymentArguments> context)
		{
			var response = await _client.GetResponse<OrderPaid>(new
			{
				Customer = context.Arguments.Customer,
				CardNumber = context.Arguments.CardNumber
			});
			return context.Completed<PaymentLog>(new
			{
				response.Message.PaymentId
			});
		}

		public async Task<CompensationResult> Compensate(CompensateContext<PaymentLog> context)
		{
			await Task.Delay(2000);
			return context.Compensated();
		}
	}

	public interface PaymentLog
	{
	}

	public interface PaymentArguments
	{
		string Customer { get; }
		string CardNumber { get; }
	}
}