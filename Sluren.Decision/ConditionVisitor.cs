namespace Sluren.Decision {
	public abstract class ConditionVisitor<T> {
		public abstract T Visit(BooleanCondition condition);
		public abstract T Visit(CompositeCondition condition);
	}
}
