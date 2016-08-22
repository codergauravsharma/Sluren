using Sluren.Decision;
using Sluren.Rule;
using System.IO;
using System.Xml.Linq;

namespace Sluren.Policy.Parser {
	public class MultiplePoliciesInOneParser : ChainingCapablePolicyParser {
		protected override Policy ConstructPolicy(XDocument policy) {
			XElement rootNode = policy.Root;
			CompositeRule myRule = new CompositeRule();
			foreach (var element in rootNode.Elements()) {
				myRule.Add(base.ConstructRuleSet(XDocument.Load(new StringReader(element.ToString()))), new Or());
			}
			return new Policy(myRule, "", "", 1.1);
		}
	}
}
