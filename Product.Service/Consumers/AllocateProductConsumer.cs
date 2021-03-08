using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Order.Contracts;
using Product.Contracts;

namespace Product.Service.Consumers
{
	public class AllocateProductConsumer : IConsumer<AllocateProductsCommand>
	{
		private readonly ProductService _service;
		private readonly ILogger<AllocateProductConsumer> _logger;

		public AllocateProductConsumer(ProductService service, ILogger<AllocateProductConsumer> logger)
		{
			_service = service;
			_logger = logger;
		}

		/// <inheritdoc />
		public async Task Consume(ConsumeContext<AllocateProductsCommand> context)
		{
			var allocationId = await _service.AllocateProductAsync(context.Message.ItemNumber, context.Message.ItemCount);
			_logger.LogInformation($"Product allocated with id={allocationId}");
			await context.RespondAsync<ProductsAllocated>(new
			{
				AllocationId = allocationId
			});
		}
	}
}