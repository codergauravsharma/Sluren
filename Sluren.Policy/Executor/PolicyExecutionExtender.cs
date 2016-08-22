namespace Sluren.Policy.Executor {
	public abstract class PolicyExecutionExtender : PolicyExecutor {
		protected PolicyExecutor executionStrategy;
		protected PolicyExecutionExtender(PolicyExecutor baseStrategy) {
			executionStrategy = baseStrategy;
		}
		public override bool Execute(Policy policy, object baseData) {
			return executionStrategy.Execute(policy, baseData);
		}
	}
}
