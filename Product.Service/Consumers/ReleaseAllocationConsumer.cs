using System.Threading.Tasks;
using MassTransit;
using Order.Contracts;
using Product.Contracts;

namespace Product.Service.Consumers
{
	public class ReleaseAllocationConsumer : IConsumer<ReleaseAllocationCommand>
	{
		private readonly ProductService _service;

		public ReleaseAllocationConsumer(ProductService service)
		{
			_service = service;
		}

		/// <inheritdoc />
		public async Task Consume(ConsumeContext<ReleaseAllocationCommand> context)
		{
			await _service.ReleaseAllocationAsync(context.Message.AllocationId);
			await context.RespondAsync<AllocationReleased>(new {});
		}
	}
}