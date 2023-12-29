using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SQLiteDataBase
{
    [Serializable]
    [FilePath("ProjectSettings/SQLiteDataBaseSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public class SQLiteDataBaseSettings : ScriptableSingleton<SQLiteDataBaseSettings>
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
        
        
        public void Save()
        {
            Save(true);
        }
    }

    [CustomEditor(typeof(SQLiteDataBaseSettings))]
    public class FbxPrefabCreateSettingEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var actionUser = (SQLiteDataBaseSettings)target;

            EditorGUILayout.LabelField("SQLiteDB保存先", EditorStyles.boldLabel);
            var sqlData = EditorGUILayout.ObjectField("Path Pattern", actionUser.SQLData, typeof(Object), false);
            actionUser.SQLData = sqlData;
            GUILayout.Space(10);

            EditorGUILayout.LabelField("SQLite Json保存先", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Selected Path:", actionUser.JsonPath);
            if (GUILayout.Button("Select Folder",GUILayout.Width(100), GUILayout.Height(20)))
            {
                actionUser.JsonPath = SelectFolderPath(actionUser.JsonPath);
            }
            GUILayout.Space(10);

            EditorGUILayout.LabelField("MasterMemory Byte保存先", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Selected Path:", actionUser.MasterPath);
            if (GUILayout.Button("Select Folder",GUILayout.Width(100), GUILayout.Height(20)))
            {
                actionUser.MasterPath = SelectFolderPath(actionUser.MasterPath);
            }
            GUILayout.Space(10);
            
            var masterName = EditorGUILayout.TextField("MasterMemory Name", actionUser.MasterName);
            actionUser.MasterName = masterName;
 
            // Force Unity to save the changes
            if (!GUI.changed)
            {
                return;
            }

            EditorUtility.SetDirty(actionUser);
            AssetDatabase.SaveAssets();
        }
    
        private static string SelectFolderPath(string setting)
        {
            var selectedPath = EditorUtility.OpenFolderPanel("Select a folder", setting, "");
            return string.IsNullOrEmpty(selectedPath) ? "" : selectedPath;
        }
    }
}