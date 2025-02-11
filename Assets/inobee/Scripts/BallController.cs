using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class BallController : MonoBehaviour
{
    // DOTween による移動の制御用 tween の参照（AlternateShooterManager から設定）
    public Tween tween;

    // 別オブジェクトにアタッチされている DecalApplier の参照（AlternateShooterManager から設定）
    public DecalApplier decalApplier;

    // ApplyDecal() の多重呼び出しを防ぐためのフラグ
    private bool decalApplied = false;

    // デカールが適用されたオブジェクトのリスト
    private static List<GameObject> decalAppliedObjects = new List<GameObject>();

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
            if (decalApplier != null && !decalApplied && !decalAppliedObjects.Contains(other.gameObject))
            {
                decalApplied = true;
                decalApplier.ApplyDecal();
                decalAppliedObjects.Add(other.gameObject); // デカールが適用されたオブジェクトをリストに追加
            }

            // 玉を破棄する
            Destroy(gameObject);
        }
    }
}