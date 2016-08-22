using Sluren.Decision;

namespace Sluren.Rule {
	public class RuleParsingResult {
		public Condition Condition { get; private set; }
		public object Left { get; private set; }
		public object Right { get; private set; }
		public object AdditionalData { get; private set; }
		public RuleParsingResult(Condition condition, object left, object right, object additionalData) {
			Condition = condition;
			Left = left;
			Right = right;
			AdditionalData = additionalData;
		}
	}
}
