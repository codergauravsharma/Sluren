using Sluren.Policy.Parser;
using Sluren.Policy.Store;

namespace Sluren.Policy {
	public class DefaultFactoryForPolicyModel : PolicyModelFactory {
		PolicyStore pStore;
		PolicyParser pParser;

		public DefaultFactoryForPolicyModel() {
			pStore = new FileSystemPolicyStore();
			pParser = new ChainingCapablePolicyParser();
		}
		public override PolicyStore PolicyStore {
			get { return pStore; }
		}
		public override PolicyParser PolicyParser {
			get { return pParser; }
		}
		protected override void Dispose(bool isDisposing) {
			if (isDisposing) {
				pStore.Dispose();
				pParser.Dispose();
			}
		}
	}
}
