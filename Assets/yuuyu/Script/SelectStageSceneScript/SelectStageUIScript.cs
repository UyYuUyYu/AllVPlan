using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectStageUIScript : MonoBehaviour
{
   [SerializeField] GameObject UICanavas;
   private bool isUIPanel;
    void Start()
    {
        isUIPanel=false;
    }
    public void BuckTitle()
   {
        SceneManager.LoadScene("Title");

   }
   public void UIPanel()
   {
        if(isUIPanel)
        {
            UICanavas.SetActive(false);
            isUIPanel=false;
        }
        else{
            UICanavas.SetActive(true);
            isUIPanel=true;
        }
        
   }

}
