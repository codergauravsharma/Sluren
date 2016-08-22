using System;

using Sluren.Policy.Parser;
using Sluren.Policy.Store;

namespace Sluren.Policy {
	public abstract class PolicyModelFactory : IDisposable {
		public abstract PolicyStore PolicyStore { get; }
		public abstract PolicyParser PolicyParser { get; }
		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool isDisposing) {
		}
	}
}
