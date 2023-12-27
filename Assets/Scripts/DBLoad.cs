using SQLite;
using Tables;
using UnityEngine;

public class DBLoad : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        const string path = "C://Test/Testdb";
        var db = new SQLiteAsyncConnection(path);
        var test = await db.GetAsync<TestTable> (1);
        Debug.Log(test.TextVal);
    }
}
