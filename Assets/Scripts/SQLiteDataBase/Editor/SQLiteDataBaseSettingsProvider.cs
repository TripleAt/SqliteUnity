
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
        private Editor _editor;

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
            var preferences = SQLiteDataBaseSettings.instance;
            preferences.hideFlags = HideFlags.HideAndDontSave & ~HideFlags.NotEditable; // ScriptableSingletonを編集可能にする

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
                    // 差分があったら保存
                    SQLiteDataBaseSettings.instance.Save();
                }
            }
        }
    }
}