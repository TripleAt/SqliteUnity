using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Cysharp.Threading.Tasks;
using MasterMemory;
using MessagePack;
using MessagePack.Resolvers;
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

		if (GUILayout.Button("SQLiteToMasterMemory"))
		{
			ImportMasterMemory().Forget();
		}
	}

	private static async UniTask ImportMasterMemory()
	{
		InitializeMessagePack();

		var tableClasses = ClassLoader.LoadAllClasses("Tables");
		var masterClasses = ClassLoader.LoadAllClasses("Master");
		var path = Application.dataPath + "/db/testdb";
		var db = new SQLiteAsyncConnection(path);
		var databaseBuilder = new DatabaseBuilder();

		foreach (var tableType in tableClasses)
		{
			foreach (var masterType in masterClasses)
			{
				// 共通のインターフェースを持つ場合のみ続行
				var commonInterfaces = tableType.GetInterfaces().Intersect(masterType.GetInterfaces());
				if (!commonInterfaces.Any())
				{
					continue;
				}

				var sqliteData = await GetSQLiteValue(tableType, db);
				var listType = typeof(List<>).MakeGenericType(masterType);
				var listInstance = Activator.CreateInstance(listType);

				// 内容をコピーして再度Listに詰める(再定義されたListに詰める作業いらんかもしれん)
				foreach (var item in sqliteData)
				{
					var masterMemoryInstance = Activator.CreateInstance(masterType);
					ClassLoader.DynamicCopyPropertiesWithCommonInterface(item, masterMemoryInstance, commonInterfaces.First());
					listType.GetMethod("Add")?.Invoke(listInstance, new[] { masterMemoryInstance });
				}

				// 基底クラスに合うメソッドを探してくる
				var appendMethod = FindAppendMethod(databaseBuilder.GetType(), listType);
				if (appendMethod != null)
				{
					appendMethod.Invoke(databaseBuilder, new[] { listInstance });
				}
			}
		}

		SaveBinaryData(databaseBuilder);
	}
	
	private static void InitializeMessagePack()
	{
		var messagePackResolvers = CompositeResolver.Create(
			MasterMemoryResolver.Instance,
			GeneratedResolver.Instance,
			StandardResolver.Instance
		);
		var options = MessagePackSerializerOptions.Standard.WithResolver(messagePackResolvers);
		MessagePackSerializer.DefaultOptions = options;
	}
	
	private static void SaveBinaryData(DatabaseBuilderBase databaseBuilder)
	{
		var binary = databaseBuilder.Build();
		const string binaryPath = "Assets/Master/Binary/Master.bytes";
		var directory = Path.GetDirectoryName(binaryPath);
		if (!Directory.Exists(directory) && directory != null)
		{
			Directory.CreateDirectory(directory);
		}
		File.WriteAllBytes(binaryPath, binary);
		AssetDatabase.Refresh();
	}
	
	private static void SaveData()
	{
		var classes = ClassLoader.LoadAllClasses("Tables"); 
		var path = Application.dataPath+"/db/testdb";  // テーブルの格納先変えました
		var db = new SQLiteAsyncConnection(path);
		foreach (var classType in classes)
		{
			SaveClass(classType, db).Forget();
		}
	}

	private static void LoadData()
	{
		var classes = ClassLoader.LoadAllClasses("Tables"); 

		var path = Application.dataPath+"/db/testdb";  // テーブルの格納先変えました
		var db = new SQLiteAsyncConnection(path);
		foreach (var classType in classes)
		{
			LoadClass(classType, db).Forget();
		}
	}

	private static async UniTask SaveClass(Type classType, SQLiteAsyncConnection db)
	{
		var listInstance = await GetSQLiteValue(classType, db);
		if (listInstance == null)
		{
			return;
		}

		// JSONにシリアライズしてファイルに保存
		var json = JsonConvert.SerializeObject(listInstance, Formatting.Indented);
		var filePath = Path.Combine(Application.dataPath, classType.Name + ".json");
		await File.WriteAllTextAsync(filePath, json);
		Debug.Log("Saved: " + filePath);
	}

	private static MethodInfo FindAppendMethod(Type databaseBuilderType, Type listType)
	{
		foreach (var method in databaseBuilderType.GetMethods())
		{
			if (method.Name != "Append" || method.GetParameters().Length != 1)
			{
				continue;
			}
			var parameterType = method.GetParameters()[0].ParameterType;
			if (parameterType.IsAssignableFrom(listType))
			{
				return method;
			}
		}
		return null;
	}
	
	private static async UniTask<dynamic> GetSQLiteValue(Type classType, SQLiteAsyncConnection db)
	{
		// AsyncTableQuery<T> オブジェクトを取得
		var tableMethod = db.GetType().GetMethod("Table")?.MakeGenericMethod(classType);
		if (tableMethod == null)
		{
			return null;
		}

		var tableInstance = tableMethod.Invoke(db, null);
		var toListAsyncMethod = tableInstance.GetType().GetMethod("ToListAsync");
		if (toListAsyncMethod == null)
		{
			return null;
		}

		// UniTask から List<T> を非同期的に取得
		dynamic task = toListAsyncMethod.Invoke(tableInstance, null);
		return await task;
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