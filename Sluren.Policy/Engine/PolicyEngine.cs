namespace Sluren.Policy.Engine {
	public enum PolicyEngineState {
		YetToStart = 0,
		InProcess = 1,
		Complete = 2,
		Cancelled = 3,
		Paused = 4,
		Aborted = 5
	}
	public abstract class PolicyEngine {
		public PolicyEngineState EngineState { get; protected set; }
		protected PolicyEngine() {
			EngineState = PolicyEngineState.YetToStart;
		}
		public abstract bool Execute(object baseData);
		public abstract void Pause();
		public abstract void Resume();
		public abstract void Stop();
	}
}
