using UnityEngine;
using UnityEngine.SceneManagement;

static class SceneSwitchInit {
#if UNITY_EDITOR
	[UnityEditor.InitializeOnLoadMethod]
#endif
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad),]
	static void Init() {
		SceneManager.sceneLoaded += SceneSwitcher.Init1;
	}
}

class SceneSwitcher : MonoBehaviour {
	public Rect WindowRect;
	Rect _windowRect0;
	string _sceneLabel;

	int _windowId;
	bool _dirty;
	static bool _initialized;
	float _dpiInv;
	const string Title = "Scene Switcher";

	internal static void Init1(Scene cur, LoadSceneMode _) {
		SceneManager.sceneLoaded -= Init1;
		if (_initialized)
			return;
		_initialized = true;

		var go = new GameObject(Title) {
			// hideFlags = HideFlags.HideInHierarchy
		};
		DontDestroyOnLoad(go);
		var ss = go.AddComponent<SceneSwitcher>();
		ss._windowId = GUIUtility.GetControlID(FocusType.Keyboard);
		ss._windowRect0 = ss.WindowRect = new Rect(0, 0, 40, 51);
		SceneManager.sceneLoaded += ss.Init2;
		ss.Init2(cur, _);
	}

	void OnDestroy() {
		SceneManager.sceneLoaded -= Init2;
	}

	void Init2(Scene activeScene, LoadSceneMode _) {
		_sceneLabel = string.Format("{0}:{1}", activeScene.buildIndex, activeScene.name);
		_dpiInv = 1f / Screen.dpi / 1.5f;
		if (Screen.dpi == 264f) // not editor GameView
			_dpiInv /= 1.5f;
		_dirty = true;
	}

	const float MinWidth = 120f;
	const float BaseWidth = 40f;

	void OnGUI() {
		if (_dirty) {
			_dirty = false;
			var labelSize = GUI.skin.label.CalcSize(new GUIContent {text = _sceneLabel});
			
			WindowRect.width = Mathf.Max(MinWidth, BaseWidth + labelSize.x);
			WindowRect.height = _windowRect0.height;
		}

#if !UNITY_EDITOR
        ScaleGUI();
#endif

		WindowRect = GUILayout.Window(_windowId, WindowRect, InnerGui, Title);
	}

	void ScaleGUI() {
		var max = Mathf.Max(Screen.width, Screen.height);
		var ratio = max * _dpiInv;
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(ratio, ratio, 1));
	}

	void InnerGui(int _) {
		using (new GUILayout.HorizontalScope()) {
			var activeScene = SceneManager.GetActiveScene();
			var valid = SceneManager.sceneCountInBuildSettings > activeScene.buildIndex && SceneManager.GetSceneByBuildIndex(activeScene.buildIndex) == activeScene;
			var buildIndex = valid ? activeScene.buildIndex : -1;
			bool buf;
			int newIndex;


			GUILayout.BeginVertical();
			{
				GUILayout.Label(_sceneLabel);
				GUILayout.BeginHorizontal();
				{
					buf = GUI.enabled;
					newIndex = buildIndex - 1;
					//GUI.enabled = newIndex >= 0;
					if (newIndex < 0) {
						newIndex = SceneManager.sceneCountInBuildSettings - 1;
					}
					if (GUILayout.Button(" < "))
						SceneManager.LoadScene(newIndex);

					GUI.enabled = buf;

					buf = GUI.enabled;
					newIndex = buildIndex + 1;
					//GUI.enabled = newIndex <= SceneManager.sceneCountInBuildSettings - 1;
					if (newIndex > SceneManager.sceneCountInBuildSettings - 1) {
						newIndex = 0;
					}
					if (GUILayout.Button(" > "))
						SceneManager.LoadScene(newIndex);

					GUI.enabled = buf;
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndVertical();
		}

		GUI.DragWindow();
	}
}