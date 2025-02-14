using System.Collections;
using UnityEngine;

public class ToggleEnabledCounter2 : MonoBehaviour
{
    // オブジェクトをオフにしてから指定秒後にオンにするメソッド
    public void StartCutinReverse(GameObject Cutin)
    {
        // GameObjectをオフにする
        Cutin.SetActive(false);
        // 指定秒後にオンにするコルーチンを開始
        StartCoroutine(EnableAfterDelay(5f, Cutin));
    }

    // 指定した秒数待ってからオブジェクトをオンにするコルーチン
    private IEnumerator EnableAfterDelay(float delay, GameObject Cutin)
    {
        yield return new WaitForSeconds(delay);
        Cutin.SetActive(true);
    }
}