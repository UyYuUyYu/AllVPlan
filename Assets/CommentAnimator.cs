using UnityEngine;
using DG.Tweening;

public class CommentAnimator : MonoBehaviour
{
    // コメントが流れる領域
    public RectTransform containerRect;

    // 開始時に枠の外に置くためのオフセット
    public float offset = 100f;

    // ピクセル/秒
    public float speed = 200f;

    /// <summary>
    /// 下から上へコメントを流す
    /// </summary>
    public void StartVerticalScroll()
    {
        RectTransform rect = GetComponent<RectTransform>();
        if (rect == null || containerRect == null)
        {
            Debug.LogWarning("RectTransform または containerRect が設定されていません。");
            return;
        }

        float height = containerRect.rect.height;

        // 下端の外
        float startY = -height / 2 - offset;
        // 上端の外
        float endY   =  height / 2 + offset;

        // 左右位置はお好みで。ここでは中央に固定
        float xPos   = 0f;

        // 下端からスタート
        rect.anchoredPosition = new Vector2(xPos, startY);

        // 移動距離と時間を計算
        float distance = endY - startY;  // プラス値
        float duration = distance / speed;

        // DOTween で Y座標を endY まで移動
        rect.DOAnchorPosY(endY, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() => Destroy(gameObject));
    }
}
