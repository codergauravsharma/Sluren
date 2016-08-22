using System.Linq;

namespace Sluren.Decision {
	public class ConditionEvaluator : ConditionVisitor<bool> {
		public override bool Visit(BooleanCondition condition) {
			return condition.Expression;
		}

		public override bool Visit(CompositeCondition condition) {
			Condition result = null, rightCondition = null;
			result = condition.Conditions.Count() > 0 ? condition.Conditions.ElementAt(0).Condition : null;

			if (result != null) {
				for (int conditionIndex = 1; conditionIndex < condition.Conditions.Count(); ++conditionIndex) {
					rightCondition = condition.Conditions.ElementAt(conditionIndex).Condition;
					//result = result.ChainTo(rightCondition, condition.Conditions.ElementAt(conditionIndex - 1).Connector);
					result = condition.Conditions.ElementAt(conditionIndex - 1).Connector.Evaluate(result, rightCondition);
				}
			}

			return result == null ? true : result.Evaluate();
		}
	}
}
