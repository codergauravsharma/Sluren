using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System.Collections;
using Sluren.Decision;
using Sluren.Infrastructure.Helpers;

namespace Sluren.Rule {
	public class RuleParser : RuleVisitor<Condition> {
		protected object dataToEvaluateRuleOn;
		public RuleParser(object ruleInput) {
			if (ruleInput == null)
				throw new NullReferenceException(
					string.Format("The incoming object for the {0} is null", this.GetType().Name)
				);

			dataToEvaluateRuleOn = ruleInput;
		}
		private object GetFinalObjectFromExpression(object objectToExtractFrom, string expression) {
			// Throw is the extraction target is null
			if (objectToExtractFrom == null)
				throw new NullReferenceException("The object being evaluated is null");

			//Split the . expression to get all tokens
			IEnumerable<string> elements = expression.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).ToList();

			//If the token count is 0 throw exception. Need at least one token in the expression
			if (elements.Count() < 1)
				throw new ArgumentException(
					"For variable based rules, left and right operand must have an expression."
					+ " e.g. Emp.Project.SourceCompany."
					+ " Hard constants are not allowed."
				);

			//Cache the first token that will be the variable
			string baseElement = elements.First();

			//Query the object members to search for a public property or field by the name taken by the base element
			IEnumerable<MemberInfo> memberInfoCollection =
				objectToExtractFrom
				.GetType()
				.GetMembers()
				.Where(mi => mi.MemberType == MemberTypes.Property || mi.MemberType == MemberTypes.Field)
				.Where(mi => mi.Name == elements.First());

			//Throw if the query didn't find any property or field that matches the name
			if (memberInfoCollection.Count() != 1)
				throw new ArgumentException(
					string.Format(
						"The {0} class object being evaluated does not have a unique public property or field named {1}"
						, objectToExtractFrom.GetType().Name
						, baseElement
					)
				);

			MemberInfo memberInfo = memberInfoCollection.First();
			object extractedObject = null;
			string memberActualType = string.Empty;

			//Depending on the type of the member, try getting the value from a real object
			if (memberInfo.MemberType == MemberTypes.Property) {
				PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
				memberActualType = propertyInfo.PropertyType.Name;
				extractedObject = propertyInfo.GetValue(objectToExtractFrom);
			}
			else if (memberInfo.MemberType == MemberTypes.Field) {
				FieldInfo fieldInfo = (FieldInfo)memberInfo;
				memberActualType = fieldInfo.FieldType.Name;
				extractedObject = fieldInfo.GetValue(objectToExtractFrom);
			}

			//If the extracted object is null, throw
			if (extractedObject == null)
				throw new NullReferenceException(
					string.Format(
						"Null {0} object exposed by the property {1}, in the {2} object being evaluated"
						, memberActualType
						, memberInfo.Name
						, objectToExtractFrom.GetType().Name
					)
				);

			//Continue the parsing the next token in the expression
			if (elements.Count() > 1) {
				IEnumerable<string> newRange = elements.ToList().GetRange(1, elements.Count() - 1);

				//Re-assign the extraction target to continue extraction
				extractedObject = GetFinalObjectFromExpression(extractedObject, string.Join(".", newRange));
			}

			return extractedObject;
		}
		public override Condition Visit(HardValueBasedRule rule) {
			Type hardType = Reflection.RetrieveType(rule.HardType);

			TypeConverter typeConverter = TypeDescriptor.GetConverter(hardType);
			object objectForOperand = typeConverter.ConvertFromString(rule.Left);

			Type operand = typeof(Operand<>).MakeGenericType(hardType);
			object operandObject = Activator.CreateInstance(operand, objectForOperand);

			object[] paramContainer;

			Type paramType = typeof(List<>).MakeGenericType(new Type[] { hardType });
			IList paramHost = Activator.CreateInstance(paramType, new object[] { }) as IList;

			if (rule.Right.Contains("and")) {
				string[] strArr = rule.Right.Split(new string[] { "and" }, StringSplitOptions.RemoveEmptyEntries);
				if (strArr.Length != 2)
					throw new ArgumentOutOfRangeException(rule.Comparator + " must have only 2 right operands with an \"and\" connector");

				strArr.ToList().ForEach((param) => { paramHost.Add(typeConverter.ConvertFromString(param)); });

				paramContainer = new object[2];
				paramHost.CopyTo(paramContainer, 0);
			}
			else if (rule.Right.Contains(",")) {
				string[] strArr = rule.Right.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
				if (strArr.Length < 1)
					throw new ArgumentOutOfRangeException(rule.Comparator + " must have at least 1 right operand with an \",\" connector");

				strArr.ToList().ForEach((param) => { paramHost.Add(typeConverter.ConvertFromString(param)); });

				paramContainer = new object[] { paramHost };
			}
			else
				paramContainer = new object[] { typeConverter.ConvertFromString(rule.Left) };

			return new BooleanCondition((bool)operand.GetMethod(rule.Comparator).Invoke(operandObject, paramContainer));
		}
		public override Condition Visit(PropertyFieldBasedRule rule) {
			object objectForOperand = GetFinalObjectFromExpression(dataToEvaluateRuleOn, rule.Left.Trim());

			Type typeLeft = objectForOperand.GetType();

			Type operand = typeof(Operand<>).MakeGenericType(typeLeft);
			object operandObject = Activator.CreateInstance(operand, objectForOperand);

			TypeConverter typeConverter = TypeDescriptor.GetConverter(typeLeft);

			object[] paramContainer;

			Type paramType = typeof(List<>).MakeGenericType(typeLeft);
			IList paramHost = Activator.CreateInstance(paramType) as IList;

			if (rule.Right.Contains("and")) {
				string[] strArr = rule.Right.Split(new string[] { "and" }, StringSplitOptions.RemoveEmptyEntries);
				if (strArr.Length != 2)
					throw new ArgumentOutOfRangeException(rule.Comparator + " must have only 2 right operands with an \"and\" connector");

				strArr.ToList().ForEach((param) => { paramHost.Add(GetFinalObjectFromExpression(dataToEvaluateRuleOn, param.Trim())); });

				paramContainer = new object[2];
				paramHost.CopyTo(paramContainer, 0);
			}
			else if (rule.Right.Contains(",")) {
				string[] strArr = rule.Right.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
				if (strArr.Length < 1)
					throw new ArgumentOutOfRangeException(rule.Comparator + " must have at least 1 right operand with an \",\" connector");

				strArr.ToList().ForEach((param) => { paramHost.Add(GetFinalObjectFromExpression(dataToEvaluateRuleOn, param.Trim())); });

				paramContainer = new object[] { paramHost };
			}
			else
				paramContainer = new object[] { GetFinalObjectFromExpression(dataToEvaluateRuleOn, rule.Right.Trim()) };

			return new BooleanCondition((bool)operand.GetMethod(rule.Comparator).Invoke(operandObject, paramContainer));
		}
		public override Condition Visit(PropertyFieldToHardValueRule rule) {
			object objectForOperand = GetFinalObjectFromExpression(dataToEvaluateRuleOn, rule.Left.Trim());

			Type typeLeft = objectForOperand.GetType();

			Type operand = typeof(Operand<>).MakeGenericType(typeLeft);
			object operandObject = Activator.CreateInstance(operand, objectForOperand);

			TypeConverter typeConverter = TypeDescriptor.GetConverter(typeLeft);

			object[] paramContainer;

			Type paramType = typeof(List<>).MakeGenericType(typeLeft);
			IList paramHost = Activator.CreateInstance(paramType) as IList;

			if (rule.Right.Contains("and")) {
				string[] strArr = rule.Right.Split(new string[] { "and" }, StringSplitOptions.RemoveEmptyEntries);
				if (strArr.Length != 2)
					throw new ArgumentOutOfRangeException(rule.Comparator + " must have only 2 right operands with an \"and\" connector");

				strArr.ToList().ForEach((param) => { paramHost.Add(typeConverter.ConvertFromString(param)); });

				paramContainer = new object[2];
				paramHost.CopyTo(paramContainer, 0);
			}
			else if (rule.Right.Contains(",")) {
				string[] strArr = rule.Right.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
				if (strArr.Length < 1)
					throw new ArgumentOutOfRangeException(rule.Comparator + " must have at least 1 right operand with an \",\" connector");

				strArr.ToList().ForEach((param) => { paramHost.Add(typeConverter.ConvertFromString(param)); });

				paramContainer = new object[] { paramHost };
			}
			else
				paramContainer = new object[] { typeConverter.ConvertFromString(rule.Left) };

			return new BooleanCondition((bool)operand.GetMethod(rule.Comparator).Invoke(operandObject, paramContainer));
		}
		public override Condition Visit(TypeBasedRule rule) {
			string[] strArr = rule.Left.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);

			if (strArr.Length < 1)
				throw new Exception("For entity based rules, at least the base type is need as left operand");

			Type targetType = Reflection.RetrieveType(strArr[0].Trim());

			List<MemberInfo> memberInfoCollection =
				dataToEvaluateRuleOn
				.GetType()
				.GetMembers()
				.Where(mi => (mi.MemberType == MemberTypes.Property || mi.MemberType == MemberTypes.Field))
				.ToList();

			if (memberInfoCollection.Count < 1)
				throw new ArgumentException(
					string.Format(
						"The {0} object being evaluated does not contain any public fields or properties"
						, dataToEvaluateRuleOn.GetType().Name
					)
				);

			Type entityType = typeof(List<>).MakeGenericType(targetType);
			IList entityHost = Activator.CreateInstance(entityType) as IList;

			memberInfoCollection.ForEach((memberInfo) => {
				if (memberInfo is PropertyInfo) {
					if (((PropertyInfo)memberInfo).PropertyType == targetType) {
						entityHost.Add(GetFinalObjectFromExpression(dataToEvaluateRuleOn, memberInfo.Name));
					}
				}
				else {
					if (((FieldInfo)memberInfo).FieldType == targetType) {
						entityHost.Add(GetFinalObjectFromExpression(dataToEvaluateRuleOn, memberInfo.Name));
					}
				}
			});

			if (entityHost.Count < 1)
				throw new ArgumentException(
					string.Format(
						"The {0} object being evaluated does not contain any public fields or properties of type {1}"
						, dataToEvaluateRuleOn.GetType().Name
						, targetType.Name
					)
				);

			object[] extractedObjects = new object[entityHost.Count];

			if (strArr.Length > 1) {
				string expression = string.Join(".", strArr.ToList().GetRange(1, strArr.Count() - 1));

				for (int objectIndex = 0; objectIndex < entityHost.Count; ++objectIndex)
					extractedObjects[objectIndex] = GetFinalObjectFromExpression(entityHost[objectIndex], expression);
			}

			List<bool> results = new List<bool>();

			foreach (var extractedObject in extractedObjects) {
				Type extractedObjectType = extractedObject.GetType();

				Type operand = typeof(Operand<>).MakeGenericType(extractedObjectType);
				object operandObject = Activator.CreateInstance(operand, extractedObject);

				object[] paramContainer;

				Type paramType = typeof(List<>).MakeGenericType(extractedObjectType);
				IList paramHost = Activator.CreateInstance(paramType) as IList;

				if (rule.Right.Contains("and")) {
					string[] strArrParams = rule.Right.Split(new string[] { "and" }, StringSplitOptions.RemoveEmptyEntries);
					if (strArrParams.Length != 2)
						throw new ArgumentOutOfRangeException(rule.Comparator + " must have only 2 right operands with an \"and\" connector");

					strArrParams.ToList().ForEach((param) => {
						paramHost.Add(Convert.ChangeType(param.Trim(), extractedObjectType));
					});

					paramContainer = new object[2];
					paramHost.CopyTo(paramContainer, 0);
				}
				else if (rule.Right.Contains(",")) {
					string[] strArrParams = rule.Right.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
					if (strArrParams.Length < 1)
						throw new ArgumentOutOfRangeException(rule.Comparator + " must have at least 1 right operand with an \",\" connector");

					strArrParams.ToList().ForEach((param) => {
						paramHost.Add(Convert.ChangeType(param.Trim(), extractedObjectType));
					});

					paramContainer = new object[] { paramHost };
				}
				else
					paramContainer = new object[] { Convert.ChangeType(rule.Right.Trim(), extractedObjectType) };

				results.Add((bool)operand.GetMethod(rule.Comparator).Invoke(operandObject, paramContainer));
			}

			if (results.Distinct().Count() < 1)
				throw new InvalidOperationException("Undeterministic rule ! couldn't evaluate :-(");

			return new BooleanCondition(results.Distinct().Count() > 1 ? false : results.ElementAt(0));
		}
		public override Condition Visit(CompositeRule rule) {
			CompositeCondition synthesizedCondition = new CompositeCondition();

			foreach (var ruleItem in rule.rules) {
				synthesizedCondition.Add(ruleItem.Rule.Accept(this), ruleItem.Connector);
			}

			return synthesizedCondition;
		}
	}
}
