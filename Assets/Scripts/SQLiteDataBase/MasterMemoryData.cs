
using System;
using System.Text.RegularExpressions;
using MasterMemory;
using SQLiteDataBase;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer;

public class MasterMemoryData : IDisposable
{
    private MemoryDatabase _db;
    private SQLiteDataBaseSettings _settings;
    private AsyncOperationHandle<byte[]> _op;

    [Inject]
    public MasterMemoryData(SQLiteDataBaseSettings settings)
    {
        _settings = settings;
    }

    public MemoryDatabase DB
    {
        get
        {
            if (_db == null)
            {
                DownloadMasterData();
            }
            return _db;
        }
    }

    private void DownloadMasterData()
    {
        const string pattern = "Assets.*$";
        var match = Regex.Match(_settings.MasterPath + "/" + _settings.MasterName, pattern);
        if (!match.Success)
        {
            return;
        }

        var data = LoadBinaryData(match.Value);
        if (data != null)
        {
            _db = new MemoryDatabase(data);
        }
    }
    
    // Note:実際はAddressablesとかで読んでくると良さそう.
    private byte[] LoadBinaryData(string binaryPath)
    {
        _op = Addressables.LoadAssetAsync<byte[]>(binaryPath);
        
        var data = _op.WaitForCompletion();
        if (data == null)
        {
            Debug.LogError("ファイルが見つかりません: " + binaryPath);
        }
        return data;
    }

    public void Dispose()
    {
        Addressables.Release(_op);
    }
}