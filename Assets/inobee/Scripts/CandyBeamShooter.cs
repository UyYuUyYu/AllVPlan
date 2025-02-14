using System.Collections;
using UnityEngine;

public class CandyBeamShooter : MonoBehaviour
{
    [Header("Candy 設定")]
    [Tooltip("candy1, candy2, candy3 の GameObject をアサイン")]
    public GameObject[] candyObjects;

    [Header("Warning Start Position 設定")]
    [Tooltip("各 Candy に対応する警告ラインの開始位置として使う Transform をアサインします。未指定の場合は Candy の位置を使用します。")]
    public Transform[] warningStartPositions;

    [Header("プレイヤー設定")]
    [Tooltip("プレイヤーの Transform をアサイン")]
    public Transform playerTransform;

    [Header("警告ライン設定")]
    [Tooltip("警告ラインとして表示する Prefab をアサインしてください。Quad などシンプルな Mesh 推奨")]
    public GameObject warningPrefab;
    [Tooltip("警告ラインの点滅時間（秒）")]
    public float warningBlinkDuration = 1f;
    [Tooltip("警告ラインの点滅間隔（秒）")]
    public float blinkInterval = 0.2f;

    [Header("Beam 設定")]
    [Tooltip("Beam 用の Prefab をアサインしてください")]
    public GameObject beamPrefab;
    [Tooltip("Beam の出現（伸びる）時間（秒、速く出す）")]
    public float beamEmergenceTime = 0.2f;
    [Tooltip("Beam の表示時間（出現後、保持する時間）")]
    public float beamDisplayDuration = 0.5f;

    [Header("延長設定")]
    [Tooltip("警告ラインおよび Beam がプレイヤーまでの距離に乗算される拡張係数。1.0 ならちょうどプレイヤーまで、>1.0 ならプレイヤーを貫通します。")]
    public float penetrationFactor = 1.2f;

    /// <summary>
    /// この関数が呼ばれると、呼び出し時のプレイヤー位置を取得し、
    /// 各 Candy から警告ラインを即時表示して点滅させ、点滅終了後に Beam を Candy からだんだん出す演出を実行します。
    /// </summary>
    public void StartWarningAndBeamSequence()
    {
        if (playerTransform == null)
        {
            Debug.LogWarning("Player Transform が設定されていません。");
            return;
        }

        // 呼び出し時のプレイヤー位置を1回取得
        Vector3 targetPosition = playerTransform.position;

        // 各 Candy について処理を開始
        for (int i = 0; i < candyObjects.Length; i++)
        {
            StartCoroutine(ProcessCandy(i, targetPosition));
        }
    }

    /// <summary>
    /// 各 Candy に対する処理：
    /// ① 警告ライン（warningPrefab）を、Candy の開始位置からプレイヤー方向に即時表示（スケールは警告ラインの長さに合わせる）
    /// ② 警告ラインを blinkInterval ごとに点滅させ、warningBlinkDuration 秒間演出する
    /// ③ 点滅が終了したら、Beam (beamPrefab) を Candy の位置から生成し、beamEmergenceTime かけてだんだん出現させる
    /// ④ beamDisplayDuration 経過後に Beam を削除
    /// </summary>
    private IEnumerator ProcessCandy(int index, Vector3 targetPosition)
    {
        // 対応する Candy オブジェクトを取得
        GameObject candy = (candyObjects.Length > index) ? candyObjects[index] : null;
        if (candy == null)
            yield break;

        // 警告ラインの開始位置（warningStartPositions があればその位置、なければ Candy の位置）
        Vector3 startPos = candy.transform.position;
        if (warningStartPositions != null && warningStartPositions.Length > index && warningStartPositions[index] != null)
        {
            startPos = warningStartPositions[index].position;
        }

        // Candy の開始位置から targetPosition への方向と距離を計算
        Vector3 diff = targetPosition - startPos;
        float distance = diff.magnitude;
        if (distance < 0.001f)
        {
            distance = 1f;
            diff = Vector3.forward;
        }
        Vector3 direction = diff.normalized;

        // 拡張した距離（プレイヤーを貫通する長さ）を算出
        float extendedDistance = distance * penetrationFactor;

        // ──【① 警告ラインの即時表示＆点滅演出】────────────────────
        GameObject warningLine = null;
        if (warningPrefab != null)
        {
            // 警告Prefab を開始位置に生成し、ターゲット方向に向ける
            warningLine = Instantiate(warningPrefab, startPos, Quaternion.LookRotation(direction));
            // 即時、Z軸スケールを extendedDistance に合わせる（伸びるアニメーションはなし）
            Vector3 warningScale = warningLine.transform.localScale;
            warningScale.z = extendedDistance;
            warningLine.transform.localScale = warningScale;

            // 点滅演出（blinkIntervalごとに MeshRenderer の表示／非表示を切り替え）
            MeshRenderer mr = warningLine.GetComponent<MeshRenderer>();
            if (mr == null)
                mr = warningLine.GetComponentInChildren<MeshRenderer>();
            float blinkTime = 0f;
            while (blinkTime < warningBlinkDuration)
            {
                if (mr != null)
                    mr.enabled = true;
                yield return new WaitForSeconds(blinkInterval);
                if (mr != null)
                    mr.enabled = false;
                yield return new WaitForSeconds(blinkInterval);
                blinkTime += blinkInterval * 2;
            }
            // 最後に非表示にして削除
            if (mr != null)
                mr.enabled = false;
            Destroy(warningLine);
        }
        else
        {
            // warningPrefab 未設定の場合は、単に待機
            yield return new WaitForSeconds(warningBlinkDuration);
        }

        // ──【② Beam の出現演出】────────────────────────────
        if (beamPrefab != null)
        {
            // Beam を開始位置に生成し、ターゲット方向に向ける
            GameObject beam = Instantiate(beamPrefab, startPos, Quaternion.LookRotation(direction));
            // 初期状態は Z 軸スケール 0（全体のX, Y はPrefabに合わせる）
            Vector3 beamScale = beam.transform.localScale;
            beamScale.z = 0f;
            beam.transform.localScale = beamScale;

            // beamEmergenceTime かけて、Beam の Z軸スケールを 0 から extendedDistance に伸ばす（速く出る）
            float elapsed = 0f;
            while (elapsed < beamEmergenceTime)
            {
                float t = elapsed / beamEmergenceTime;
                Vector3 newScale = beam.transform.localScale;
                newScale.z = Mathf.Lerp(0, extendedDistance, t);
                beam.transform.localScale = newScale;
                elapsed += Time.deltaTime;
                yield return null;
            }
            // 確実にフルの長さに設定
            Vector3 finalScale = beam.transform.localScale;
            finalScale.z = extendedDistance;
            beam.transform.localScale = finalScale;

            // beamDisplayDuration 経過後に Beam を削除
            yield return new WaitForSeconds(beamDisplayDuration);
            Destroy(beam);
        }
    }
}
