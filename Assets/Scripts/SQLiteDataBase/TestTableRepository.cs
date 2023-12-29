using System;
using Cysharp.Threading.Tasks;
using VContainer;

public class TestTableRepository:IDisposable
{
#if Connection_MasterMemory // プリプロセッサディレクティブで使いわけしたりデータキャッシュできる仕組みにできるとさらに良さそう
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
        var path = Application.dataPath + "/db/testdb";
        var db = new SQLiteAsyncConnection(path);
        data = await db.GetAsync<TestTable> (1);
#endif
       return data;
    }

    public void Dispose()
    {
    }
}