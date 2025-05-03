using System.Collections;
using UnityEngine;

public class ToggleEnabledCounter : MonoBehaviour
{
    // オブジェクトをオンにしてから5秒後にオフにするメソッド
    public void StartCutin(GameObject Cutin)
    {
        // GameObjectをオンにする
        Cutin.SetActive(true);
        // 5秒後にオフにするコルーチンを開始
        StartCoroutine(DisableAfterDelay(5f, Cutin));
    }

    // 指定した秒数待ってからオブジェクトをオフにするコルーチン
    private IEnumerator DisableAfterDelay(float delay, GameObject Cutin)
    {
        yield return new WaitForSeconds(delay);
        Cutin.SetActive(false);
    }
}