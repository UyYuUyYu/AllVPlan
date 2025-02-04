using System.Collections;
using UnityEngine;
using DG.Tweening;

public class AlternateShooterManager : MonoBehaviour
{
    [Header("発射オブジェクト（２つ）")]
    public Transform shooter1;
    public Transform shooter2;

    [Header("各発射オブジェクトの玉生成位置")]
    public Transform point1;
    public Transform point2;

    [Header("玉のプレハブ（Collider, Rigidbody, BallController を付与）")]
    public GameObject ballPrefab;

    [Header("DecalApplier を持つオブジェクト")]
    public DecalApplier decalApplier;  // Screen とは別のオブジェクトにアタッチしている DecalApplier コンポーネントの参照

    [Header("各種設定")]
    [Tooltip("上昇時のオフセット")]
    public float moveUpOffset = 1.0f;
    [Tooltip("上昇／元に戻る移動時間")]
    public float moveDuration = 0.5f;
    [Tooltip("玉の初速（DOTween でのジャンプ力算出に使用）")]
    public float throwSpeed = 10f;
    [Tooltip("玉の飛行時間（カメラまでの移動時間）")]
    public float ballFlightDuration = 1.0f;
    [Tooltip("次の発射までの待機時間")]
    public float delayAfterHit = 0.2f;

    // 各 shooter の初期位置を記録
    private Vector3 shooter1OriginalPos;
    private Vector3 shooter2OriginalPos;

    void Start()
    {
        if (shooter1 != null)
            shooter1OriginalPos = shooter1.position;
        if (shooter2 != null)
            shooter2OriginalPos = shooter2.position;

        StartCoroutine(ShootLoop());
    }

    IEnumerator ShootLoop()
    {
        int shooterIndex = 0; // 0: shooter1, 1: shooter2

        while (true)
        {
            // 現在の shooter と玉生成位置を選択
            Transform currentShooter = (shooterIndex == 0) ? shooter1 : shooter2;
            Transform currentPoint   = (shooterIndex == 0) ? point1 : point2;
            Vector3 currentOriginalPos = (shooterIndex == 0) ? shooter1OriginalPos : shooter2OriginalPos;

            // shooter を上方向に移動
            Vector3 targetPos = currentOriginalPos + Vector3.up * moveUpOffset;
            yield return StartCoroutine(MoveToPosition(currentShooter, targetPos, moveDuration));

            // 玉を生成（DOTween による移動のため Rigidbody の物理挙動は無効化）
            GameObject ball = Instantiate(ballPrefab, currentPoint.position, Quaternion.identity);
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            // カメラの位置をターゲットとする（必要に応じて位置調整してください）
            Vector3 cameraTarget = Camera.main.transform.position;

            // DOTween の DOJump を利用して、玉が弧を描くように移動
            float jumpPower = throwSpeed * 0.5f;
            Tween ballTween = ball.transform.DOJump(cameraTarget, jumpPower, 1, ballFlightDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    OnBallReachedTarget();
                    if (ball != null) Destroy(ball);
                });

            // BallController に tween と DecalApplier の参照を渡す
            BallController ballController = ball.GetComponent<BallController>();
            if (ballController != null)
            {
                ballController.tween = ballTween;
                ballController.decalApplier = decalApplier;
            }

            // 玉の飛行時間＋少し余裕を持って待機
            yield return new WaitForSeconds(ballFlightDuration + 0.1f);

            // shooter を元の位置に戻す
            yield return StartCoroutine(MoveToPosition(currentShooter, currentOriginalPos, moveDuration));

            // 次の shooter に切り替え
            shooterIndex = (shooterIndex + 1) % 2;
            yield return new WaitForSeconds(delayAfterHit);
        }
    }

    // 指定時間をかけてオブジェクトを targetPos まで移動させる補助コルーチン
    IEnumerator MoveToPosition(Transform obj, Vector3 targetPos, float duration)
    {
        Vector3 startPos = obj.position;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            obj.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        obj.position = targetPos;
    }

    // 玉が飛行完了したときの処理（Collision で先に破棄される場合もあり）
    void OnBallReachedTarget()
    {
        Debug.Log("玉がカメラ付近に到達しました");
        // 必要なら追加処理を記述
    }
}
