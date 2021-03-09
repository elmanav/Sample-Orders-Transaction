using System;
using Automatonymous;

namespace Orders.Service.StateMachine
{
	public class OrderState : SagaStateMachineInstance
	{
		public Guid CorrelationId { get; set; }
		public string CurrentState { get; set; }
		public string Customer { get; set; }
		public string CardNumber { get; set; }
	}
}