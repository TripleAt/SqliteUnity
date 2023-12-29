
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace SQLiteDataBase
{
    public class SqLiteDataBaseSettingsProvider: SettingsProvider
    {
        // Projectウィンドウでの表示位置
        private const string SettingsPath = "Project/SQLiteDataBaseSettings";
        private const string FilePath = "Assets/Settings/SQLiteDataBaseSettings.asset";
        private Editor _editor;
        private SQLiteDataBaseSettings preferences;

        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            return new SqLiteDataBaseSettingsProvider(SettingsPath, SettingsScope.Project);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqLiteDataBaseSettingsProvider"/> class.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="scopes"></param>
        /// <param name="keywords"></param>
        private SqLiteDataBaseSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null)
            : base(path, scopes, keywords)
        {
        }

        /// <inheritdoc/>
        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            preferences = AssetDatabase.LoadAssetAtPath<SQLiteDataBaseSettings>(FilePath);
            if (preferences == null)
            {
                // 設定ファイルが存在しない場合、新しく作成
                preferences = ScriptableObject.CreateInstance<SQLiteDataBaseSettings>();
                AssetDatabase.CreateAsset(preferences, FilePath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            // 設定ファイルの標準のインスペクターのエディタを生成
            Editor.CreateCachedEditor(preferences, null, ref _editor);
        }

        /// <inheritdoc/>
        public override void OnGUI(string searchContext)
        {
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                // 設定ファイルの標準のインスペクターを表示
                _editor.OnInspectorGUI();
                if (check.changed)
                {
                    EditorUtility.SetDirty(_editor.target);
                    AssetDatabase.SaveAssets();
                }
            }
        }
    }
}