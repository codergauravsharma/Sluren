namespace Sluren.Decision {
	public class ConditionNode {
		public Condition Condition { get; private set; }
		public Operator Connector { get; private set; }
		public ConditionNode(Condition node, Operator connector) {
			Condition = node;
			Connector = connector;
		}
	}
}
