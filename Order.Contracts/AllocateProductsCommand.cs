using System;

namespace Order.Contracts
{
	public interface AllocateProductsCommand
	{
		string ItemNumber { get; }
		int ItemCount { get; }
	}
}
