using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] TransitionSample transitionSample;

    public void LoadScene(string SceneName)
    {
          SceneManager.LoadScene(SceneName);


    }

    IEnumerator ChangeSecne(string SceneName)
    {
        yield return new WaitForSeconds(1);

    }
}
