using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EffectsController : MonoBehaviour
{
    // 複数のセリフを格納する配列（台詞は3つ）
    [SerializeField] private string[] dialogues = new string[]
    {
        "痛い！",
        "こんなに食らったら\n糖分過多だよぉ...",
        "しっかりよけてー！"
    };

    // セリフを表示するUI Text（Canvas上に配置したTextオブジェクト）
    [SerializeField] private Text dialogueText;

    // オブジェクトの元々の大きさを記録する変数
    private Vector3 originalScale;

    private void Awake()
    {
        // このスクリプトがアタッチされているオブジェクトの元のスケールを記録
        originalScale = transform.localScale;
    }
    void Start()
    {
        
    }

    // DamageVoid()が呼ばれると、ランダムなセリフの表示、オブジェクトの表示→縮小、拡大縮小のアニメーションを実行する
    public void DamageVoid()
    {
        ShowRandomDialogue();
        ShowAndShrink(gameObject);
        StartScaleAnimation(gameObject);
    }

    // --------------------------
    // ① ランダムなセリフを表示する関数
    // --------------------------
    public void ShowRandomDialogue()
    {
        if (dialogues == null || dialogues.Length == 0 || dialogueText == null)
        {
            Debug.LogWarning("セリフまたはTextが設定されていません");
            return;
        }

        // 配列からランダムにセリフを選択
        int randomIndex = Random.Range(0, dialogues.Length);
        dialogueText.text = dialogues[randomIndex];
    }

    // --------------------------
    // ② オブジェクトを表示し、5秒後に元の比率を保ったまま徐々に縮小して非表示にする関数
    // --------------------------
    public void ShowAndShrink(GameObject target)
    {
        if (target == null)
        {
            Debug.LogWarning("対象のオブジェクトが設定されていません");
            return;
        }

        // オブジェクトを表示
        target.SetActive(true);
        // 元の大きさにリセット
        target.transform.localScale = originalScale;

        // 5秒後から1秒かけて元のスケールの10%に縮小する
        // このとき、元々の比率は保たれます
        target.transform.DOScale(originalScale * 0.1f, 1f)
            .SetDelay(5f)
            .OnComplete(() => target.SetActive(false));
    }

    // --------------------------
    // ③ 子要素は無視して、対象のオブジェクトだけが元の比率を保ったまま拡大縮小を繰り返す関数
    // --------------------------
    public void StartScaleAnimation(GameObject target)
    {
        if (target == null)
        {
            Debug.LogWarning("対象のオブジェクトが設定されていません");
            return;
            
        }

        // 0.5秒かけて元のスケールの120%に拡大し、その後0.5秒で元の大きさに戻る（Yoyoループ）
        target.transform.DOScale(originalScale * 1.2f, 0.5f)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
