namespace Sluren.Policy.Executor {
	public class PolicyPostProcessorInvoker : PolicyExecutionExtender {
		PolicyPostProcessor policyPostProcessor;
		public PolicyPostProcessorInvoker(PolicyExecutor executionStrategy, PolicyPostProcessor postProcessor)
			: base(executionStrategy) {
			policyPostProcessor = postProcessor;
		}
		public override bool Execute(Policy policy, object baseData) {
			bool policyEvaluationResult = base.Execute(policy, baseData);

			if (policyPostProcessor != null) {
				if (policyEvaluationResult == true) policyPostProcessor.PolicySucceeded(policy);
				else policyPostProcessor.PolicyFailed(policy);
			}
			return policyEvaluationResult;
		}
	}
}
