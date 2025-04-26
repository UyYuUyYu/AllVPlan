using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    int _koeruSubscriber;
    float subscribeMoney;
    int targetSubscribeMoney;
    [SerializeField] int ratioInreeesSubscription;
    [SerializeField] Text targetSubscribeMoneyText,subscribeMoneyText;
    
    void Awake()
    {
        subscribeMoney=0;
        string sceneName=SceneManager.GetActiveScene().name;
        if(sceneName=="Stage1")
        {
            targetSubscribeMoney=1000;
        }
        else if(sceneName=="Stage2")
        {
            targetSubscribeMoney=2000;
        }
        else if(sceneName=="TestYuuyu2")
        {
            targetSubscribeMoney=200;
        }
        _koeruSubscriber=GameManager.koeruSubscriber;
        print(_koeruSubscriber);
        targetSubscribeMoneyText.text=targetSubscribeMoney.ToString("f0");

    }

    // Update is called once per frame
    void Update()
    {
        DefaultIncreesSubscribe();
        if((int)subscribeMoney==targetSubscribeMoney)
        {
            
            StageClear();
        }

    }

    void DefaultIncreesSubscribe()
    {
        subscribeMoneyText.text=subscribeMoney.ToString("f0");
        subscribeMoney+=ratioInreeesSubscription*_koeruSubscriber*Time.deltaTime;
    }

    public void StageClear()
    {
        print("Clear");
    }
}
