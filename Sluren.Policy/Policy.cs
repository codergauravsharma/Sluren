using System;

namespace Sluren.Policy {
	public sealed class Policy : IDisposable {
		public Rule.Rule RuleSet { get; private set; }
		public string Name { get; private set; }
		public string Description { get; private set; }
		public double Version { get; private set; }
		public Policy(Rule.Rule ruleset, string name, string description, double version) {
			RuleSet = ruleset;
			Name = name;
			Description = description;
			Version = version;
		}
		public void Dispose() {
			if (RuleSet != null) RuleSet.Dispose();
		}
	}
}
