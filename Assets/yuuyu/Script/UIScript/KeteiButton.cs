using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeteiButton : MonoBehaviour
{
    public int number;
    public void SceneChange()
    {
        if(number==0)
        {
            SceneManager.LoadScene("SweetScene");
        }
        if(number==1)
        {
            
        }
        if(number==2)
        {
            
        }
    }
    public void GetNumber(int _number)
    {
        number=_number;
    }
}
