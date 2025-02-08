using UnityEngine;
using UnityEngine.Events;
using AirSticker.Runtime.Scripts;  // AirStickerProjector のある名前空間

public class DecalApplier : MonoBehaviour
{
    [Header("貼り付け対象とデカール設定")]
    [SerializeField] private GameObject receiverObject;  // デカールを貼り付けるオブジェクト
    [SerializeField] private Material decalMaterial;       // 使用するデカール用マテリアル

    [Header("投影範囲の設定")]
    [SerializeField] private float width = 1.0f;   // 投影範囲の幅
    [SerializeField] private float height = 1.0f;  // 投影範囲の高さ
    [SerializeField] private float depth = 1.0f;   // 投影範囲の奥行
    [SerializeField] private float zOffset = 0.005f; // デカール空間でのZ方向オフセット

    /// <summary>
    /// 任意のタイミングで呼び出してデカールを貼り付ける処理
    /// </summary>
    public void ApplyDecal()
    {
        // 1. 新規の GameObject を作成して、プロジェクタ用のコンテナとする
        GameObject projectorGO = new GameObject("AirStickerProjector_Instance");
        // 必要に応じて、貼り付けたい位置や回転を設定（ここでは呼び出し元の位置・回転を利用）
        projectorGO.transform.position = transform.position;
        projectorGO.transform.rotation = transform.rotation;

        // 2. 完了時に呼ばれるコールバックを設定（非同期処理の終了を受け取る）
        UnityAction<AirStickerProjector.State> onDecalApplied = (state) =>
        {
            if (state == AirStickerProjector.State.LaunchingCompleted)
            {
                Debug.Log("デカール貼り付けが正常に完了しました。");
            }
            else
            {
                Debug.Log("デカール貼り付けがキャンセルまたは失敗しました。");
            }
            // 完了後、プロジェクタ GameObject は不要であれば削除する
            Destroy(projectorGO);
        };

        // 3. CreateAndLaunch() を使って AirStickerProjector を生成し、投影処理を実行する
        //    launchOnAwake を true にすると、内部で自動的に Launch() が呼ばれます。
        AirStickerProjector.CreateAndLaunch(
            owner: projectorGO,        // プロジェクタコンポーネントを追加する GameObject
            receiverObject: receiverObject,  // デカールを貼る対象
            decalMaterial: decalMaterial,    // 使用するマテリアル
            width: width,                    // 投影範囲の幅
            height: height,                  // 投影範囲の高さ
            depth: depth,                    // 投影範囲の奥行
            launchOnAwake: true,             // 自動で投影処理を開始する
            onCompletedLaunch: onDecalApplied, // 完了時のコールバック
            zOffsetInDecalSpace: zOffset      // Zオフセット
        );
    }
}
