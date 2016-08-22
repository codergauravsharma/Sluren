namespace Sluren.Decision {
	public class BooleanCondition : Condition {
		public bool Expression { get; private set; }

		public BooleanCondition(bool expression) {
			Expression = expression;
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
	}
}
