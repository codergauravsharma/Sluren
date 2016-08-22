namespace Sluren.Rule {
	public class HardValueBasedRule : Rule {
		public string HardType { get; private set; }
		public string Left { get; private set; }
		public string Comparator { get; private set; }
		public string Right { get; private set; }
		public HardValueBasedRule(string hardType, string left, string comparator, string right) {
			HardType = hardType; Left = left; Comparator = comparator; Right = right;
		}
		public override T Accept<T>(RuleVisitor<T> visitor) {
			return visitor.Visit(this);
		}
	}
}
