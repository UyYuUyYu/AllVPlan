using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



//ステージの強制スクロールとお金周り
public class StageManager : MonoBehaviour
{
    int _koeruSubscriber;
    float subscribeMoney;
    int targetSubscribeMoney;

    float elapsedTime,limitTime;
    [SerializeField] int ratioInreeesSubscription;
    [SerializeField] Text targetSubscribeMoneyText,subscribeMoneyText;

    GameUIManager gameUIManager;

    [SerializeField] Camera targetCamera;           // 動かす対象のカメラ
    [SerializeField] float scrollSpeed = 1.0f;      // スクロール速度（ユニット/秒）
    Vector3 scrollDirection = Vector3.right; // スクロール方向
    
    void Awake()
    {
        Cursor.visible = false; // カーソルを非表示
        Cursor.lockState = CursorLockMode.Locked; // カーソルを画面中央に固定
        GameManager.isStartGame=true;
        subscribeMoney=0;
        elapsedTime=0;
        string sceneName=SceneManager.GetActiveScene().name;
        gameUIManager=GameObject.Find("UIManager").GetComponent<GameUIManager>();
        if(sceneName=="Stage1")
        {
            limitTime=120.0f;
            targetSubscribeMoney=1000;
        }
        else if(sceneName=="Stage2")
        {
            limitTime=120.0f;
            targetSubscribeMoney=2000;
        }
        else if(sceneName=="TestYuuyu2")
        {
            limitTime=5.0f;
            targetSubscribeMoney=10;
        }
        _koeruSubscriber=GameManager.koeruSubscriber;
        print(_koeruSubscriber);
        targetSubscribeMoneyText.text=targetSubscribeMoney.ToString("f0");

    }

    // Update is called once per frame
    void Update()
    {
        if (targetCamera != null&&GameManager.isStartGame&&(elapsedTime<=limitTime))
        {
            elapsedTime+=Time.deltaTime;
            // 時間に応じてカメラを移動
            targetCamera.transform.position += scrollDirection.normalized * scrollSpeed * Time.deltaTime;
        }
        
        if((int)subscribeMoney>=targetSubscribeMoney)
        {
            if(GameManager.isStartGame)
            {
                StageClear();
            }
                
        }
        else
        {
            
            DefaultIncreesSubscribe();
        }

    }

    void DefaultIncreesSubscribe()
    {
        subscribeMoney+=ratioInreeesSubscription*_koeruSubscriber*Time.deltaTime;
        subscribeMoneyText.text=subscribeMoney.ToString("f0");
    }

    public void AddSubscibe(string _userName, int _money)
    {
        subscribeMoney+=_money;
        subscribeMoneyText.text=subscribeMoney.ToString("f0");
        gameUIManager.Subscribe(_userName,_money);
    }

    public void StageClear()
    {
        GameManager.isStartGame=false;
        print("Clear");
    }
}
