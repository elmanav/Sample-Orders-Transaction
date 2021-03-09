using System;

namespace Payments.Contracts
{
	public interface OrderPaid
	{
		Guid PaymentId { get; }
	}
}