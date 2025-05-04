using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] TransitionSample transitionSample;
    [SerializeField] private string sceneToLoad; // ロードするシーン名を指定

    public void LoadScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    IEnumerator ChangeSecne(string SceneName)
    {
        yield return new WaitForSeconds(1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) // エンターキーを検出
        {
            LoadScene(sceneToLoad); // 指定されたシーンをロード
        }
    }
}