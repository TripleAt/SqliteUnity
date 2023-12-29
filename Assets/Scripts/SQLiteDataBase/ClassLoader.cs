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

	public static void DynamicCopyPropertiesWithCommonInterface(object source, object target, Type interfaceType)
	{
		foreach (var prop in interfaceType.GetProperties())
		{
			var sourceProp = source.GetType().GetProperty(prop.Name);
			var targetProp = target.GetType().GetProperty(prop.Name);

			if (sourceProp == null || targetProp == null || !targetProp.CanWrite) continue;
			var valueToCopy = sourceProp.GetValue(source);
			targetProp.SetValue(target, valueToCopy);
		}
	}

}