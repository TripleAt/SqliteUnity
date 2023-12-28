using System;
using UnityEditor;
using UnityEngine;

namespace SQLiteDataBase
{
    [Serializable]
    [FilePath("ProjectSettings/SQLiteDataBaseSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public class SQLiteDataBaseSettings : ScriptableSingleton<SQLiteDataBaseSettings>
    {
        [Header("SQLiteDB保存先")]
        [SerializeField]
        public string pathPattern =　@"";

        [Header("SQLite Json保存先")]
        [SerializeField]
        public string pathJsonPattern =　@"";

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
            var pathPattern = EditorGUILayout.TextField("Path Pattern", actionUser.pathPattern);
            actionUser.pathPattern = pathPattern;

            EditorGUILayout.LabelField("SQLite Json保存先", EditorStyles.boldLabel);
            var pathJsonPattern = EditorGUILayout.TextField("Path Pattern", actionUser.pathJsonPattern);
            actionUser.pathJsonPattern = pathJsonPattern;

            // Force Unity to save the changes
            if (!GUI.changed)
            {
                return;
            }

            EditorUtility.SetDirty(actionUser);
            AssetDatabase.SaveAssets();
        }
    }
}