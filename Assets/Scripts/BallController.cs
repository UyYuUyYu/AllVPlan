using UnityEngine;
using DG.Tweening;

public class BallController : MonoBehaviour
{
    // DOTween による移動の制御用 tween の参照（AlternateShooterManager から設定）
    public Tween tween;

    // 別オブジェクトにアタッチされている DecalApplier の参照（AlternateShooterManager から設定）
    public DecalApplier decalApplier;

    // ApplyDecal() の多重呼び出しを防ぐためのフラグ
    private bool decalApplied = false;

    void OnTriggerEnter(Collider other)
    {
        // 衝突対象が "Screen" タグの場合
        if (other.CompareTag("Screen"))
        {
            // tween が動作中なら停止する
            if (tween != null && tween.IsActive())
            {
                tween.Kill();
            }

            // DecalApplier が設定されており、まだ適用していなければ ApplyDecal() を呼び出す
            if (decalApplier != null && !decalApplied)
            {
                decalApplied = true;
                decalApplier.ApplyDecal();
            }

            // 玉を破棄する
            Destroy(gameObject);
        }


    }
    
}
