using UnityEngine;
using VContainer;

public class DBLoad : MonoBehaviour
{
    [Inject]
    private ITableRepository repository;
    
    private async void Start()
    { 
        var data = await repository.GetDataAsync(1);
        Debug.Log(data.TextVal);
    }
}
