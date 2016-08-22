using Sluren.Rule;

namespace Sluren.Policy.Executor {
	public class VanillaExecutor : PolicyExecutor {
		PolicyDataProvider policyDataProvider;
		public VanillaExecutor(PolicyDataProvider dataProvider) {
			policyDataProvider = dataProvider;
		}
		public override bool Execute(Policy policy, object baseData) {
			object enrichedData = policyDataProvider == null ? baseData : policyDataProvider.FetchDataForPolicy(baseData);
			RuleEvaluator evaluator = new DefaultEvaluator(new RuleParserEnriched(enrichedData));

			return evaluator.EvaluateRule(policy.RuleSet);
		}
	}
}
