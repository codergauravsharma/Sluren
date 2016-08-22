using System;

namespace Sluren.Rule {
	public abstract class Rule: IDisposable {
		protected Rule() { }
		public abstract T Accept<T>(RuleVisitor<T> visitor);
		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool isDisposing) {
		}
	}
}
