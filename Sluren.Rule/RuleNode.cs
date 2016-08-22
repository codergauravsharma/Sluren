using Sluren.Decision;

namespace Sluren.Rule {
	public class RuleNode {
		public Rule Rule { get; private set; }
		public Operator Connector { get; private set; }
		public RuleNode(Rule node, Operator connector) {
			Rule = node;
			Connector = connector;
		}
	}
}
