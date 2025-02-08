using System.Collections;
using UnityEngine;

public class DecalFader : MonoBehaviour
{
    // フェードアウトにかける秒数（初期値は 3秒）
    private float fadeOutDuration = 3.0f;
    private Renderer decalRenderer;
    private Color originalColor;
    private bool isFading = false;

    /// <summary>
    /// フェードアウトにかける秒数を設定し、フェード処理を開始する
    /// </summary>
    /// <param name="duration">フェードアウト時間（秒）</param>
    public void SetFadeOutDuration(float duration)
    {
        fadeOutDuration = duration;
        if (!isFading)
        {
            StartFadeOut();
        }
    }

    private void StartFadeOut()
    {
        decalRenderer = GetComponent<Renderer>();
        if (decalRenderer == null)
        {
            Debug.LogError("DecalFader には Renderer コンポーネントが必要です。");
            return;
        }
        // マテリアルをインスタンス化（共有マテリアルへの影響を防ぐ）
        decalRenderer.material = new Material(decalRenderer.material);
        originalColor = decalRenderer.material.color;
        StartCoroutine(FadeOutCoroutine());
        isFading = true;
    }

    private IEnumerator FadeOutCoroutine()
    {
        float elapsed = 0f;
        Material mat = decalRenderer.material;

        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeOutDuration;
            // 元のアルファ値から 0 へリニアに補間
            float newAlpha = Mathf.Lerp(originalColor.a, 0f, t);
            Color newColor = mat.color;
            newColor.a = newAlpha;
            mat.color = newColor;
            yield return null;
        }

        // 完全にフェードアウトしたらオブジェクトを削除
        Destroy(gameObject);
    }
}
