using System;

namespace Orders.Contracts
{
	public interface OrderAccepted
	{
		public Guid OrderId { get; }
		string CardNumber { get; }
	}
}