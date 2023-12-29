
using System.IO;
using MasterMemory;
using UnityEngine;

public class MasterMemoryData
{
    private MemoryDatabase _db;

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
        const string binaryPath = "Assets/Master/Binary/Master.bytes";

        var data = LoadBinaryData(binaryPath);
        if (data != null)
        {
            _db = new MemoryDatabase(data);
        }
    }
    
    // Note:実際はAddressablesとかで読んでくると良さそう.
    private static byte[] LoadBinaryData(string binaryPath)
    {
        if (File.Exists(binaryPath))
        {
            var binary = File.ReadAllBytes(binaryPath);
            return binary;
        }

        Debug.LogError("ファイルが見つかりません: " + binaryPath);

        return null;
    }
}