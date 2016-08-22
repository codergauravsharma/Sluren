using System;
using System.Linq;

namespace Sluren.Rule {
	public class RuleXMLFormatVisualizer : RuleVisitor<string> {
		public override string Visit(HardValueBasedRule rule) {
			return string.Format(
				"<HardValueBasedRule type=\"{0}\" leftvalue=\"{1}\" comparator=\"{2}\" rightvalue=\"{3}\" />"
				, rule.HardType, rule.Left, rule.Comparator, rule.Right);
		}
		public override string Visit(CompositeRule rule) {
			string enclosure = "<CompositeRule>{0}</CompositeRule>", incrementalResult = "<{0} /> {1}", result = "";

			Rule leftRule, rightRule;

			leftRule = rightRule = rule.rules.Count() > 0 ? rule.rules.ElementAt(0).Rule : null;

			result = leftRule != null ? leftRule.Accept(this) : String.Empty;

			if (leftRule != null) {
				for (int ruleIndex = 1; ruleIndex < rule.rules.Count(); ++ruleIndex) {
					rightRule = rule.rules.ElementAt(ruleIndex).Rule;
					string connector = rule.rules.ElementAt(ruleIndex - 1).Connector.ToString();
					result += string.Format(incrementalResult, connector, rightRule.Accept(this));
					leftRule = rightRule;
				}
			}
			return string.Format(enclosure, result);
		}
		public override string Visit(PropertyFieldBasedRule rule) {
			throw new NotImplementedException();
		}
		public override string Visit(TypeBasedRule rule) {
			throw new NotImplementedException();
		}

		public override string Visit(PropertyFieldToHardValueRule rule) {
			throw new NotImplementedException();
		}
	}
}
