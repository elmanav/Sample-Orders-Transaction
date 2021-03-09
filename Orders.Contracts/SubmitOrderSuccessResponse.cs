using System;

namespace Orders.Contracts
{
	public interface SubmitOrderSuccessResponse
	{
		DateTime Timestamp { get; }
		Guid OrderId { get; }
	}
}