using System;

namespace Sluren.Policy.Store {
	public abstract class PolicyStore : IDisposable {
		public abstract PolicyCapsule GetPolicy(PolicyQueryModel queryModel);
		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool isDisposing) {
		}
	}

}
