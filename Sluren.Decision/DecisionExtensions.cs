namespace Sluren.Decision {
	public static class DecisionExtensions {
		public static Condition And(this Condition left, Condition right) {
			return (new And()).Evaluate(left, right);
		}
		public static Condition Or(this Condition left, Condition right) {
			return (new Or()).Evaluate(left, right);
		}
	}
}
