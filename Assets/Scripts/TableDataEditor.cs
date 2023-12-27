using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using SQLite;
using UnityEditor;
using UnityEngine;

public class TableDataEditor : EditorWindow
{
	[MenuItem("Tools/Table Data Editor")]
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

	private static void SaveData()
	{
		var classes = ClassLoader.LoadAllClasses("Tables"); // Replace 'YourNamespace' with the actual namespace

		var path = Application.dataPath+"/db/testdb";  // テーブルの格納先変えました
		var db = new SQLiteAsyncConnection(path);
		foreach (var classType in classes)
		{
			SaveClass(classType, db).Forget();
		}
	}

	private static void LoadData()
	{
		var classes = ClassLoader.LoadAllClasses("Tables"); // Replace 'YourNamespace' with the actual namespace

		var path = Application.dataPath+"/db/testdb";  // テーブルの格納先変えました
		var db = new SQLiteAsyncConnection(path);
		foreach (var classType in classes)
		{
			LoadClass(classType, db).Forget();
		}
	}

	private static async UniTask SaveClass(Type classType, SQLiteAsyncConnection db)
	{
		// AsyncTableQuery<T> オブジェクトを取得
		var tableMethod = db.GetType().GetMethod("Table")?.MakeGenericMethod(classType);
		if (tableMethod == null)
		{
			return;
		}

		var tableInstance = tableMethod.Invoke(db, null);
		var toListAsyncMethod = tableInstance.GetType().GetMethod("ToListAsync");
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
	
	private static async UniTask LoadClass(Type classType, SQLiteAsyncConnection db)
	{
		var filePath = Path.Combine(Application.dataPath, classType.Name + ".json");
		if (!File.Exists(filePath))
		{
			Debug.Log("File not found: " + filePath);
			return;
		}

		try
		{
			// テーブルが存在しない場合は作成
			await db.CreateTableAsync(classType);

			var json = await File.ReadAllTextAsync(filePath);
			var listType = typeof(List<>).MakeGenericType(classType);
			var listInstance = JsonConvert.DeserializeObject(json, listType) as System.Collections.IList;

			if (listInstance != null)
			{
				foreach (var item in listInstance)
				{
					await db.InsertAsync(item);
				}
				Debug.Log("Data loaded and inserted into database.");
			}
		}
		catch (Exception e)
		{
			Debug.LogError("Error loading JSON from file or inserting into database: " + e.Message);
		}
	}
}