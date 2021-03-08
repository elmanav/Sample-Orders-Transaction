using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Product.Service
{
	public class ProductService
	{
		private readonly ILogger<ProductService> _logger;

		public ProductService(ILogger<ProductService> logger)
		{
			_logger = logger;
		}

		public async Task<Guid> AllocateProductAsync(string itemNumber, int itemCount)
		{
			_logger.LogInformation($"Allocate product for order. Item number: {itemNumber}; item count: {itemCount}");
			await Task.Delay(2000);
			return Guid.NewGuid();
		}

		public Task ReleaseAllocationAsync(Guid allocationId)
		{
			_logger.LogInformation($"Cancel allocation {allocationId} product for order.");
			return Task.Delay(2000);
		}
	}
}