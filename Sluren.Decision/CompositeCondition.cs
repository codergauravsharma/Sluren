using System.Collections.Generic;
using System.Linq;

namespace Sluren.Decision {
	public class CompositeCondition : Condition {
		List<ConditionNode> conditionCollection;
		public IEnumerable<ConditionNode> Conditions { get { return conditionCollection.AsEnumerable(); } }
		public CompositeCondition() {
			conditionCollection = new List<ConditionNode>();
		}
		protected virtual void AddCondition(Condition condition, Operator connector) {
			conditionCollection.Add(new ConditionNode(condition, connector));
		}
		public CompositeCondition Add(Condition condition, Operator connector) {
			AddCondition(condition, connector);
			return this;
		}
		public void Remove(Condition condition) {
			conditionCollection.Remove(conditionCollection.First((c) => { return c.Condition == condition; }));
		}
		public override T Accept<T>(ConditionVisitor<T> visitor) {
			return visitor.Visit(this);
		}
		public override bool Evaluate() {
			return Accept(evaluator);
		}
		public override string ToString() {
			return Accept(visualizer);
		}
		protected override void Dispose(bool isDisposing) {
			if (isDisposing)
				if (conditionCollection != null)
					conditionCollection.Clear();
		}
	}
}
