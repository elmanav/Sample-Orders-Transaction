using System;

namespace Orders.Contracts
{
	public interface AcceptOrderCommand
	{
		Guid OrderId { get; }
		string CardNumber { get; }
	}
}