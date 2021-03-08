using System;

namespace Order.Contracts
{
	public interface ReleaseAllocationCommand
	{
		Guid AllocationId { get; }
	}
}