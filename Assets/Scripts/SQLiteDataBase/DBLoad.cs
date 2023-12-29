using UnityEngine;
using VContainer;

public class DBLoad : MonoBehaviour
{
    [Inject]
    private TestTableRepository repository;
    
    private async void Start()
    { 
        var data = await repository.GetDataAsync(1);
        Debug.Log(data.TextVal);
    }
}
