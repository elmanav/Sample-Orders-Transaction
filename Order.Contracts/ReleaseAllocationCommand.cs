using System;

namespace Product.Contracts
{
	public interface ReleaseAllocationCommand
	{
		Guid AllocationId { get; }
	}
}