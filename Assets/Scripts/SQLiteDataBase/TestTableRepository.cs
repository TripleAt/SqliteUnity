using Cysharp.Threading.Tasks;

public static class TestTableRepository
{
    public static async UniTask<ITestTable> GetDataAsync(int index)
    {
        ITestTable data;
#if true // プリプロセッサディレクティブで使いわけしたりデータキャッシュできる仕組みにできるとさらに良さそう
        data = MasterMemoryData.DB.MasterMemoryTestTableTable.FindById(index);
#else
        var path = Application.dataPath + "/db/testdb";
        var db = new SQLiteAsyncConnection(path);
        data = await db.GetAsync<TestTable> (1);
#endif
        return data;
    }
}