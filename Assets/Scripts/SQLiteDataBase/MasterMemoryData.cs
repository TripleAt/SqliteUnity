
using System.IO;
using MasterMemory;
using UnityEngine;

public static class MasterMemoryData
{
    private static MemoryDatabase _db;

    // とりあえず、シングルトンで処理(DIにするともっと良いよね)
    public static MemoryDatabase DB
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

    private static void DownloadMasterData()
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