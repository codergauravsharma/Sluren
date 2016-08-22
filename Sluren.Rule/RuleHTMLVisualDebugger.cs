using Sluren.Decision;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace Sluren.Rule {
	public class RuleHTMLVisualDebugger : RuleVisitor<WebControl> {
		RuleVisitor<WebControl> Visualizer;
		RuleVisitor<Condition> Evaluator;
		public RuleHTMLVisualDebugger(RuleVisitor<WebControl> visualizer, RuleVisitor<Condition> evaluator) {
			Visualizer = visualizer;
			Evaluator = evaluator;
		}

		private WebControl DecorateControl(Rule rule) {
			bool result = rule.Accept(Evaluator).Evaluate();
			WebControl decoratedControl = rule.Accept(Visualizer);

			if (result) decoratedControl.Style.Add("color", "#05B911");
			else decoratedControl.Style.Add("color", "red");

			return decoratedControl;
		}
		public override WebControl Visit(HardValueBasedRule rule) {
			return DecorateControl(rule);
		}

		public override WebControl Visit(PropertyFieldBasedRule rule) {
			return DecorateControl(rule);
		}

		public override WebControl Visit(TypeBasedRule rule) {
			return DecorateControl(rule);
		}

		public override WebControl Visit(CompositeRule rule) {
			WebControl ruleOpeningParenthesis = new Panel();
			ruleOpeningParenthesis.Controls.Add(new Label() { Text = "(" });

			WebControl ruleContainer = new Panel();

			WebControl ruleClosingParenthesis = new Panel();
			ruleClosingParenthesis.Controls.Add(new Label() { Text = ")" });

			Rule leftRule, rightRule;

			leftRule = rightRule = rule.rules.Count() > 0 ? rule.rules.ElementAt(0).Rule : null;

			if (leftRule != null) {
				ruleContainer.Controls.Add(leftRule.Accept(this));

				for (int ruleIndex = 1; ruleIndex < rule.rules.Count(); ++ruleIndex) {
					rightRule = rule.rules.ElementAt(ruleIndex).Rule;
					string connector = rule.rules.ElementAt(ruleIndex - 1).Connector.ToString();

					WebControl ruleConnector = new Panel();
					ruleConnector.Controls.Add(new Label() { Text = "<i>" + connector + "</i>" });

					ruleContainer.Controls.Add(ruleConnector);
					ruleContainer.Controls.Add(rightRule.Accept(this));

					leftRule = rightRule;
				}
			}

			//ruleContainer.Controls.Add(panel);

			WebControl finalRule = new Panel();
			finalRule.Controls.Add(ruleOpeningParenthesis);
			finalRule.Controls.Add(ruleContainer);
			finalRule.Controls.Add(ruleClosingParenthesis);

			bool result = rule.Accept(Evaluator).Evaluate();

			if (result) {
				finalRule.Style.Add("border-left", "solid 1px green");
			}
			else {
				foreach (WebControl control in finalRule.Controls) {
					control.Style.Add("border-left", "solid 1px red");
				}
			}
			return finalRule;
		}

		public override WebControl Visit(PropertyFieldToHardValueRule rule) {
			throw new NotImplementedException();
		}
	}
}
