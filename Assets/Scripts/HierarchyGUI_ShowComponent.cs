using UnityEngine;
using UnityEditor;
public static class HierarchyGUI_ShowComponent
{
    private const int ICON_SIZE = 16;
    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnGUI;
    }
    private static void OnGUI(int instanceID, Rect selectionRect)
    {
        // instanceID をオブジェクト参照に変換
        var go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (go == null)
        {
            return;
        }
        // オブジェクトが所持しているコンポーネント一覧を取得
        var components = go.GetComponents<Component>();
        if (components.Length == 0)
        {
            return;
        }
        selectionRect.x = selectionRect.xMax - ICON_SIZE * components.Length;
        selectionRect.width = ICON_SIZE;
        foreach (var component in components)
        {
            // コンポーネントのアイコン画像を取得
            var texture2D = AssetPreview.GetMiniThumbnail(component);
            GUI.DrawTexture(selectionRect, texture2D);
            selectionRect.x += ICON_SIZE;
        }
    }
}