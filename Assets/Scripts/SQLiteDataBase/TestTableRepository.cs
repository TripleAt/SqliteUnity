using Cysharp.Threading.Tasks;

#if Connection_MasterMemory || !UNITY_EDITOR
using VContainer;
#else
using SQLiteDataBase;
using SQLite;
using Tables;
using UnityEditor;
#endif

public class TestTableRepository
{
#if Connection_MasterMemory || !UNITY_EDITOR 
    private readonly MasterMemoryData _master;
    
    [Inject]
    public TestTableRepository(MasterMemoryData masterMemoryData)
    {
        _master = masterMemoryData;
    }
#endif

    public async UniTask<ITestTable> GetDataAsync(int index)
    {
        ITestTable data;
#if Connection_MasterMemory || !UNITY_EDITOR
        data = _master.DB.MasterMemoryTestTableTable.FindById(index);
#else
        var path = "";
#if UNITY_EDITOR
        path = AssetDatabase.GetAssetPath(SQLiteDataBaseSettings.instance.SQLData);
#endif
        var db = new SQLiteAsyncConnection(path);
        data = await db.GetAsync<TestTable> (index);
#endif
       return data;
    }
}