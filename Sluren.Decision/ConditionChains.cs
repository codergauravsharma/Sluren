namespace Sluren.Decision {
	public static class ConditionChains {
		public static CompositeCondition And(this CompositeCondition condition, Condition link) {
			return condition.Add(link, new And());
		}
		public static CompositeCondition Or(this CompositeCondition condition, Condition link) {
			return condition.Add(link, new Or());
		}
	}
}
