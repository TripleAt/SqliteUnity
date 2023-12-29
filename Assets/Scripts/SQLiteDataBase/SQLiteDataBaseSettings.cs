using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Object = UnityEngine.Object;

namespace SQLiteDataBase
{
    [Serializable]
#if UNITY_EDITOR
    [FilePath("Assets/Settings/SQLiteDataBaseSettings.asset", FilePathAttribute.Location.ProjectFolder)]
#endif
    public class SQLiteDataBaseSettings : ScriptableObject
    {
        [Header("SQLite DB")]
        [SerializeField]
        public Object SQLData;

        [Header("SQLite Json保存先")]
        [SerializeField]
        public string JsonPath =　@"";

        [Header("MasterMemory Byte保存先")]
        [SerializeField]
        public string MasterPath =　@"";
        
        [Header("MasterMemory Byte名")]
        [SerializeField]
        public string MasterName =　@"Master.bytes";
    }

}