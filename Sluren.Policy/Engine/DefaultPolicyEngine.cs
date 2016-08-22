using System;

using Sluren.Policy.Executor;
using Sluren.Policy.Parser;
using Sluren.Policy.Store;

namespace Sluren.Policy.Engine {
	public class DefaultPolicyEngine : PolicyEngine {
		protected PolicyStore policyStore;
		protected PolicyParser policyParser;
		protected PolicyQueryModel policyQueryModel;
		public DefaultPolicyEngine(PolicyModelFactory factory, PolicyQueryModel query) {
			policyStore = factory.PolicyStore;
			policyParser = factory.PolicyParser;
			policyQueryModel = query;
		}
		public override bool Execute(object baseData) {
			PolicyCapsule policyCapsule = policyStore.GetPolicy(policyQueryModel);
			Policy policy = policyParser.GetPolicy(policyCapsule);

			PolicyExecutor executor = policyParser.GetPolicyExecutor(policyCapsule);

			return executor.Execute(policy, baseData);
		}
		public override void Pause() { throw new NotImplementedException(); }
		public override void Resume() { throw new NotImplementedException(); }
		public override void Stop() { throw new NotImplementedException(); }
		public override string ToString() { return base.ToString(); }
		protected virtual bool BeforeLoad() { return true; }
		protected virtual void AfterLoad() { }
		protected virtual bool BeforeExecute(Policy policy) { return true; }
		protected virtual void AfterExecute() { }
	}
}
