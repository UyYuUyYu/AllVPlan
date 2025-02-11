using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeImageAndDestroy : MonoBehaviour
{
    [Tooltip("フェードアウトにかける時間（秒）")]
    public float fadeDuration = 2f;

    private Image img;

    void Awake()
    {
        // Imageコンポーネントを取得
        img = GetComponent<Image>();
    }

    void OnEnable()
    {
        if (img != null)
        {
            // 前回のTweenが残っている場合は停止
            img.DOKill();

            // オブジェクトが有効になったタイミングでアルファを1にリセット
            Color color = img.color;
            color.a = 1f;
            img.color = color;

            // fadeDuration秒かけてアルファ値を0にフェードアウトし、完了時にSetActive(false)する
            img.DOFade(0f, fadeDuration).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
        else
        {
            Debug.LogWarning("Imageコンポーネントが見つかりません");
        }
    }
}
