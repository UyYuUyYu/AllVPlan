using UnityEngine;
using System.Collections.Generic;

public class UmbrellaSpawner : MonoBehaviour
{
    [Header("Umbrellaプレハブを複数登録")]
    public GameObject[] umbrellaPrefabs;

    [Header("左右の生成範囲")]
    public float xMin = -5f;
    public float xMax =  5f;

    [Header("生成時のY座標")]
    public float spawnY = 10f;

    [Header("Umbrellaの最大数")]
    public int maxUmbrellaCount = 5;

    [Header("傘のZ座標参照用オブジェクト")]
    public Transform pointObject;

    // --- UmbrellaController のパラメータをまとめて設定 ---
    [Header("=== UmbrellaController Settings ===")]
    public float fallSpeed       = 1f;  // 落下速度
    public float swingDistance   = 1f;  // 左右ふわふわの振幅（不要なら0に）
    public float swingFrequency  = 1f;  // 左右ふわふわの周波数（不要なら0に）
    public float rotationSpeed   = 50f; // くるくる回転速度

    // 現在アクティブなUmbrellaを管理
    private List<GameObject> activeUmbrellas = new List<GameObject>();

    void Update()
    {
        // 存在するUmbrellaが最大数未満なら新たに生成
        if (activeUmbrellas.Count < maxUmbrellaCount)
        {
            SpawnUmbrella();
        }

        // 画面外に落ちたUmbrellaを破棄
        for (int i = activeUmbrellas.Count - 1; i >= 0; i--)
        {
            var umb = activeUmbrellas[i];
            if (umb == null)
            {
                activeUmbrellas.RemoveAt(i);
                continue;
            }

            // 例えばY座標が -5 より下に行ったら消す
            if (umb.transform.position.y < -5f)
            {
                Destroy(umb);
                activeUmbrellas.RemoveAt(i);
            }
        }
    }

    void SpawnUmbrella()
    {
        // pointObject の Z座標を使用
        float zPos = (pointObject != null) ? pointObject.position.z : 0f;

        // ランダムなX座標を決定
        float randomX = Random.Range(xMin, xMax);
        Vector3 spawnPos = new Vector3(randomX, spawnY, zPos);

        // umbrellaPrefabs からランダムに1つ選ぶ
        if (umbrellaPrefabs == null || umbrellaPrefabs.Length == 0)
        {
            Debug.LogWarning("UmbrellaPrefabs が設定されていません");
            return;
        }
        int randomIndex = Random.Range(0, umbrellaPrefabs.Length);
        GameObject prefab = umbrellaPrefabs[randomIndex];

        // 生成
        GameObject newUmbrella = Instantiate(prefab, spawnPos, Quaternion.identity);
        activeUmbrellas.Add(newUmbrella);

        // --- UmbrellaController のパラメータをここで一括設定 ---
        UmbrellaController uc = newUmbrella.GetComponent<UmbrellaController>();
        if (uc != null)
        {
            uc.fallSpeed      = Random.Range(6f, 15f); // Set rotationSpeed to a random value between 10 and 70
            uc.swingDistance  = swingDistance;
            uc.swingFrequency = swingFrequency;
            uc.rotationSpeed  = Random.Range(10f, 70f); // Set rotationSpeed to a random value between 10 and 70
        }
    }
}
