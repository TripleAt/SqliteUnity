// -----------------------------------------------------------------------
// <copyright file="DefineDirectiveEditor.cs" company="None">
// <summary>Define Directive Editor</summary>
// <remarks>デファインを付け足したりするエディタ機能.</remarks>
// Created by やまだたいし
// </copyright>
// -----------------------------------------------------------------------

#if UNITY_EDITOR
namespace DefineDirective.Editor
{
	using System.Collections.Generic;
	using System.Linq;
	using UnityEditor;
	using UnityEngine;

	public class DefineDirectiveEditor : EditorWindow
	{
		private string _customDefine = string.Empty;
		private Dictionary<string, bool> _customDefines = new ();
		private Dictionary<string, bool> _originalDefines = new ();
		private bool _shouldApply;

		[MenuItem("Yamada Tools/Define Directive Editor")]
		public static void ShowWindow()
		{
			GetWindow<DefineDirectiveEditor>("Define Directive Editor");
		}

		private void OnEnable()
		{
			LoadCustomDefines();
			_originalDefines = new Dictionary<string, bool>(_customDefines);
		}

		private void OnGUI()
		{
			GUILayout.Label("Custom Define Directive", EditorStyles.boldLabel);

			_customDefine = EditorGUILayout.TextField("Define:", _customDefine);

			if (GUILayout.Button("Add Define"))
			{
				AddDefine(_customDefine);
				_shouldApply = true;
			}

			EditorGUILayout.Space();

			GUILayout.Label("Custom Defines", EditorStyles.boldLabel);

			var keysToRemove = new List<string>();
			var labelStyle = new GUIStyle(GUI.skin.label)
			{
				wordWrap = true
			};

			foreach (var define in _customDefines.Keys.ToList())
			{
				EditorGUILayout.BeginHorizontal();
				var originalValue = _customDefines[define];
				// 可変長のラベルでトグルを表示
				_customDefines[define] = EditorGUILayout.ToggleLeft(new GUIContent(define), _customDefines[define], labelStyle);
				
				if (originalValue != _customDefines[define])
				{
					_shouldApply = true;
				}

				if (GUILayout.Button("Remove"))
				{
					keysToRemove.Add(define);
				}

				EditorGUILayout.EndHorizontal();
			}

			foreach (var keyToRemove in keysToRemove)
			{
				RemoveDefine(keyToRemove);
				_shouldApply = true;
			}

			EditorGUI.BeginDisabledGroup(EditorApplication.isCompiling || !_shouldApply);
			if (GUILayout.Button("Apply"))
			{
				ApplyCustomDefines();
				LoadCustomDefines();
				_originalDefines = new Dictionary<string, bool>(_customDefines);
				_shouldApply = false;
			}

			EditorGUI.EndDisabledGroup();

			EditorGUI.BeginDisabledGroup(EditorApplication.isCompiling);
			if (GUILayout.Button("Revert"))
			{
				_customDefines = new Dictionary<string, bool>(_originalDefines);
				ApplyCustomDefines();
				LoadCustomDefines();
				_shouldApply = false;
			}

			EditorGUI.EndDisabledGroup();
		}

		private void LoadCustomDefines()
		{
			_customDefines.Clear();
			var buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
			var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
			var defineList = defines.Split(';');

			foreach (var define in defineList)
			{
				if (!string.IsNullOrEmpty(define))
				{
					_customDefines.Add(define, true);
				}
			}
		}

		private void AddDefine(string define)
		{
			_customDefines.TryAdd(define, true);
		}

		private void RemoveDefine(string define)
		{
			if (_customDefines.ContainsKey(define))
			{
				_customDefines.Remove(define);
			}
		}

		private void ApplyCustomDefines()
		{
			var buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
			var defines = string.Empty;

			foreach (var define in _customDefines.Where(define => define.Value))
			{
				if (string.IsNullOrEmpty(defines))
				{
					defines = define.Key;
				}
				else
				{
					defines += ";" + define.Key;
				}
			}

			PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defines);
		}
	}
}
#endif