using System.Collections;
using UnityEngine;

public class ToggleEnabledCounter : MonoBehaviour
{
    // 対象となるGameObjectをInspectorから設定
    [SerializeField] private GameObject Cutin;

    // オブジェクトをオンにしてから5秒後にオフにするメソッド
    public void StartCutin()
    {
        // GameObjectをオンにする
        Cutin.SetActive(true);
        // 5秒後にオフにするコルーチンを開始
        StartCoroutine(DisableAfterDelay(5f));
    }

    // 指定した秒数待ってからオブジェクトをオフにするコルーチン
    private IEnumerator DisableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Cutin.SetActive(false);
    }
}