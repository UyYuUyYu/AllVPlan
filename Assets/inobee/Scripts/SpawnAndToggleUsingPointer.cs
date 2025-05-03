using UnityEngine;
using System.Collections;

public class SpawnAndToggleUsingPointer : MonoBehaviour
{
    // 生成するプレハブの配列（インスペクター上で2つの要素を設定してください）
    public GameObject[] prefabs;
    // 生成する位置の配列（インスペクター上で2つの要素を設定してください）
    public Transform[] spawnPositions;

    // active 状態を切り替える対象のオブジェクト
    public GameObject targetObject;

    public void MakeDonut()
    {
        // 配列が正しく設定されているかチェック（2つ以上の要素が必要）
        if (prefabs == null || spawnPositions == null || prefabs.Length < 2 || spawnPositions.Length < 2)
        {
            Debug.LogWarning("Prefab と Spawn Position の配列に2つ以上の要素を設定してください。");
            return;
        }

        // 配列のインデックス0と1でドーナツを生成
        for (int i = 0; i < 2; i++)
        {
            if (prefabs[i] != null && spawnPositions[i] != null)
            {
                Instantiate(prefabs[i], spawnPositions[i].position, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Prefab または Spawn Position が null です。インデックス: " + i);
            }
        }

        // 対象オブジェクトを active にして、5 秒後に非表示にする
        if (targetObject != null)
        {
            targetObject.SetActive(true);
            StartCoroutine(DisableAfterDelay(5f));
        }
        else
        {
            Debug.LogWarning("Target Object が設定されていません。");
        }
    }

    // 指定秒数後に targetObject を非アクティブにするコルーチン
    IEnumerator DisableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
    }
}
