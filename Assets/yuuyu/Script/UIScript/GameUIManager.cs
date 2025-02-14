using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{

    [SerializeField] GameObject[] KoeruPowerPanel;
    [SerializeField] GameObject[] PlayerHPPanel;

    // Start is called before the first frame update
    void Start()
    {
        
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
                KoeruPowerPanel[i-1].GetComponent<Image>().enabled=true;
            else
                KoeruPowerPanel[i-1].GetComponent<Image>().enabled=false;
        }
    }
    public void ChangeHPPanel(int _nowPlayerHP, int _maxPlayerHP)
    {
        for (int i= 1; i <= _maxPlayerHP; i++)
        {
            if(i<=_nowPlayerHP)
                PlayerHPPanel[i-1].GetComponent<Image>().enabled=true;
            else
                PlayerHPPanel[i-1].GetComponent<Image>().enabled=false;
        }
    }
    

}
