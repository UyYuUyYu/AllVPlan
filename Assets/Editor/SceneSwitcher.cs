using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace UnityToolbarExtender.Examples
{
	static class ToolbarStyles
	{
		public static readonly GUIStyle commandButtonStyle;

		static ToolbarStyles()
		{
			commandButtonStyle = new GUIStyle("Command")
			{
				fontSize = 8,
				alignment = TextAnchor.MiddleCenter,
				imagePosition = ImagePosition.ImageAbove,
				fontStyle = FontStyle.Bold
			};
		}
	}

	[InitializeOnLoad]
	public class SceneSwitchLeftButton
	{
		static SceneSwitchLeftButton()
		{
			ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
		}

		static void OnToolbarGUI()
		{
			GUILayout.FlexibleSpace();


			// if (GUILayout.Button(new GUIContent("Talk", "会話シーン"), ToolbarStyles.commandButtonStyle))
			// {
			// 	SceneHelper.StartScene("talk1");
			// }
			if (GUILayout.Button(new GUIContent("Title", "タイトル"), ToolbarStyles.commandButtonStyle))
			{
				SceneHelper.StartScene("Title");
			}
			if (GUILayout.Button(new GUIContent("GDtalk", "試遊版シーン"), ToolbarStyles.commandButtonStyle))
			{
				SceneHelper.StartScene("Talk_test");
			}
			if (GUILayout.Button(new GUIContent("Game", "ゲーム画面"), ToolbarStyles.commandButtonStyle))
			{
				SceneHelper.StartScene("PlayGame");
			}
			// if (GUILayout.Button(new GUIContent("Select", "ステージ選択"), ToolbarStyles.commandButtonStyle))
			// {
			// 	SceneHelper.StartScene("selectstage");
			// }

			// if (GUILayout.Button(new GUIContent("Tutorial", "チュートリアル"), ToolbarStyles.commandButtonStyle))
			// {
			// 	SceneHelper.StartScene("Tutorial");
			// }
			if (GUILayout.Button(new GUIContent("End", "戦闘後会話シーン"), ToolbarStyles.commandButtonStyle))
			{
				SceneHelper.StartScene("talk2");
			}
			if (GUILayout.Button(new GUIContent("load", "loading"), ToolbarStyles.commandButtonStyle))
			{
				SceneHelper.StartScene("loading");
			}
		}
	}

	static class SceneHelper
	{
		static string sceneToOpen;

		public static void StartScene(string sceneName)
		{
			if (EditorApplication.isPlaying)
			{
				EditorApplication.isPlaying = false;
			}

			sceneToOpen = sceneName;
			EditorApplication.update += OnUpdate;
		}

		static void OnUpdate()
		{
			if (sceneToOpen == null ||
				EditorApplication.isPlaying || EditorApplication.isPaused ||
				EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode)
			{
				return;
			}

			EditorApplication.update -= OnUpdate;

			if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
			{
				// need to get scene via search because the path to the scene
				// file contains the package version so it'll change over time
				string[] guids = AssetDatabase.FindAssets("t:scene " + sceneToOpen, null);
				if (guids.Length == 0)
				{
					Debug.LogWarning("Couldn't find scene file");
				}
				else
				{
					string scenePath = AssetDatabase.GUIDToAssetPath(guids[0]);
					EditorSceneManager.OpenScene(scenePath);
					//	EditorApplication.isPlaying = true;
				}
			}
			sceneToOpen = null;
		}
	}
}