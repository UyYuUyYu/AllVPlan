using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectStageManager : MonoBehaviour
{
    public static int nowStageTileNum=0;  //今どこのStageTileにいるか

    [SerializeField] GameObject stageDecisionUI;
    bool isOnSelectStageTile=true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.E)&&isOnSelectStageTile==true)
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
