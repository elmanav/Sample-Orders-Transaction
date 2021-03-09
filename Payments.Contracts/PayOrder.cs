namespace Payments.Contracts
{
	public interface PayOrder
	{
		string Customer { get; }
		string CardNumber { get; }
	}
}