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
		private readonly IRequestClient<AllocateProductsCommand> _clientAllocate;
		private readonly IRequestClient<ReleaseAllocationCommand> _clientRelease;

		public AllocateProductActivity(IRequestClient<AllocateProductsCommand> clientAllocate, IRequestClient<ReleaseAllocationCommand> clientRelease)
		{
			_clientAllocate = clientAllocate;
			_clientRelease = clientRelease;
		}

		/// <inheritdoc />
		public async Task<ExecutionResult> Execute(ExecuteContext<AllocateProductArguments> context)
		{
			var response = await _clientAllocate.GetResponse<ProductsAllocated>(new
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
		public async Task<CompensationResult> Compensate(CompensateContext<AllocateProductLog> context)
		{
			await _clientRelease.GetResponse<AllocationReleased>(new
			{
				context.Log.AllocationId
			});
			return context.Compensated();
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