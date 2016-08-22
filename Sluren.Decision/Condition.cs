using System;

namespace Sluren.Decision {
	public abstract class Condition : IDisposable {
		protected ConditionVisitor<bool> evaluator { get; set; }
		protected ConditionVisitor<string> visualizer { get; set; }

		protected Condition() {
			evaluator = ConditionVisitorFactoryHandle.Factory.Evaluator;
			visualizer = ConditionVisitorFactoryHandle.Factory.Visualizer;
		}

		public abstract T Accept<T>(ConditionVisitor<T> visitor);
		public abstract bool Evaluate();

		//public Condition ChainTo(Condition right, Operator operatorToChainWith) {
		//	if (right == null)
		//		throw new NullReferenceException("Condition with which the current condition is to be evaluated, cannot be null");
		//	if (operatorToChainWith == null)
		//		return this;
		//	return operatorToChainWith.Evaluate(this, right);
		//}
		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool isDisposing) {
		}
	}

}
