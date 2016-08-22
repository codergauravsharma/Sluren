namespace Sluren.Decision {
	public abstract class Operator {
		public abstract Condition Evaluate(Condition left, Condition right);
		public override string ToString() {
			return this.GetType().Name;
		}
	}

	public class And : Operator {
		public override Condition Evaluate(Condition left, Condition right) {
			return new BooleanCondition(left.Evaluate() && right.Evaluate());
		}
	}

	public class Or : Operator {
		public override Condition Evaluate(Condition left, Condition right) {
			return new BooleanCondition(left.Evaluate() || right.Evaluate());
		}
	}
}
