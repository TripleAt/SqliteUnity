using Cysharp.Threading.Tasks;
using VContainer;

#if Connection_MasterMemory || !UNITY_EDITOR
using VContainer;
#else
using SQLiteDataBase;
using SQLite;
using Tables;
using UnityEditor;
#endif

public class TestTableRepository:ITableRepository
{
    private readonly MasterMemoryData _master;
    
    [Inject]
    public TestTableRepository(MasterMemoryData masterMemoryData)
    {
        _master = masterMemoryData;
    }
    

    public async UniTask<ITestTable> GetDataAsync(int index)
    {
        ITestTable data;
        data = _master.DB.MasterMemoryTestTableTable.FindById(index);
       return data;
    }
}