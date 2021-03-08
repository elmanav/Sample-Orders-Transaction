using System;

namespace Orders.Contracts
{
	/// <summary>
	/// выполняем заказ
	/// </summary>
	public interface FulfillOrderCommand
	{
		Guid OrderId { get; }
	}
}