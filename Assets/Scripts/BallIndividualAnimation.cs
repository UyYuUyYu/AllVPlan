// BallIndividualAnimation.cs
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BallIndividualAnimation: MonoBehaviour
{
    public Image ball1;
    public Image ball2;
    public Image ball3;

    void Start()
    {
        // ボール1のアニメーション
        AnimateBall(ball1, 0.6f, 0.0f); // すぐに開始

        // ボール2のアニメーション
        AnimateBall(ball2, 0.6f, 0.2f); // 0.5秒後に開始

        // ボール3のアニメーション
        AnimateBall(ball3, 0.6f, 0.4f); // 1秒後に開始
    }

    void AnimateBall(Image ball, float duration, float delay)
    {
        ball.rectTransform.DOAnchorPosY(-50, duration) // 初期位置から下に落ちる
            .SetRelative()
            .SetEase(Ease.InOutQuad)
            .SetLoops(-1, LoopType.Yoyo) // Yoyoループで元の位置に戻る
            .SetDelay(delay);
    }
}
