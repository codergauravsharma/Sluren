namespace Sluren.Rule {
	public class TypeBasedRule : Rule {
		public string Left { get; private set; }
		public string Comparator { get; private set; }
		public string Right { get; private set; }
		public TypeBasedRule(string left, string comparator, string right) {
			Left = left; Comparator = comparator; Right = right;
		}
		public override T Accept<T>(RuleVisitor<T> visitor) {
			return visitor.Visit(this);
		}
	}
}
