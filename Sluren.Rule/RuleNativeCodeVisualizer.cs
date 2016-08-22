using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace Sluren.Rule {
	public class RuleNativeCodeVisualizer : RuleVisitor<WebControl> {
		public override WebControl Visit(HardValueBasedRule rule) {
			WebControl panel = new Panel();

			panel.Controls.Add(new Label() {
				Text =
					string.Format(
					"{0} {1} {2}"
					, rule.Left, rule.Comparator, rule.Right)
			});

			return panel;
		}
		public override WebControl Visit(PropertyFieldBasedRule rule) {
			WebControl panel = new Panel();

			panel.Controls.Add(new Label() {
				Text =
					string.Format(
					"{0} {1} {2}"
					, rule.Left, rule.Comparator, rule.Right)
			});

			return panel;
		}
		public override WebControl Visit(TypeBasedRule rule) {
			WebControl panel = new Panel();

			panel.Controls.Add(new Label() {
				Text =
					string.Format(
					"{0} {1} {2}"
					, rule.Left, rule.Comparator, rule.Right)
			});

			return panel;
		}
		public override WebControl Visit(CompositeRule rule) {
			WebControl ruleOpeningParenthesis = new Panel();
			ruleOpeningParenthesis.Controls.Add(new Label() { Text = "(" });

			WebControl ruleContainer = new Panel();

			WebControl ruleClosingParenthesis = new Panel();
			ruleClosingParenthesis.Controls.Add(new Label() { Text = ")" });

			Rule leftRule, rightRule;

			leftRule = rightRule = rule.rules.Count() > 0 ? rule.rules.ElementAt(0).Rule : null;

			WebControl ruleRepresentation;

			if (leftRule != null) {
				ruleRepresentation = leftRule.Accept(this);
				OnRuleProcessed(leftRule, ruleRepresentation);
				ruleContainer.Controls.Add(ruleRepresentation);

				for (int ruleIndex = 1; ruleIndex < rule.rules.Count(); ++ruleIndex) {
					rightRule = rule.rules.ElementAt(ruleIndex).Rule;
					string connector = rule.rules.ElementAt(ruleIndex - 1).Connector.ToString();
					
					WebControl ruleConnector = new Panel();
					ruleConnector.Controls.Add(new Label() { Text = "<i>" + connector + "</i>" });
					ruleConnector.Style.Add("color", "black");

					ruleContainer.Controls.Add(ruleConnector);

					ruleRepresentation = rightRule.Accept(this);
					OnRuleProcessed(rightRule, ruleRepresentation);
					ruleContainer.Controls.Add(ruleRepresentation);

					leftRule = rightRule;
				}
			}

			WebControl finalRule = new Panel();

			if (ruleContainer.Controls.Count > 0) {
				finalRule.Controls.Add(ruleOpeningParenthesis);
				finalRule.Controls.Add(ruleContainer);
				finalRule.Controls.Add(ruleClosingParenthesis);
			}

			return finalRule;
		}

		public override WebControl Visit(PropertyFieldToHardValueRule rule) {
			throw new NotImplementedException();
		}
	}
}
