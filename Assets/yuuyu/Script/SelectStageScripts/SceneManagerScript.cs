using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagerScript : MonoBehaviour
{
    public void SceneMove1()
    {
        SceneManager.LoadScene("loading");
    }
    public void SceneMove2()
    {
        SceneManager.LoadScene("loading2");
    }
}
