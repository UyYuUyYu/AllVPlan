using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class SubscribePopup : MonoBehaviour
{
    [SerializeField] Text messageNameText,moneyText;         // 表示するメッセージ

    [SerializeField] float fadeDuration = 0.5f;           // フェードイン/アウト時間
    [SerializeField] float displayDuration = 2.5f;        // 表示し続ける時間
    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
    }

    public void Initialize(string _message,int _money)
    {
        messageNameText.text = _message;
        moneyText.text=$"¥{_money}";
        StartCoroutine(ShowPopup());
    }

    private IEnumerator ShowPopup()
    {
        // フェードイン
        yield return StartCoroutine(Fade(1f, 1f, fadeDuration));

        // 表示維持
        yield return new WaitForSeconds(displayDuration);

        // フェードアウト
        yield return StartCoroutine(Fade(1f, 0f, fadeDuration));

        Destroy(gameObject); // 自動削除
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = to;
    }
}
