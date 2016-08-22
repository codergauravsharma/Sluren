namespace Sluren.Decision {
	public class ConditionVisitorFactory {
		public virtual ConditionVisitor<bool> Evaluator { get; protected set; }
		public virtual ConditionVisitor<string> Visualizer { get; protected set; }

		public ConditionVisitorFactory() {
			Evaluator = new ConditionEvaluator();
			Visualizer = new ConditionVisualizer();
		}
	}
}
