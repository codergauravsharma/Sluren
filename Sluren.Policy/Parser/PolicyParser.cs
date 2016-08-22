using System;
using System.Xml.Linq;
using System.Xml.Schema;
using Sluren.Policy.Executor;

namespace Sluren.Policy.Parser {
	public abstract class PolicyParser : IDisposable {
		protected abstract Policy ConstructPolicy(XDocument policy);
		protected abstract PolicyExecutor ConstructPolicyExecutor(XDocument policy);
		private void ValidateSchema(XDocument policy, XmlSchema policySchema) {
			XmlSchemaSet schemaSet = new XmlSchemaSet();
			schemaSet.Add(policySchema);
			//policy.Validate(schemaSet, null);
		}
		public Policy GetPolicy(PolicyCapsule policyCapsule) {
			ValidateSchema(policyCapsule.Policy, policyCapsule.Schema);
			return ConstructPolicy(policyCapsule.Policy);
		}

		public PolicyExecutor GetPolicyExecutor(PolicyCapsule policyCapsule) {
			ValidateSchema(policyCapsule.Policy, policyCapsule.Schema);
			return ConstructPolicyExecutor(policyCapsule.Policy);
		}
		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool isDisposing) {
		}
	}
}
