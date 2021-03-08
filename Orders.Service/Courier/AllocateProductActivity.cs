using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Courier;
using Order.Contracts;
using Product.Contracts;

namespace Orders.Service.Courier
{
	public class AllocateProductActivity : IActivity<AllocateProductArguments, AllocateProductLog>
	{
		private readonly IRequestClient<AllocateProductsCommand> _client;

		public AllocateProductActivity(IRequestClient<AllocateProductsCommand> client)
		{
			_client = client;
		}

		/// <inheritdoc />
		public async Task<ExecutionResult> Execute(ExecuteContext<AllocateProductArguments> context)
		{
			var response = await _client.GetResponse<ProductsAllocated>(new
			{
				ItemNumber = context.Arguments.ItemNumber,
				ItemCount = context.Arguments.Quantity
			});
			return context.Completed<AllocateProductLog>(new
			{
				response.Message.AllocationId
			});
		}

		/// <inheritdoc />
		public Task<CompensationResult> Compensate(CompensateContext<AllocateProductLog> context)
		{
			throw new NotImplementedException();
		}
	}

	public interface AllocateProductLog
	{
		Guid AllocationId { get; }
	}

	public interface AllocateProductArguments
	{
		Guid OrderId { get; }
		string ItemNumber { get; }
		int Quantity { get; }
	}
}