using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    void Update()
    {
        // キーが押された、マウスの左クリック、またはコントローラーの「Submit」ボタンが押されたときにシーンを切り替える
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.GetButtonDown("Submit"))
        {
            SceneManager.LoadScene("selectstage");
        }
    }
}
