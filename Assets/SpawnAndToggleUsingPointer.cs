using UnityEngine;
using System.Collections;

public class SpawnAndToggleUsingPointer : MonoBehaviour
{
    // インスペクターで指定するプレハブ
    public GameObject prefab;

    // active 状態を切り替える対象のオブジェクト
    public GameObject targetObject;

    // プレハブを生成する固定座標
    private Vector3 spawnPosition = new Vector3(-16.5f, 130f, 12.5f);

    void MakeDonut()
    {
        // 固定座標にプレハブを生成
        if (prefab != null)
        {
            Instantiate(prefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Prefab が設定されていません。");
        }

        // 対象オブジェクトを active にして、5 秒後に false にする
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

    // 指定秒数後に targetObject の active 状態を false にするコルーチン
    IEnumerator DisableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
    }
}
