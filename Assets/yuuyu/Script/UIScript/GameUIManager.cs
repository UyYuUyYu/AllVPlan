using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{

    [SerializeField] GameObject[] KoeruPowerPanel;
    [SerializeField] GameObject tutorialUIPanel;
    bool isTutorialUIPanel=false;

    // Start is called before the first frame update
    void Start()
    {
       // print(()=>isTutorialUIPanel=!isTutorialUIPanel);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeKoeruPowerPanel(int _nowKoeruPower, int _maxKoeruPower)
    {
        for (int i= 1; i <= _maxKoeruPower; i++)
        {
            if(i<=_nowKoeruPower)
                KoeruPowerPanel[i-1].SetActive(true);
            else
                KoeruPowerPanel[i-1].SetActive(false);
        }

       
    }

    void ChangeTutorialPanel()
    {
        if(isTutorialUIPanel)
        {
            isTutorialUIPanel=false;
            //print(()=>isTutorialUIPanel=!isTutorialUIPanel);
            tutorialUIPanel.SetActive(isTutorialUIPanel);
        }  
        else
        {
            isTutorialUIPanel=true;
            tutorialUIPanel.SetActive(isTutorialUIPanel);
        }
            
    }

}
