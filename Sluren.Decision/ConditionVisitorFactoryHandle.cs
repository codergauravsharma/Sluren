using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Sluren.Decision {
	public static class ConditionVisitorFactoryHandle {
		public static ConditionVisitorFactory Factory { get; private set; }
		private static IUnityContainer UnityContainer { get; set; }
		static ConditionVisitorFactoryHandle() {
			UnityContainer = new UnityContainer();
			UnityContainer.LoadConfiguration();
			Factory = UnityContainer.Resolve<ConditionVisitorFactory>();
		}
	}
}
