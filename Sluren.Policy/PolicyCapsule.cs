using System.Xml.Linq;
using System.Xml.Schema;

namespace Sluren.Policy {
	public sealed class PolicyCapsule {
		public XDocument Policy { get; private set; }
		public XmlSchema Schema { get; private set; }
		public PolicyCapsule(XDocument policy, XmlSchema schema) {
			Policy = policy;
			Schema = schema;
		}
	}
}
