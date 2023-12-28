using UnityEngine;

public class DBLoad : MonoBehaviour
{
    // Start is called before the first frame update
    private async void Start()
    {
        var data = await TestTableRepository.GetDataAsync(1);
        Debug.Log(data.TextVal);
    }
}
