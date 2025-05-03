using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class CommentManager : MonoBehaviour
{
    [Header("▼ コメント表示用の領域 (Canvas内のPanelなど)")]
    public RectTransform commentContainer;

    [Header("▼ コメントのPrefab (子にAvatar,Name,Messageなど)")]
    public GameObject commentPrefab;

    [Header("▼ コメントデータ (ScriptableObject)")]
    public CommentListSO commentListSO;

    [Header("▼ 表示・アニメーション設定")]
    public float baseLineHeight = 50f;
    public float shiftDuration = 0.3f;
    public float removeThreshold = 50f;
    public float spawnInterval = 2f;

    [Header("▼ 文字数に応じた行間設定")]
    public int characterThreshold = 20;
    public float lineHeightShort = 50f;
    public float lineHeightLong = 80f;

    public TextAttackSpawner attackSpawner; // Inspectorでアタッチ
public float attackTextSpeed = 10.0f;    // 速度は固定10


    // 現在表示中のコメントオブジェクト（古いものから順に先頭に格納）
    private List<GameObject> commentObjects = new List<GameObject>();

    // Damage モード中は通常コメントを一時停止するためのフラグ
    private bool isDamageMode = false;
    // Attack モード中も通常コメント生成を停止するためのフラグ
    private bool isAttackMode = false;
    private Coroutine normalCommentCoroutine;
    private int normalIndex = 0; // normalList を順番に参照するためのインデックス

    public void StartComment()
    {
        normalCommentCoroutine = StartCoroutine(NormalCommentFlow());
    }

    /// <summary>
    /// 一定間隔で normalList からコメントを生成（Damage/Attack中は一時停止）
    /// </summary>
    IEnumerator NormalCommentFlow()
    {
        while (true)
        {
            if (!isDamageMode && !isAttackMode)
            {
                yield return new WaitForSeconds(spawnInterval);
                if (!isDamageMode && !isAttackMode)
                {
                    SpawnNextNormalComment();
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    /// <summary>
    /// normalList から順番にコメントを取得し生成
    /// </summary>
    private void SpawnNextNormalComment()
    {
        if (commentListSO == null || commentListSO.normalList.Count == 0) return;

        var data = commentListSO.normalList[normalIndex];
        SpawnComment(data);

        // 次のコメントへ
        normalIndex++;
        if (normalIndex >= commentListSO.normalList.Count)
        {
            normalIndex = 0;
        }
    }

    /// <summary>
    /// ScriptableObject のデータを使ってコメントオブジェクトを生成、下に追加
    /// </summary>
    private void SpawnComment(CommentListSO.CommentData data)
    {
        // 1) コメントの文字数に応じた行間を決定
        float usedLineHeight = (data.message.Length > characterThreshold) ? lineHeightLong : lineHeightShort;

        // 2) 既存コメントを上に usedLineHeight 分押し上げるアニメーション
        ShiftExistingCommentsUp(usedLineHeight);

        // 3) 新しいコメントを生成
        GameObject newComment = Instantiate(commentPrefab, commentContainer);

        Transform avatarTr = newComment.transform.Find("Avatar");
        Transform nameTr = newComment.transform.Find("Name");
        Transform messageTr = newComment.transform.Find("Message");

        if (nameTr != null)
        {
            Text nameText = nameTr.GetComponent<Text>();
            if (nameText != null)
            {
                nameText.text = data.name;
                nameText.color = data.nameColor;
            }
            // 名前が「コエル」のときだけ Avatar を表示
            if (avatarTr != null)
            {
                if (nameText != null && nameText.text == "コエル")
                {
                    Image avatarImage = avatarTr.GetComponent<Image>();
                    if (avatarImage != null)
                    {
                        avatarImage.sprite = data.avatar;
                    }
                    avatarTr.gameObject.SetActive(true);
                }
                else
                {
                    avatarTr.gameObject.SetActive(false);
                }
            }
        }
        if (messageTr != null)
        {
            Text messageText = messageTr.GetComponent<Text>();
            if (messageText != null)
                messageText.text = data.message;
        }

        // 4) 新しいコメントの配置（必要に応じて調整）
        RectTransform newRect = newComment.GetComponent<RectTransform>();
        if (newRect != null)
        {
            newRect.anchoredPosition = new Vector2(0, usedLineHeight - 80);
        }

        // 5) コメントオブジェクトのリストに追加
        commentObjects.Add(newComment);

        // 6) 画面外に押し上げられたコメントを削除
        RemoveOutOfRangeComments();
    int randomPos = Random.Range(0, attackSpawner.GeneratePositionCount);
    Debug.Log("コメント発射");
    attackSpawner.GenerateAttackText(data.message, attackTextSpeed, randomPos);


    }

    /// <summary>
    /// 既存のコメントを上へ shiftAmount 分押し上げる（DOTween アニメーション）
    /// </summary>
    private void ShiftExistingCommentsUp(float shiftAmount)
    {
        foreach (var obj in commentObjects)
        {
            RectTransform rt = obj.GetComponent<RectTransform>();
            if (rt == null) continue;

            float currentY = rt.anchoredPosition.y;
            float targetY = currentY + shiftAmount;

            rt.DOAnchorPosY(targetY, shiftDuration).SetEase(Ease.OutSine);
        }
    }

    /// <summary>
    /// 上に押し上げられたコメントが枠外に出た場合は削除
    /// </summary>
    private void RemoveOutOfRangeComments()
    {
        float containerHeight = commentContainer.rect.height;
        List<GameObject> toRemove = new List<GameObject>();

        foreach (var obj in commentObjects)
        {
            RectTransform rt = obj.GetComponent<RectTransform>();
            if (rt == null) continue;

            if (rt.anchoredPosition.y > containerHeight + removeThreshold)
            {
                toRemove.Add(obj);
            }
        }

        foreach (var r in toRemove)
        {
            commentObjects.Remove(r);
            Destroy(r);
        }
    }

    /// <summary>
    /// ダメージ時のコメント処理（damageList のコメントを順次生成）
    /// </summary>
    public void Damage()
    {
        if (!isDamageMode)
        {
            StartCoroutine(DamageCommentFlow());
        }
    }

   IEnumerator DamageCommentFlow()
{
    isDamageMode = true;

    // damageList から候補リストを作成
    List<CommentListSO.CommentData> damageCandidates = new List<CommentListSO.CommentData>(commentListSO.damageList);

    // 出力するコメントを格納するリスト
    List<CommentListSO.CommentData> selectedComments = new List<CommentListSO.CommentData>();

    // 「コエル」のコメントがあれば、最初に追加し候補から除外する
    int koeruIndex = damageCandidates.FindIndex(x => x.name == "コエル");
    if (koeruIndex >= 0)
    {
        selectedComments.Add(damageCandidates[koeruIndex]);
        damageCandidates.RemoveAt(koeruIndex);
    }

    // 残りのコメントからランダムに選んで、合計5件になるようにする
    int targetCount = 5;
    while (selectedComments.Count < targetCount && damageCandidates.Count > 0)
    {
        int randomIndex = Random.Range(0, damageCandidates.Count);
        selectedComments.Add(damageCandidates[randomIndex]);
        damageCandidates.RemoveAt(randomIndex);
    }

    // 選ばれたコメントを順次生成
    foreach (var data in selectedComments)
    {
        SpawnComment(data);
        yield return new WaitForSeconds(0.3f);
    }

    yield return new WaitForSeconds(1f);
    isDamageMode = false;
}


    /// <summary>
    /// Attack 時のコメント処理（attackList のコメントを順次生成）
    /// </summary>
    public void Attack()
    {
        if (!isAttackMode)
        {
            StartCoroutine(AttackCommentFlow());
        }
    }

    IEnumerator AttackCommentFlow()
    {
        isAttackMode = true;

        int attackCount = Mathf.Min(commentListSO.attackList.Count, 10);
        for (int i = 0; i < attackCount; i++)
        {
            var data = commentListSO.attackList[i];
            SpawnComment(data);
            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(1f);
        isAttackMode = false;
    }
}
