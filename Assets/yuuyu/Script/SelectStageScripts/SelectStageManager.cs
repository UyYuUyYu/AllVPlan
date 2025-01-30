using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SelectStageManager : MonoBehaviour
{
    public static int nowStageTileNum=0;  //今どこのStageTileにいるか
    PlayerInput playerInput;
    [SerializeField] GameObject stageDecisionUI;
    bool isOnSelectStageTile=true;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Awake()
    {
        playerInput=this.GetComponent<PlayerInput>();
    }
    void OnEnable()
    {
        playerInput.actions["Select"].performed+=OnChangeSecene;
    }
    void OnDisable()
    {
         playerInput.actions["Select"].performed-=OnChangeSecene;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.E)&&isOnSelectStageTile)
        {
            ScneChange();
        }
    }
    //コントローラーのボタンの東側が押されたら
    void OnChangeSecene(InputAction.CallbackContext context)
    {
       if(isOnSelectStageTile)
       {
            ScneChange();
       }
    }    
    //今いるステージタイルを参照してシーン切り替え
    public void ScneChange()
    {
        switch(nowStageTileNum)
        {
            case 0:
                Debug.Log("0");
                break;
            case 1:
                Debug.Log("1");
                SceneManager.LoadScene("Stage1");
                break;
            case 2:
                Debug.Log("2");
                SceneManager.LoadScene("Stage2");
                break;
            default:
                break;
        } 
    }
    //今いるStageTileがどこなのか番号を伝える関数使うかも？
    public void AddNowStageTileNum(int _stageTileNum)
    {
        nowStageTileNum=_stageTileNum;
    }

    public void ChangeActiveStageDecisionUI(bool _isChangeActive)
    {
        isOnSelectStageTile=_isChangeActive;
        stageDecisionUI.SetActive(_isChangeActive);
    }

}
