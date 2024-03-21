using Cysharp.Threading.Tasks;
using SQLite;
using Tables;
using UnityEditor;


using SQLiteDataBase;
using SQLite;
using Tables;
using UnityEditor;

public class TestMasterRepository:ITableRepository
{
	private static SQLiteDataBaseSettings _settings;

	public TestMasterRepository(SQLiteDataBaseSettings settings)
	{
		_settings = settings;
	}
    

    public async UniTask<ITestTable> GetDataAsync(int index)
    {
        ITestTable data;
        var path = "";
        path = AssetDatabase.GetAssetPath(_settings.SQLData);
        var db = new SQLiteAsyncConnection(path);
        data = await db.GetAsync<TestTable> (index);
       return data;
    }
}