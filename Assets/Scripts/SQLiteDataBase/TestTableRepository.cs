using Cysharp.Threading.Tasks;
using SQLiteDataBase;

#if Connection_MasterMemory
using System;
using VContainer;
#else
using SQLite;
using Tables;
#endif

public class TestTableRepository
{
#if Connection_MasterMemory 
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
#if Connection_MasterMemory // プリプロセッサディレクティブで使いわけしたりデータキャッシュできる仕組みにできるとさらに良さそう
        data = _master.DB.MasterMemoryTestTableTable.FindById(index);
#else
        var path = "";
#if UNITY_EDITOR
        path = SQLiteDataBaseSettings.instance.MasterPath + "/" + SQLiteDataBaseSettings.instance.MasterName;
#endif
        var db = new SQLiteAsyncConnection(path);
        data = await db.GetAsync<TestTable> (index);
#endif
       return data;
    }
}