using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{

    //PayerDefautParameter
    [SerializeField] float moveSpeed=2;
    [SerializeField] float jumpPower=10f;
    [SerializeField] int jumpCount=1;
    [SerializeField] int playerHP=3;
    int maxPlayerHP;
    [SerializeField] int now_KoeruPower = 0;
    [SerializeField] int max_KoeruPower = 4;
    [SerializeField] float getKoeruPowerTime = 15.0f;

    //PayerComponent
    PlayerInput playerInput;
    Rigidbody prayerRb;
    //InputAction jumpAction;

    GameUIManager gameUIManager;

    PlayerAnimation playerAnimation;


    Vector3 direction;
    int nowJumpCount;
    float countTime;


    private Animator koeruAnimator;


    // Start is called before the first frame update
    void Start()
    {

        koeruAnimator= gameObject.GetComponent<Animator>();

        gameUIManager = GameObject.Find("UIManager").GetComponent<GameUIManager>();
        prayerRb=this.GetComponent<Rigidbody>();
        nowJumpCount=jumpCount;
        direction=new Vector3(0,0,0);
        countTime = 0;
        maxPlayerHP=playerHP;
    }
    void Awake()
    {
        playerInput=this.GetComponent<PlayerInput>();
        //jumpAction=playerInput.actions["Jump"];
    }

    void OnEnable()
    {
        playerInput.actions["Move"].performed+=OnMove;
        playerInput.actions["Move"].canceled+=OnMoveStop;
        playerInput.actions["Jump"].performed+=OnJump;
    }
    void OnDisable()
    {
        playerInput.actions["Move"].performed-=OnMove;
        playerInput.actions["Move"].canceled-=OnMoveStop;
        playerInput.actions["Jump"].performed-=OnJump;
    }
    // Update is called once per frame
    void Update()
    {
        #region コントローラー
        transform.position+=direction*moveSpeed*Time.deltaTime;
        /*
        if(jumpAction.ReadValue<float>()>0)
        {
            Jump();
        }
        */
        #endregion

        #region キー入力
        if(Input.GetKey(KeyCode.D))
        {

            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            koeruAnimator.SetBool("isRun", true);

            transform.position+=new Vector3(moveSpeed*Time.deltaTime,0,0);
        }
        if(Input.GetKeyUp(KeyCode.D))
        {

            koeruAnimator.SetBool("isRun", false);

            direction=Vector3.zero;
        }
        if(Input.GetKey(KeyCode.A))
        {

            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            koeruAnimator.SetBool("isRun", true);

            transform.position-=new Vector3(moveSpeed*Time.deltaTime,0,0);
        }
        if(Input.GetKeyUp(KeyCode.A))
        {

            koeruAnimator.SetBool("isRun", false);

            direction=Vector3.zero;
        }
        /*
        if (Input.GetKey(KeyCode.W))
        {

            koeruAnimator.SetBool("isRun", true);
            transform.position += new Vector3(0, 0, moveSpeed * Time.deltaTime);
        }
        if(Input.GetKeyUp(KeyCode.W))
        {
            koeruAnimator.SetBool("isRun", false);

            direction=Vector3.zero;
        }
        */
        if (Input.GetKey(KeyCode.S))
        {

            koeruAnimator.SetBool("isRun", true);
            //transform.position -= new Vector3(0, 0, moveSpeed * Time.deltaTime);
        }
        if(Input.GetKeyUp(KeyCode.S))
        {
            koeruAnimator.SetBool("isRun", false);

            direction=Vector3.zero;
        }
        if (Input.GetKeyDown(KeyCode.Space)&&(nowJumpCount>0))
        {
            koeruAnimator.SetBool("isJump", true);
            prayerRb.velocity = Vector3.up * jumpPower;
            nowJumpCount--;

            //prayerRb.AddForce(new Vector3(0,jumpPower,0),ForceMode.Impulse);
        }
        #endregion

        #region コエルパワー
        countTime += Time.deltaTime;
        if(countTime>getKoeruPowerTime)
        {
            countTime = 0;
            if (now_KoeruPower < max_KoeruPower)
                AddKoeruPower();
        }
        #endregion

    }

    void OnMove(InputAction.CallbackContext context)
    {

        koeruAnimator.SetBool("isRun", true);

        var value=context.ReadValue<Vector2>();
        if(value.x>0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        //direction=new Vector3(value.x,0,0).normalized;
        direction = new Vector3(value.x, 0, value.y).normalized;
    }
    void OnMoveStop(InputAction.CallbackContext context)
    {

        koeruAnimator.SetBool("isRun", false);

        direction=Vector3.zero;
    }
    void OnJump(InputAction.CallbackContext context)
    {
        koeruAnimator.SetBool("isJump", true);
        prayerRb.velocity = Vector3.up * jumpPower;
        nowJumpCount--;
    }

    [ContextMenu("AddKoeruPower")]
    public void AddKoeruPower()
    {
        now_KoeruPower++;
        gameUIManager.ChangeKoeruPowerPanel(now_KoeruPower, max_KoeruPower);
    }
    [ContextMenu("ResetKoeruPower")]
    public void ResetKoeruPower()
    {
        now_KoeruPower = 0;
        gameUIManager.ChangeKoeruPowerPanel(now_KoeruPower, max_KoeruPower);
    }

    [ContextMenu("Damege")]
     public void Damege()
    {
        playerHP--;
        gameUIManager.ChangeHPPanel(playerHP, maxPlayerHP);
    }

    [ContextMenu("ResetHP")]
    public void ResetHP()
    {
        playerHP=maxPlayerHP;
        gameUIManager.ChangeHPPanel(playerHP, maxPlayerHP);
    }

    void OnCollisionEnter(Collision collision)
    {
        string tagname=collision.gameObject.tag;
        switch(tagname)
        {
            case "Grand":
                koeruAnimator.SetBool("isJump", false);
                nowJumpCount=jumpCount;
                break;
            case "Bullet":
                Damege();
                break;
            default:
                break;
        }
    }

    /*外からダメージ判定を呼ぶ用ダメージに大きさを持たせる時
    public void Damege(int _damege)
    {
        playerHP-=_damege;
    }
    */
}
