using System;

namespace Orders.Contracts
{
	public interface OrderSubmitted
	{
		Guid OrderId { get; }
		string Customer { get; }
	}
}