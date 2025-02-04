using System.Collections;
using UnityEngine;

public class FadeAndDestroyChildren : MonoBehaviour
{
    // フェードアウトにかける秒数（Inspectorから変更可能）
    [SerializeField]
    private float fadeDuration = 5.0f;

    /// <summary>
    /// 子オブジェクトのフェードアウトと削除を開始します。
    /// 任意のタイミングでこのメソッドを呼び出してください。
    /// </summary>
    /// 
    public void Update()
    {
        FadeOutChildren();
    }
    public void FadeOutChildren()
    {
        StartCoroutine(FadeOutChildrenCoroutine());
    }

    /// <summary>
    /// 子オブジェクトのRendererのマテリアルの色のアルファ値を
    /// fadeDuration秒かけて 1→0 にリニアに補間し、フェードアウト完了後に削除します。
    /// </summary>
    private IEnumerator FadeOutChildrenCoroutine()
    {
        // 現在の子オブジェクトの参照を配列に格納しておく
        int childCount = transform.childCount;
        Transform[] childTransforms = new Transform[childCount];
        for (int i = 0; i < childCount; i++)
        {
            childTransforms[i] = transform.GetChild(i);
        }

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            // アルファ値を 1 から 0 へリニアに変化させる
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);

            // 各子オブジェクトのRendererコンポーネントを取得し、マテリアルのアルファ値を更新
            foreach (Transform child in childTransforms)
            {
                if (child == null) continue;

                Renderer rend = child.GetComponent<Renderer>();
                if (rend != null)
                {
                    // renderer.material を使うと、インスタンス化されたマテリアルが返されるため安全です
                    Material mat = rend.material;
                    if (mat.HasProperty("_Color"))
                    {
                        Color col = mat.color;
                        col.a = alpha;
                        mat.color = col;
                    }
                }
            }

            yield return null;
        }

        // フェードアウト完了後、全ての子オブジェクトを削除
        foreach (Transform child in childTransforms)
        {
            if(child != null)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
