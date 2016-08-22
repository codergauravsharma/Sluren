using System.Collections.Generic;
using System.Linq;

namespace Sluren.Policy.Store {
	public class PolicyQueryObject {
		public string Key { get; private set; }
		public string Value { get; private set; }
		public PolicyQueryObject(string key, string value) {
			Key = key;
			Value = value;
		}
	}
	public abstract class PolicyQueryModel {
		public abstract IEnumerable<PolicyQueryObject> QueryKeys { get; }
		public string GetValueForKey(string key) {
			if (QueryKeys.Where(queryObject => queryObject.Key == key).Count() > 0) {
				return QueryKeys.Single(queryObject => queryObject.Key == key).Value;
			}
			throw new KeyNotFoundException(string.Format("The PolicyQueryModel doesn't have an entry for key - {0}", key));
		}
	}

	public class NameAndVersionBasedQueryModel : PolicyQueryModel {
		private readonly List<PolicyQueryObject> list;
		public NameAndVersionBasedQueryModel(string Name, string Version) {
			list = new List<PolicyQueryObject> {
				new PolicyQueryObject("Name", Name)
				, new PolicyQueryObject("Version", Version)
			};
		}
		public override IEnumerable<PolicyQueryObject> QueryKeys {
			get { return list; }
		}
	}
}
