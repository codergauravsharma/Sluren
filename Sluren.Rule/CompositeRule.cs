using Sluren.Decision;
using System.Collections.Generic;
using System.Linq;

namespace Sluren.Rule {
	public class CompositeRule : Rule {
		List<RuleNode> ruleCollection;
		public IEnumerable<RuleNode> rules { get { return ruleCollection.AsEnumerable(); } }
		public CompositeRule() {
			ruleCollection = new List<RuleNode>();
		}
		
		public CompositeRule Add(Rule rule, Operator connector) {
			AddRule(rule, connector); return this;
		}

		public void Remove(Rule rule) {
			ruleCollection.Remove(ruleCollection.First((r) => { return r.Rule == rule; }));
		}
		protected virtual void AddRule(Rule rule, Operator connector) {
			ruleCollection.Add(new RuleNode(rule, connector));
		}
		public override T Accept<T>(RuleVisitor<T> visitor) {
			return visitor.Visit(this);
		}
		protected override void Dispose(bool isDisposing) {
			if (isDisposing)
				if (ruleCollection != null) ruleCollection.Clear();
		}
	}
}
