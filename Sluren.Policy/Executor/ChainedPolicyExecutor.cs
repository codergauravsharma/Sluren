using System;

namespace Sluren.Policy.Executor {
	public class ChainedPolicyExecutor : PolicyExecutionExtender {
		string nextIfTrue, nextIfFalse;
		public ChainedPolicyExecutor(PolicyExecutor executionStrategy, string nextPolicyNameIfTrue, string nextPolicyNameIfFalse)
			: base(executionStrategy) {
			nextIfTrue = nextPolicyNameIfTrue;
			nextIfFalse = nextPolicyNameIfFalse;
		}
		public override bool Execute(Policy policy, object baseData) {
			bool evaluationResult = base.Execute(policy, baseData);

			string nextPolicy = evaluationResult ? nextIfTrue : nextIfFalse;

			if (nextPolicy != string.Empty) {
				string[] nextPolicySet = nextPolicy.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				if (nextPolicySet.Length != 2) throw new Exception("Invalid successor policy configuration - " + nextPolicy);

				//Dispose code here
				policy.Dispose();

				//PolicyEngine engine = new DefaultPolicyEngine(
				//	new DefaultFactoryForPolicyModel()
				//	, new NameAndVersionBasedQueryModel(nextPolicySet[0], nextPolicySet[1])
				//);

				//evaluationResult = engine.Execute(baseData);
			}
			return evaluationResult;
		}
	}
}
