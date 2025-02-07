using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawberrySeedLauncher : MonoBehaviour
{
    [Header("いちご＆LineRenderer設定")]
    [Tooltip("５つのいちごのオブジェクトをアサイン（必ず同じ順番で）")]
    public GameObject[] strawberries;  // ５つのいちご
    [Tooltip("各いちごに対応するLineRendererのGameObjectをアサイン")]
    public GameObject[] lineRenderers; // ５つのLineRendererオブジェクト（SetActiveで表示/非表示）
    [Tooltip("各いちごから発射するタネの目標位置（Transform）をアサイン")]
    public Transform[] seedEndPositions;  // いちごごとのタネの飛ぶ先

    [Header("タネプレハブ設定")]
    [Tooltip("発射するタネのプレハブをアサイン")]
    public GameObject seedPrefab;

    [Header("回転設定")]
    [Tooltip("いちごの回転にかかる時間")]
    public float rotationDuration = 0.5f;
    [Tooltip("回転時にY軸方向へ上昇させるオフセット（単位はワールド座標）")]
    public float verticalOffset = 1f;

    [Header("LineRenderer（点滅）設定")]
    [Tooltip("LineRendererの点滅（点滅状態の継続時間）")]
    public float blinkingDuration = 2f;
    [Tooltip("点滅の間隔")]
    public float blinkInterval = 0.2f;

    [Header("タネ発射設定")]
    [Tooltip("いちごから発射するタネの回数")]
    public int seedShotCount = 3;
    [Tooltip("タネが目標位置に到達するまでの移動時間")]
    public float seedTravelTime = 1f;
    [Tooltip("各タネ発射の間隔")]
    public float seedShotInterval = 0.2f;

    [Header("シーケンス設定")]
    [Tooltip("シーケンス（全体の処理）の繰り返し回数")]
    public int repetitionCount = 3;
    [Tooltip("各シーケンス間の待機時間")]
    public float delayBetweenRounds = 2f;

    [Header("選択設定")]
    [Tooltip("チェックを入れると、選ばれるいちごの個数がランダムになります。")]
    public bool randomizeSelectionCount = true;
    [Tooltip("チェックがオフの場合、固定個数（1～いちご数内）で選択します。")]
    public int fixedSelectionCount = 5;

    // 各処理中のいちごの数を管理するための内部変数
    private int activeProcessCount = 0;

    /// <summary>
    /// 外部から呼び出してシーケンス処理を開始する関数
    /// </summary>
    public void StartSequence()
    {
        StartCoroutine(SequenceCoroutine());
    }

    /// <summary>
    /// シーケンス処理：指定された回数だけランダムな（または固定個数の）いちごを選び、  
    /// 回転＋上昇→点滅→タネ発射→元の状態（回転・位置復帰）を行います。
    /// </summary>
    private IEnumerator SequenceCoroutine()
    {
        for (int round = 0; round < repetitionCount; round++)
        {
            // ①【いちご選択】：選ばれるいちごの個数を決定
            int selectionCount = 0;
            if (randomizeSelectionCount)
            {
                // 1～いちご数（配列長）の間でランダムに選ぶ
                selectionCount = Random.Range(1, strawberries.Length + 1);
            }
            else
            {
                selectionCount = Mathf.Clamp(fixedSelectionCount, 1, strawberries.Length);
            }

            // ①-2：0～(いちご数-1)のインデックスリストを作成しシャッフルして先頭 selectionCount 個を選ぶ
            List<int> indices = new List<int>();
            for (int i = 0; i < strawberries.Length; i++)
            {
                indices.Add(i);
            }
            // シャッフル処理
            for (int i = 0; i < indices.Count; i++)
            {
                int temp = indices[i];
                int randomIndex = Random.Range(i, indices.Count);
                indices[i] = indices[randomIndex];
                indices[randomIndex] = temp;
            }
            List<int> selectedIndices = indices.GetRange(0, selectionCount);

            // ①-3：選ばれた個数分の処理を並列に開始するので、カウンターをセット
            activeProcessCount = selectedIndices.Count;
            foreach (int index in selectedIndices)
            {
                StartCoroutine(ProcessStrawberry(index));
            }

            // ②：全てのいちごの処理が終わるまで待つ
            yield return new WaitUntil(() => activeProcessCount == 0);

            // ③：次のシーケンスまで待機（最終回以外）
            if (round < repetitionCount - 1)
            {
                yield return new WaitForSeconds(delayBetweenRounds);
            }
        }
    }

    /// <summary>
    /// いちごひとつに対する処理（回転＋上昇→点滅→タネ発射→元の回転・位置復帰）  
    /// インデックスにより回転の方向（＋90 or －90）を決定します。  
    /// いちご1,3,5（インデックス0,2,4）は＋90、いちご2,4（インデックス1,3）は－90に回転させます。
    /// </summary>
    /// <param name="index">対象のいちごのインデックス</param>
    private IEnumerator ProcessStrawberry(int index)
    {
        // 対象のいちごとLineRendererオブジェクトを取得
        GameObject strawberry = strawberries[index];
        GameObject lineObj = lineRenderers[index];

        // 現在の回転と位置を保存
        Quaternion originalRotation = strawberry.transform.rotation;
        Vector3 originalPosition = strawberry.transform.position;

        // 回転方向を決定（0,2,4は＋90、1,3は－90）
        float zRotationDelta = (index % 2 == 0) ? 90f : -90f;
        Quaternion targetRotation = Quaternion.Euler(
            strawberry.transform.eulerAngles.x,
            strawberry.transform.eulerAngles.y,
            strawberry.transform.eulerAngles.z + zRotationDelta
        );
        // Y軸方向に上昇させた位置を算出
        Vector3 targetPosition = originalPosition + new Vector3(0, verticalOffset, 0);

        // ①【回転＋上昇】指定時間内で回転と位置移動（元位置→targetPosition）
        float elapsed = 0f;
        while (elapsed < rotationDuration)
        {
            float t = elapsed / rotationDuration;
            strawberry.transform.rotation = Quaternion.Slerp(originalRotation, targetRotation, t);
            strawberry.transform.position = Vector3.Lerp(originalPosition, targetPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        strawberry.transform.rotation = targetRotation;
        strawberry.transform.position = targetPosition;

        // ②【LineRenderer点滅】回転開始と同時に対応するLineRendererをアクティブにして点滅させる
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
        // 点滅終了後はLineRendererを非表示
        lineObj.SetActive(false);

        // ③【タネ発射】タネプレハブを生成し、指定のEndPositionへ向かって直線移動させる
        if (seedEndPositions.Length > index && seedEndPositions[index] != null)
        {
            for (int i = 0; i < seedShotCount; i++)
            {
                // タネ生成（いちごの位置から）
                GameObject seed = Instantiate(seedPrefab, strawberry.transform.position, Quaternion.identity);
                Vector3 startPos = seed.transform.position;
                Vector3 endPos = seedEndPositions[index].position;
                float t = 0f;
                // seedTravelTimeかけて直線移動（Lerpで線形補間）
                while (t < seedTravelTime)
                {
                    seed.transform.position = Vector3.Lerp(startPos, endPos, t / seedTravelTime);
                    t += Time.deltaTime;
                    yield return null;
                }
                seed.transform.position = endPos;
                // 必要なら一定時間後に削除（ここでは2秒後に削除）
                Destroy(seed, 2f);

                yield return new WaitForSeconds(seedShotInterval);
            }
        }
        else
        {
            Debug.LogWarning("Seed end position が未設定です。Index：" + index);
        }

        // ④【回転・位置復帰】元の回転と位置に戻す
        elapsed = 0f;
        while (elapsed < rotationDuration)
        {
            float t = elapsed / rotationDuration;
            strawberry.transform.rotation = Quaternion.Slerp(targetRotation, originalRotation, t);
            strawberry.transform.position = Vector3.Lerp(targetPosition, originalPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        strawberry.transform.rotation = originalRotation;
        strawberry.transform.position = originalPosition;

        // ⑤：このいちごの処理が終了したことを通知
        activeProcessCount--;
    }
}
