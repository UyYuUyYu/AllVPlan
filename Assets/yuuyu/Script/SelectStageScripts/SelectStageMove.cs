using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class SelectStageMove : MonoBehaviour
{
    PlayerInput playerInput;
    [SerializeField] GameObject[] stageTiles;
    bool isMoveOK=true;
    // Start is called before the first frame update
    void Start()
    {
        SelectStageManager.nowStageTileNum=0;
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
            isMoveOK=false;
            Nextnum();
        }
        if(Input.GetKeyDown(KeyCode.A)&&isMoveOK)
        {
            isMoveOK=false;
            Bucknum();
        }
    }

    [ContextMenu("numa")]
    public void Nextnum()
    {
        MoveNextStageTile(1);
    }

    [ContextMenu("numb")]
    public void Bucknum()
    {
        MoveNextStageTile(-1);
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
    void DotoweenMove(int _nowNum,int _nextNum)
    {
        print(_nowNum+"と"+_nextNum);
        this.transform.DOPath(
        path: new Vector3[] {
            new Vector3(stageTiles[_nowNum].transform.position.x, this.transform.position.y, stageTiles[_nowNum].transform.position.z),
            new Vector3(stageTiles[_nextNum].transform.position.x, this.transform.position.y, stageTiles[_nextNum].transform.position.z)
        },
        duration: 4f,
        pathType: PathType.CatmullRom
        )
        .SetLookAt(0.05f, Vector3.forward)
        .OnComplete(()=>isMoveOK=true);
    }

    void OnMove(InputAction.CallbackContext context)
    {
        var value=context.ReadValue<Vector2>();
        float direction=value.x;

    }
}
