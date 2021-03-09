using System;

namespace Orders.Contracts
{
	public interface SubmitOrderCommand
	{
		Guid OrderId { get; }
	}
}