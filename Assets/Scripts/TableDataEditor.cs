using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SQLite;
using UnityEditor;
using UnityEngine;

public class TableDataEditor : EditorWindow
{
	[MenuItem("Window/Table Data Editor")]
	public static void ShowWindow()
	{
		GetWindow<TableDataEditor>("Table Data Editor");
	}

	void OnGUI()
	{
		if (GUILayout.Button("Save Data"))
		{
			SaveData();
		}

		if (GUILayout.Button("Load Data"))
		{
			LoadData();
		}
	}

	void SaveData()
	{
		var classes = ClassLoader.LoadAllClasses("Tables"); // Replace 'YourNamespace' with the actual namespace

		const string path = "C://Test/Testdb";
		var db = new SQLiteAsyncConnection(path);
		foreach (var classType in classes)
		{
			SaveClass(classType, db);
		}
	}

	private static async Task SaveClass(Type classType, SQLiteAsyncConnection db)
	{
		// AsyncTableQuery<T> オブジェクトを取得
		MethodInfo tableMethod = db.GetType().GetMethod("Table")?.MakeGenericMethod(classType);
		var interfaces = classType.GetInterfaces();
		if (tableMethod == null)
		{
			return;
		}

		var tableInstance = tableMethod.Invoke(db, null);
		MethodInfo toListAsyncMethod = tableInstance.GetType().GetMethod("ToListAsync");
		if (toListAsyncMethod == null) return;

		// UniTask から List<T> を非同期的に取得
		dynamic task = toListAsyncMethod.Invoke(tableInstance, null);
		var listInstance = await task;

		// JSONにシリアライズしてファイルに保存
		var json = JsonConvert.SerializeObject(listInstance, Formatting.Indented);
		var filePath = Path.Combine(Application.dataPath, classType.Name + ".json");
		await File.WriteAllTextAsync(filePath, json);
		Debug.Log("Saved: " + filePath);
	}

	void LoadData()
	{
		var classes = ClassLoader.LoadAllClasses("Tables"); // Replace 'YourNamespace' with the actual namespace
		// Implement the logic to deserialize and load these classes from JSON
	}
}