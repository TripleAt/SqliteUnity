
using System.IO;
using MasterMemory;
using UnityEngine;

public class MasterMemoryData
{
    private MemoryDatabase _db;
    private string _binaryPath = "Assets/Master/Binary/Master.bytes";

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

    public void DownloadMasterData(string binaryPath = "")
    {
        if (binaryPath.Equals(string.Empty))
        {
            _binaryPath = binaryPath;
        }
        var data = LoadBinaryData(_binaryPath);
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