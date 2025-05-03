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

    public Transform cameraTransform;      // 揺らすカメラ
    private float shakeDuration = 0.8f;     // 揺れる時間
    private float shakeMagnitude = 0.5f;    // 揺れの強さ

    private Vector3 originalPos;
    private float currentShakeTime = 0f;
    private bool isShaking = false;

    
    void Awake()
    {
        Cursor.visible = false; // カーソルを非表示
        Cursor.lockState = CursorLockMode.Locked; // カーソルを画面中央に固定
        GameManager.isStartGame=true;
        cameraTransform=targetCamera.gameObject.transform;
        originalPos = cameraTransform.localPosition;
        subscribeMoney=0;
        elapsedTime=0;
        string sceneName=SceneManager.GetActiveScene().name;
        gameUIManager=GameObject.Find("UIManager").GetComponent<GameUIManager>();
        if(sceneName=="SweetScene")
        {
            limitTime=5.0f;
            targetSubscribeMoney=10;
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
        else if(sceneName=="ComicScene")
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

        if (isShaking)
        {
            currentShakeTime -= Time.unscaledDeltaTime;

            if (currentShakeTime > 0)
            {
                Vector3 randomOffset = Random.insideUnitSphere * shakeMagnitude;
                cameraTransform.localPosition = originalPos + randomOffset;
            }
            else
            {
                isShaking = false;
                cameraTransform.localPosition = originalPos;
                gameUIManager.GameOverPanel();
            }
        }

    }

    void DefaultIncreesSubscribe()
    {
        subscribeMoney+=ratioInreeesSubscription*_koeruSubscriber*Time.deltaTime;
        subscribeMoneyText.text=subscribeMoney.ToString("f0");
    }

    public void AddSubscibe(string _userName, int _money)
    {
        if(GameManager.isStartGame)
        {
            subscribeMoney+=_money;
            subscribeMoneyText.text=subscribeMoney.ToString("f0");
            gameUIManager.Subscribe(_userName,_money);
        }
       
    }

    public void StageClear()
    {
        GameManager.isStartGame=false;
        print("Clear");
        
    }
    public void TriggerGameOver()
    {
        isShaking = true;
        currentShakeTime = shakeDuration;

        // スローモーションもかける場合
        Time.timeScale = 0.2f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
}
