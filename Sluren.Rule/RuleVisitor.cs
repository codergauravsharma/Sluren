namespace Sluren.Rule {
	public abstract class RuleVisitor<T> {
		public delegate void RuleProcessedEventHandler(Rule rule, T output);
		public event RuleProcessedEventHandler RuleProcessed;
		protected void OnRuleProcessed(Rule rule, T output) {
			RuleProcessed(rule, output);
		}
		public abstract T Visit(HardValueBasedRule rule);
		public abstract T Visit(PropertyFieldBasedRule rule);
		public abstract T Visit(PropertyFieldToHardValueRule rule);
		public abstract T Visit(TypeBasedRule rule);
		public abstract T Visit(CompositeRule rule);
	}
}
