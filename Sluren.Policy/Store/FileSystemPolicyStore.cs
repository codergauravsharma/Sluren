using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Sluren.Policy.Store {
	public class FileSystemPolicyStore : PolicyStore {
		string policyName;
		double policyVersion;

		private Dictionary<string, string> policyMap, policySchemaMap;
		public FileSystemPolicyStore() {
			policyMap = new Dictionary<string, string>();
			policySchemaMap = new Dictionary<string, string>();

			policyMap.Add("BasicPolicyApplicability", "D:\\POLicyENgine\\TestApp\\BasicPolicyApplicability.xml");
			policyMap.Add("BasicPolicyRules", "D:\\POLicyENgine\\TestApp\\BasicPolicyRules.xml");
			policyMap.Add("VisaPolicyApplicability", "D:\\POLicyENgine\\TestAppVisaPolicyApplicability.xml");

			policySchemaMap.Add("BasicPolicyApplicability", "D:\\POLicyENgine\\TestApp\\BaseRules.xsd");
			policySchemaMap.Add("BasicPolicyRules", "D:\\POLicyENgine\\TestApp\\BaseRules.xsd");
			policySchemaMap.Add("VisaPolicyApplicability", "D:\\POLicyENgine\\TestApp\\BaseRules.xsd");

		}
		public override PolicyCapsule GetPolicy(PolicyQueryModel queryModel) {
			policyName = queryModel.GetValueForKey("Name");
			policyVersion = Convert.ToDouble(queryModel.GetValueForKey("Version"));

			XDocument policy;
			XmlSchema policySchema;

			if (policyMap.ContainsKey(policyName))
				policy = XDocument.Load(policyMap[policyName]);
			else
				throw new Exception("Policy name not mapped/found");

			if (policySchemaMap.ContainsKey(policyName)) {
				using (FileStream fs = new FileStream(policySchemaMap[policyName], FileMode.Open)) {
					policySchema = XmlSchema.Read(fs, null);
				}
			}
			else
				throw new Exception("Policy schema not mapped/found");

			return new PolicyCapsule(policy, policySchema);
		}
		protected override void Dispose(bool isDisposing) {
			if (isDisposing) {
				if (policyMap != null) policyMap.Clear();
				if (policySchemaMap != null) policySchemaMap.Clear();
			}
		}
	}
}
