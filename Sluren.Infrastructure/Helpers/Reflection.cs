using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sluren.Infrastructure.Helpers {
	public class TypeNotFoundException : Exception {
		public TypeNotFoundException(string message) : base(message) { }
	}
	public static class Reflection {
		private static Dictionary<Assembly, List<Type>> loadedTypes;
		static Reflection() {
			loadedTypes = new Dictionary<Assembly, List<Type>>();

			AppDomain
			.CurrentDomain
			.GetAssemblies()
			.ToList()
			.ForEach((loadedAssembly) => { loadedTypes.Add(loadedAssembly, loadedAssembly.GetTypes().ToList()); });
		}
		public static Type RetrieveType(string typeName) {
			Type hardType = null;
			bool found = false;

			foreach (Assembly loadedAssembly in loadedTypes.Keys) {
				foreach (Type type in loadedTypes[loadedAssembly]) {
					if (type.FullName.EndsWith("." + typeName) || type.FullName == typeName) {
						hardType = type;
						found = true;
						break;
					}
				}
				if (found)
					break;
			}

			if (hardType == null)
				throw new TypeNotFoundException(
					string.Format("The requested type - {0}, isn't loaded in the current context", typeName)
				);

			return hardType;
		}
	}

}
