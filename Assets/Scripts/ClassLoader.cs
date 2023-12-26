using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ClassLoader
{
	public static IEnumerable<Type> LoadAllClasses(string namespaceName)
	{
		var classList = new List<Type>();
		var assemblies = AppDomain.CurrentDomain.GetAssemblies();

		foreach (var assembly in assemblies)
		{
			classList.AddRange(
				assembly.GetTypes().Where(
					t => t.IsClass && t.Namespace == namespaceName && !t.IsSubclassOf(typeof(MonoBehaviour))
				)
			);
		}

		return classList;
	}
}