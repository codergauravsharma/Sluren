namespace Sluren.Policy {
	public abstract class PolicyPostProcessor {
		public abstract bool PolicySucceeded(Policy policy);
		public abstract bool PolicyFailed(Policy policy);
	}
	public abstract class PolicyDataProvider {
		public abstract object FetchDataForPolicy(object baseData);
	}
}
