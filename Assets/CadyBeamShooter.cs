using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CadyBeamShooter : MonoBehaviour
{
    [Header("Cadyオブジェクト設定")]
    [Tooltip("cady1, cady2, cady3のGameObjectをアサイン")]
    public GameObject[] cadyObjects;  // 例：3つのCadyオブジェクト

    [Tooltip("各Cadyに対応するLineRendererオブジェクトをアサイン")]
    public GameObject[] cadyLineRenderers;  // 各Cady専用のLineRenderer（演出用）

    [Header("プレイヤー設定")]
    [Tooltip("プレイヤーのTransformをアサイン（シーン内のプレイヤーオブジェクト）")]
    public Transform playerTransform;

    [Header("ビーム設定")]
    [Tooltip("発射するビームのプレハブ")]
    public GameObject beamPrefab;
    [Tooltip("ビームが目標位置に到達するまでの移動時間")]
    public float beamTravelTime = 1f;

    [Header("回転設定")]
    [Tooltip("Cadyがプレイヤーの方向に回転する時間")]
    public float rotationDuration = 0.5f;

    [Header("LineRenderer（点滅）設定")]
    [Tooltip("LineRendererの点滅（演出）の継続時間")]
    public float blinkingDuration = 2f;
    [Tooltip("点滅の間隔")]
    public float blinkInterval = 0.2f;

    // 各Cadyの処理終了を管理する内部変数
    private int activeCadyProcessCount = 0;

    /// <summary>
    /// 外部から呼び出して全Cadyがビームを放つシーケンスを開始する関数  
    /// ※この関数が呼ばれると、その時点のプレイヤーの位置を1回だけ取得し、  
    /// 各Cadyがその方向へ回転、LineRenderer演出、ビーム発射、元の向きへの復帰を行います。
    /// </summary>
    public void StartBeamSequence()
    {
        if (playerTransform == null)
        {
            Debug.LogWarning("Player Transformが設定されていません。");
            return;
        }

        // 呼び出し時点のプレイヤーの位置を取得（各Cadyで共有）
        Vector3 targetPosition = playerTransform.position;
        activeCadyProcessCount = cadyObjects.Length;

        // 各Cadyに対して処理を並列実行
        for (int i = 0; i < cadyObjects.Length; i++)
        {
            StartCoroutine(ProcessCady(i, targetPosition));
        }
    }

    /// <summary>
    /// 個々のCadyに対する処理  
    /// ①：プレイヤーの位置方向へ回転  
    /// ②：各Cadyの位置をLineRendererのスタート位置に合わせ、点滅演出を行う  
    /// ③：ビームプレハブを生成し、プレイヤーの位置へ直線移動させる  
    /// ④：元の向きに戻す
    /// </summary>
    /// <param name="index">対象のCadyのインデックス</param>
    /// <param name="targetPosition">取得したプレイヤーの位置</param>
    private IEnumerator ProcessCady(int index, Vector3 targetPosition)
    {
        // 対象のCadyと対応するLineRendererオブジェクトを取得
        GameObject cady = cadyObjects[index];
        GameObject lineObj = (cadyLineRenderers.Length > index) ? cadyLineRenderers[index] : null;

        if (cady == null)
        {
            activeCadyProcessCount--;
            yield break;
        }

        // 現在の向きを保存
        Quaternion originalRotation = cady.transform.rotation;

        // プレイヤー方向へ回転するため、水平のみの方向を算出（y軸は無視）
        Vector3 direction = targetPosition - cady.transform.position;
        direction.y = 0;
        if (direction.sqrMagnitude < 0.001f)
        {
            // プレイヤーとの水平距離がほぼゼロの場合は、現在のforwardを利用
            direction = cady.transform.forward;
        }
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // ①【回転】指定時間内に元の向きからプレイヤー方向へ回転
        float elapsed = 0f;
        while (elapsed < rotationDuration)
        {
            float t = elapsed / rotationDuration;
            cady.transform.rotation = Quaternion.Slerp(originalRotation, targetRotation, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        cady.transform.rotation = targetRotation;

        // ②【LineRenderer点滅】cadyの現在位置にLineRendererを配置して点滅演出
        if (lineObj != null)
        {
            // cadyの位置をスタート位置として設定
            lineObj.transform.position = cady.transform.position;
            lineObj.SetActive(true);
            float blinkElapsed = 0f;
            bool lineVisible = true;
            while (blinkElapsed < blinkingDuration)
            {
                lineObj.SetActive(lineVisible);
                yield return new WaitForSeconds(blinkInterval);
                blinkElapsed += blinkInterval;
                lineVisible = !lineVisible;
            }
            lineObj.SetActive(false);
        }

        // ③【ビーム発射】ビームプレハブを生成し、プレイヤーの位置へ直線的に移動させる
        if (beamPrefab != null)
        {
            // ビームの初期位置はCadyの位置、向きはプレイヤー方向に設定
            GameObject beam = Instantiate(beamPrefab, cady.transform.position, Quaternion.LookRotation(direction));
            Vector3 startPos = cady.transform.position;
            float beamElapsed = 0f;
            while (beamElapsed < beamTravelTime)
            {
                float t = beamElapsed / beamTravelTime;
                beam.transform.position = Vector3.Lerp(startPos, targetPosition, t);
                beamElapsed += Time.deltaTime;
                yield return null;
            }
            beam.transform.position = targetPosition;
            // 例として、2秒後にビームを削除
            Destroy(beam, 2f);
        }

        // ④【元の向きに戻す】指定時間内に元の向きへスムーズに復帰
        elapsed = 0f;
        while (elapsed < rotationDuration)
        {
            float t = elapsed / rotationDuration;
            cady.transform.rotation = Quaternion.Slerp(targetRotation, originalRotation, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        cady.transform.rotation = originalRotation;

        activeCadyProcessCount--;
    }
}
