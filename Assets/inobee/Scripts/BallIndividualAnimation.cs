// BallIndividualAnimation.cs
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BallIndividualAnimation : MonoBehaviour
{
    [Header("ボール画像の配列（インスペクターで設定）")]
    public Image[] balls;

    [Header("アニメーションするボールの個数")]
    [Range(0, 10)]
    public int ballCount = 3; // ここで実際にアニメーションさせるボールの個数を指定

    [Header("アニメーション設定")]
    public float animationDuration = 0.6f;
    public float delayIncrement = 0.2f; // ボールごとに追加する遅延時間

    void Start()
    {
        // 配列に設定されたボールの数と ballCount の値の整合性をチェック
        int count = Mathf.Min(ballCount, balls.Length);

        for (int i = 0; i < count; i++)
        {
            AnimateBall(balls[i], animationDuration, delayIncrement * i);
        }
    }

    void AnimateBall(Image ball, float duration, float delay)
    {
        // ボールのRectTransformのY軸を下に移動し、Yoyoループで往復アニメーションを実行
        ball.rectTransform.DOAnchorPosY(-25, duration)
            .SetRelative()
            .SetEase(Ease.InOutQuad)
            .SetLoops(-1, LoopType.Yoyo)
            .SetDelay(delay);
    }
}
