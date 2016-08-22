using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Sluren.Policy.Executor;
using Sluren.Infrastructure.Helpers;
using Sluren.Rule;
using Sluren.Decision;

namespace Sluren.Policy.Parser {
	public class ChainingCapablePolicyParser : PolicyParser {
		private void ValidateRootNode(XDocument policy) {
			XElement policyRoot = policy.Root;
			if (policyRoot.Name.ToString().ToLower() != "policy") {
				throw new NotSupportedException(
					string.Format(
						"The node \"{0}\" is not a valid root node for the parser {1}"
						, policyRoot.Name.ToString()
						, this.GetType().Name
					)
				);
			}
		}
		protected override PolicyExecutor ConstructPolicyExecutor(XDocument policy) {
			ValidateRootNode(policy);
			XElement policyRoot = policy.Root;
			string policyDataProviderAttributeName = "dataprovider";
			string policyPostProcessorAttributeName = "postprocessor";
			string policySuccessorOnTrueAttributeName = "successorontrue";
			string policySuccessorOnFalseAttributeName = "successoronfalse";

			XAttribute policyDataProviderAttribute = policyRoot.Attribute(policyDataProviderAttributeName);
			if (policyDataProviderAttribute == null)
				throw new NullReferenceException(
					string.Format(
						"The attribute - {0}, wasn't found on the policy node"
						, policyDataProviderAttributeName
					)
				);

			XAttribute policyPostProcessorAttribute = policyRoot.Attribute(policyPostProcessorAttributeName);
			if (policyPostProcessorAttribute == null)
				throw new NullReferenceException(
					string.Format(
						"The attribute - {0}, wasn't found on the policy node"
						, policyPostProcessorAttributeName
					)
				);

			PolicyDataProvider policyDataProvider = null;

			try {
				policyDataProvider = (PolicyDataProvider)Activator.CreateInstance(
					Reflection.RetrieveType(policyDataProviderAttribute.Value)
				);
			}
			catch (TypeNotFoundException ex) {
				Console.Write(ex);
			}
			
			PolicyExecutor executor = new VanillaExecutor(policyDataProvider);

			if (policyPostProcessorAttribute.Value != string.Empty) {
				PolicyPostProcessor policyPostProcessor = (PolicyPostProcessor)Activator.CreateInstance(
					Reflection.RetrieveType(policyPostProcessorAttribute.Value)
				);
				executor = new PolicyPostProcessorInvoker(
					executor
					, policyPostProcessor
				);
			}
	
			XAttribute successor = policy.Root.Attribute(policySuccessorOnTrueAttributeName);
			string nextTrue = successor == null ? string.Empty : successor.Value;

			successor = policy.Root.Attribute(policySuccessorOnFalseAttributeName);
			string nextFalse = successor == null ? string.Empty : successor.Value;

			if (nextTrue != string.Empty || nextFalse != string.Empty)
				executor = new ChainedPolicyExecutor(executor, nextTrue, nextFalse);

			return executor;
		}
		protected override Policy ConstructPolicy(XDocument policy) {
			ValidateRootNode(policy);

			XElement policyRoot = policy.Root;
			
			/*
			string policySuccessorOnTrueAttributeName = "successorontrue";
			string policySuccessorOnFalseAttributeName = "successoronfalse";

			XAttribute successor = policy.Root.Attribute(policySuccessorOnTrueAttributeName);
			string nextTrue = successor == null ? string.Empty : successor.Value;

			successor = policy.Root.Attribute(policySuccessorOnFalseAttributeName);
			string nextFalse = successor == null ? string.Empty : successor.Value;

			string[] nextPolicySetTrue;
			Policy nextPolicyIfTrue = null;
			if (nextTrue != string.Empty) {
				nextPolicySetTrue = nextTrue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				if (nextPolicySetTrue.Length != 2) throw new Exception("Invalid successor policy configuration - " + nextTrue);
				nextPolicyIfTrue = ConstructPolicy(new DefaultFactoryForPolicyModel().PolicyStore.GetPolicy(new NameAndVersionBasedQueryModel(nextPolicySetTrue[0], nextPolicySetTrue[1])).Policy);
			}

			string[] nextPolicySetFalse;
			Policy nextPolicyIfFalse = null;
			if (nextFalse != string.Empty) {
				nextPolicySetFalse = nextFalse.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				if (nextPolicySetFalse.Length != 2) throw new Exception("Invalid successor policy configuration - " + nextFalse);
				nextPolicyIfFalse = ConstructPolicy(new DefaultFactoryForPolicyModel().PolicyStore.GetPolicy(new NameAndVersionBasedQueryModel(nextPolicySetFalse[0], nextPolicySetFalse[1])).Policy);
			}
			*/

			return new Policy(
				ConstructRuleSet(policy)
				, policyRoot.Attribute("name").Value
				, policyRoot.Attribute("description").Value
				, Convert.ToDouble(policyRoot.Attribute("version").Value)
			);
		}
		protected virtual Rule.Rule ConstructRuleSet(XDocument policy) {
			CompositeRule cRule = new CompositeRule();

			foreach (XElement node in policy.Root.Elements()) {
				KeyValuePair<Rule.Rule, Operator> rule = MakeRule(node);
				cRule.Add(rule.Key, rule.Value);
			}

			return cRule;
		}
		protected virtual KeyValuePair<Rule.Rule, Operator> MakeRule(XElement node) {
			XAttribute connector = node.Attribute("connector");

			Operator oper = (connector != null && connector.Value.ToLower() == "and") ? (Operator)new And() : new Or();

			if (node.Name == "Single") {
				return new KeyValuePair<Rule.Rule, Operator>(
					new HardValueBasedRule(
						hardType: node.Name.ToString()
						, left: node.Attribute("left").Value
						, comparator: node.Attribute("comparator").Value
						, right: node.Attribute("right").Value
					)
					, oper
				);
			}
			else if (node.Name == "var") {
				return new KeyValuePair<Rule.Rule, Operator>(
					new PropertyFieldBasedRule(
						left: node.Attribute("left").Value
						, comparator: node.Attribute("comparator").Value
						, right: node.Attribute("right").Value
					)
					, oper
				);
			}
			else if (node.Name == "type") {
				return new KeyValuePair<Rule.Rule, Operator>(
					new TypeBasedRule(
						left: node.Attribute("left").Value
						, comparator: node.Attribute("comparator").Value
						, right: node.Attribute("right").Value
					)
					, oper
				);
			}
			else if (node.Name == "rules") {
				return new KeyValuePair<Rule.Rule, Operator>(
					ConstructRuleSet(XDocument.Load(new StringReader(node.ToString())))
					, oper
				);
			}
			else
				throw new InvalidDataException(node.Name.ToString() + "isn't a support rule type");
		}
		protected override void Dispose(bool isDisposing) {
			if (isDisposing) {
			}
		}
	}
}
