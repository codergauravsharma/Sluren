namespace Sluren.Policy.Executor {
	public abstract class PolicyExecutor {
		public abstract bool Execute(Policy policy, object baseData);
	}
}
