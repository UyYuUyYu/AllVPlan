using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] string nextSceneName;
    [SerializeField] TransitionSample transitionSample;
    void Update()
    {
        // キーが押された、マウスの左クリック、またはコントローラーの「Submit」ボタンが押されたときにシーンを切り替える
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.GetButtonDown("Submit"))
        {
            transitionSample.SetProgress(18);
            StartCoroutine("ChangeSecne");
            
        }

    }
    IEnumerator ChangeSecne()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(nextSceneName);
    }
}
