using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class SelectStageMove : MonoBehaviour
{
    SelectStageManager selectStageManager;
    PlayerInput playerInput;
    [SerializeField] GameObject[] stageTiles;
    public bool isMoveOK=true;
    // Start is called before the first frame update
    void Start()
    {
        //SelectStageManager.nowStageTileNum=0;
        selectStageManager=GameObject.Find("SelectStageManager").GetComponent<SelectStageManager>();
    }
    void Awake()
    {
        playerInput=this.GetComponent<PlayerInput>();
    }
    void OnEnable()
    {
        playerInput.actions["Move"].performed+=OnMove;
    }
    void OnDisable()
    {
         playerInput.actions["Move"].performed-=OnMove;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D)&&isMoveOK)
        {
            CheckOnStageTile(false);
            MoveNextStageTile(1);
        }
        if(Input.GetKeyDown(KeyCode.A)&&isMoveOK)
        {
            CheckOnStageTile(false);
            MoveNextStageTile(-1);
        }
    }
    //ゲームパッドでの入力
    void OnMove(InputAction.CallbackContext context)
    {
        var value=context.ReadValue<Vector2>();
        float directionX=value.x;
        float directionY=value.y;
        //一旦右方向だけ
        if(isMoveOK)
        {
            CheckOnStageTile(false);
            if(directionX>0)
            {
                MoveNextStageTile(1);
            }
            else
            {
                MoveNextStageTile(-1);
            }
        }
    }

    //引数に前に進むか後ろに進むかで1と-1を代入すると動く
    public void MoveNextStageTile(int _nextNum)
    {
        int nowNum = SelectStageManager.nowStageTileNum;
        int nextNum = Mathf.Clamp(nowNum+_nextNum, 0, stageTiles.Length - 1);
        SelectStageManager.nowStageTileNum=nextNum;
        //int nextNum = NextTile(_nextNum);
        print(nextNum);
        if(nextNum>0||nextNum<stageTiles.Length)
        {
            print("aaa");
            DotoweenMove(nowNum,nextNum);
        }
       
    }
     
    //stageTIleの上にのる時に呼ばれて、移動中か移動中ジャないかを教える
    void CheckOnStageTile(bool _isOnStageTile)
    {
        isMoveOK=_isOnStageTile;
        selectStageManager.ChangeActiveStageDecisionUI(_isOnStageTile);
    }
    void DotoweenMove(int _nowNum,int _nextNum)
    {
        this.transform.DOPath(
            path: new Vector3[] {
                new Vector3(stageTiles[_nowNum].transform.position.x, this.transform.position.y, stageTiles[_nowNum].transform.position.z),
                new Vector3(stageTiles[_nextNum].transform.position.x, this.transform.position.y, stageTiles[_nextNum].transform.position.z)
            },
            duration: 4f,
            pathType: PathType.CatmullRom
            )
            .OnComplete(()=>CheckOnStageTile(true));
        /*
        this.transform.DOPath(
        path: new Vector3[] {
            new Vector3(stageTiles[_nowNum].transform.position.x, this.transform.position.y, stageTiles[_nowNum].transform.position.z),
            new Vector3(stageTiles[_nextNum].transform.position.x, this.transform.position.y, stageTiles[_nextNum].transform.position.z)
        },
        duration: 4f,
        pathType: PathType.CatmullRom
        )
        .OnComplete(()=>isMoveOK=true);
        */
    }


   
}
