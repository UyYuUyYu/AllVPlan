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
    [Tooltip("玉の初速（投げる際の動きの速さに影響）")]
    public float throwSpeed = 10f;
    [Tooltip("玉の飛行時間（ターゲットまでの移動時間）")]
    public float ballFlightDuration = 1.0f;
    [Tooltip("次の発射までの待機時間")]
    public float delayAfterHit = 0.2f;

    [Header("UI Settings")]
    [Tooltip("玉が画面中央に到達した際にオンにするUIのImageオブジェクト（事前に非表示にしておく）")]
    public GameObject centerImage;

    // 各 shooter の初期位置を記録
    private Vector3 shooter1OriginalPos;
    private Vector3 shooter2OriginalPos;

    public void StartHoipAttack()
    {
        if (shooter1 != null)
            shooter1OriginalPos = shooter1.position;
        if (shooter2 != null)
            shooter2OriginalPos = shooter2.position;

        // centerImage を初期状態で非表示にしておく
        if (centerImage != null)
        {
            centerImage.SetActive(false);
        }

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
            Vector3 targetPosForShooter = currentOriginalPos + Vector3.up * moveUpOffset;
            yield return StartCoroutine(MoveToPosition(currentShooter, targetPosForShooter, moveDuration));

            // 玉を生成（DOTween による移動のため Rigidbody の物理挙動は無効化）
            GameObject ball = Instantiate(ballPrefab, currentPoint.position, Quaternion.identity);
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            // ★ターゲット位置は常にカメラの位置にする（あるいは必要ならカメラ前方の少し奥へ）
            // ここではカメラの位置そのものをターゲットにしています
            Vector3 targetPos = Camera.main.transform.position;
            // ※必要に応じて、targetPos = Camera.main.transform.position + Camera.main.transform.forward * 0.5f; などと調整可

            // 玉の初期スケールを小さく設定しておく（遠くから迫ってくる印象を出すため）
            ball.transform.localScale = Vector3.one * 0.5f;

            // DOTween Sequence で移動とスケールアップを同時に実行
            Sequence ballSequence = DOTween.Sequence();
            ballSequence.Join(ball.transform.DOMove(targetPos, ballFlightDuration).SetEase(Ease.Linear));
            ballSequence.Join(ball.transform.DOScale(54f, ballFlightDuration).SetEase(Ease.Linear));
            ballSequence.OnComplete(() =>
            {
                OnBallReachedTarget();
                if (ball != null) Destroy(ball);
            });

            // BallController に tween と DecalApplier の参照を渡す
            BallController ballController = ball.GetComponent<BallController>();
            if (ballController != null)
            {
                ballController.tween = ballSequence;
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

    // 玉がカメラに衝突したときの処理
    void OnBallReachedTarget()
    {
        Debug.Log("玉がカメラに衝突しました");

        // UIのImageをオンにする処理
        if (centerImage != null)
        {
            centerImage.SetActive(true);
        }
        
        // 衝突のインパクト演出としてカメラシェイクを実行
        // ※Camera.mainがnullの場合に備えてチェックする
        if (Camera.main != null)
        {
            // 0.2秒間、0.5単位の揺れを与える（必要に応じて調整）
           // Camera.main.transform.DOShakePosition(0.2f, 0.5f);
        }
    }
}
