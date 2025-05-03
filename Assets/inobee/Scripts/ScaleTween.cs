using UnityEngine;
using DG.Tweening;

public class ScaleTween : MonoBehaviour
{
    // 目標のスケール（オブジェクトが拡大するサイズ）
    [SerializeField]
    private Vector3 targetScale = new Vector3(1.5f, 1.5f, 1.5f);

    // 拡大・縮小の所要時間
    [SerializeField]
    private float duration = 0.5f;

    void Start()
    {
        // 現在のスケールからtargetScaleへduration秒で変化し、
        // その後元に戻す（Yoyoループ）無限ループで繰り返す
        transform.DOScale(targetScale, duration)
                 .SetLoops(-1, LoopType.Yoyo)
                 .SetEase(Ease.InOutSine);
    }
}
