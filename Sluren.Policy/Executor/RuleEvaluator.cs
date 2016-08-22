using Sluren.Rule;
using System;

namespace Sluren.Policy.Executor {
	public abstract class RuleEvaluator {
		public abstract bool EvaluateRule(Rule.Rule rule);
	}

	public class DefaultEvaluator : RuleEvaluator {
		RuleVisitor<RuleParsingResult> policyEvaluator;
		public DefaultEvaluator(RuleVisitor<RuleParsingResult> evaluator) {
			policyEvaluator = evaluator;
			policyEvaluator.RuleProcessed += policyEvaluator_RuleProcessed;
		}
		private void policyEvaluator_RuleProcessed(Rule.Rule rule, RuleParsingResult output) {
			Console.WriteLine("Left: " + output.Left.ToString());
			Console.WriteLine("Right: " + output.Right.ToString());
		}
		public override bool EvaluateRule(Rule.Rule rule) {
			return rule.Accept(policyEvaluator).Condition.Evaluate();
		}
	}
}
