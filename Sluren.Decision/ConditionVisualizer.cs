using System.Linq;

namespace Sluren.Decision {
	public class ConditionVisualizer : ConditionVisitor<string> {
		public override string Visit(BooleanCondition condition) {
			return condition.Expression.ToString();
		}

		public override string Visit(CompositeCondition condition) {
			string enclosure = "({0})", incrementalResult = " {0} {1}", result = "";

			Condition nextCondition = condition.Conditions.Count() > 0 ? condition.Conditions.ElementAt(0).Condition : null;

			if (nextCondition != null) {
				result = nextCondition.ToString();

				for (int conditionIndex = 1; conditionIndex < condition.Conditions.Count(); ++conditionIndex) {
					nextCondition = condition.Conditions.ElementAt(conditionIndex).Condition;
					string connector = condition.Conditions.ElementAt(conditionIndex - 1).Connector.ToString();
					result += string.Format(incrementalResult, connector, nextCondition);
				}
			}
			return string.Format(enclosure, result);
		}
	}
}
