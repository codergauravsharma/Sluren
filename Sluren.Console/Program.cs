using Sluren.Decision;

namespace Sluren.Console {
	class Program {
		static void Main(string[] args) {
			Condition condition = (new CompositeCondition()).Add(new BooleanCondition(false), new Or());

			System.Console.WriteLine(condition.Evaluate());

			System.Console.WriteLine(new BooleanCondition(false).Or(new BooleanCondition(true)));
			System.Console.WriteLine(new CompositeCondition().And(new BooleanCondition(false)));
		}
	}
}
