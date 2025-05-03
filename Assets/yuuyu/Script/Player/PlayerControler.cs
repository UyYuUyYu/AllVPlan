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
    Animator koeruAnimator;
    [SerializeField] GameObject standCollider,sitCollider,jumpCollider;


    //InputAction jumpAction;
    GameUIManager gameUIManager;

    Vector3 direction;
    int nowJumpCount;
    float countTime;
    
    private bool isSitKoeru;
    public bool isMoveDeray=false;
    public bool isRightDeray=false;
    public bool isLeftDeray=false;
    public bool isJumpDeray=false;
    private string moveKey;
    private float keyA=0,keyD=0;
    
    public float flashDuration = 1.0f;       // チカチカする合計時間
    public float flashInterval = 0.1f;       // チカチカの間隔

    private Renderer[] renderers;
    // Start is called before the first frame update
    void Start()
    {
        isSitKoeru=false;
        koeruAnimator= gameObject.GetComponent<Animator>();
            // 子オブジェクト含むすべてのRendererを取得
        renderers = GetComponentsInChildren<Renderer>();

        gameUIManager = GameObject.Find("UIManager").GetComponent<GameUIManager>();
        prayerRb=this.GetComponent<Rigidbody>();
        nowJumpCount=jumpCount;
        direction=new Vector3(0,0,0);
        countTime = 0;
        maxPlayerHP=playerHP;
        standCollider.SetActive(true);

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
        if(koeruAnimator.GetCurrentAnimatorStateInfo(0).IsName("Koeru_Stand"))
        {
            isSitKoeru=false;
        }
        if(!isSitKoeru)
        {
            
            if(Input.GetKeyDown(KeyCode.D))
            {
                StartCoroutine(Deray("D"));
            }
            
            if(Input.GetKey(KeyCode.D))
            {
                /*
                if(keyD<1.0f)
                {
                    keyD+=Time.deltaTime;
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    koeruAnimator.SetBool("isRun", true);

                    transform.position+=new Vector3(moveSpeed*Time.deltaTime,0,0);
                }
                */
                   
               
               
            }
            if(Input.GetKeyUp(KeyCode.D))
            {
                //DerayMove("D");
                keyD=0;
                StartCoroutine(DerayUp("D"));
                /*
                koeruAnimator.SetBool("isRun", false);

                direction=Vector3.zero;
                */
                
            }
            if(Input.GetKeyDown(KeyCode.A))
            {
                StartCoroutine(Deray("A"));
            }
            if(Input.GetKey(KeyCode.A))
            {
                /*
                if(isMoveDeray)
                {
                    transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                    koeruAnimator.SetBool("isRun", true);

                    transform.position-=new Vector3(moveSpeed*Time.deltaTime,0,0);
                }
                */
            }
            if(Input.GetKeyUp(KeyCode.A))
            {
                //DerayMove("A");
                keyA=0;
                StartCoroutine(DerayUp("A"));
                /*
                koeruAnimator.SetBool("isRun", false);

                direction=Vector3.zero;
                */
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
        }
        //playerの動きderay描けるよう
      
        if(isRightDeray)
        {
            print("横");
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            koeruAnimator.SetBool("isRun", true);

            transform.position+=new Vector3(moveSpeed*Time.deltaTime,0,0);
        }
        if(isLeftDeray)
        {

            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            koeruAnimator.SetBool("isRun", true);

            transform.position-=new Vector3(moveSpeed*Time.deltaTime,0,0);
        }
            
        
        
        if (Input.GetKey(KeyCode.S))
        {
            StartCoroutine(Deray("S"));
            isSitKoeru=true;
            koeruAnimator.SetBool("isSit", true);
            standCollider.SetActive(false);
            sitCollider.SetActive(true);
            //transform.position -= new Vector3(0, 0, moveSpeed * Time.deltaTime);
        }
        if(Input.GetKeyUp(KeyCode.S))
        {
            StartCoroutine(Deray("S"));
            koeruAnimator.SetBool("isSit", false);
            sitCollider.SetActive(false);
            standCollider.SetActive(true);
            direction=Vector3.zero;
        }
        if ((Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown(KeyCode.W))&&(nowJumpCount>0))
        {
            StartCoroutine(DerayJump("Jump"));
            
            /*
            koeruAnimator.SetBool("isJump", true);
            prayerRb.velocity = Vector3.up * jumpPower;
            nowJumpCount--;
            standCollider.SetActive(false);
            jumpCollider.SetActive(true);
            */
            

            //prayerRb.AddForce(new Vector3(0,jumpPower,0),ForceMode.Impulse);
        }
        #endregion

        if(playerHP==0)
        {

        }
        /*
        #region コエルパワー
        countTime += Time.deltaTime;
        if(countTime>getKoeruPowerTime)
        {
            countTime = 0;
            if (now_KoeruPower < max_KoeruPower)
                AddKoeruPower();
        }
        #endregion
        */

    }
    private IEnumerator Deray(string _key)
    {
   
       //moveKey=_key;
        yield return new WaitForSeconds(0.5f);
        if(isRightDeray || isLeftDeray)
        {
           
       
        }
        else if(!isRightDeray || !isLeftDeray)
        {
            if(_key=="D")
            {
                isRightDeray=true;
            }
            else if(_key=="A")
            {
                isLeftDeray=true;
            }
            //isMoveDeray=true;
            yield break;
        }
       
    }
    private IEnumerator DerayUp(string _key)
    {
        yield return new WaitForSeconds(0.5f);
        koeruAnimator.SetBool("isRun", false);

        direction=Vector3.zero;
        //isMoveDeray=false;
       // moveKey=null;
        if(_key=="D")
        {
            isRightDeray=false;
        }
        if(_key=="A")
        {
            isLeftDeray=false;
        }
        yield break;
    }
    private IEnumerator DerayJump(string _key)
    {
    
        yield return new WaitForSeconds(0.5f);

        koeruAnimator.SetBool("isJump", true);
        prayerRb.velocity = Vector3.up * jumpPower;
        nowJumpCount--;
        standCollider.SetActive(false);
        jumpCollider.SetActive(true);
        moveKey=null;
        yield break;
    
    }
    private void DerayMove(string _key)
    {
        if(_key=="D")
        {
            for(int i=0;i<3.0;i++)
            {
                print(i);
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                koeruAnimator.SetBool("isRun", true);

                transform.position+=new Vector3(moveSpeed*Time.deltaTime,0,0);
            }
            koeruAnimator.SetBool("isRun", false);

            direction=Vector3.zero;
        }
        if(_key=="A")
        {
            for(int i=0;i<1.0;i++)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                koeruAnimator.SetBool("isRun", true);

                transform.position+=new Vector3(moveSpeed*Time.deltaTime,0,0);
            }
            koeruAnimator.SetBool("isRun", false);

            direction=Vector3.zero;
        }
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
        print("Damege");
        StartCoroutine(FlashRoutine());
        playerHP--;
        gameUIManager.ChangeHPPanel(playerHP, maxPlayerHP);
    }

    [ContextMenu("ResetHP")]
    public void ResetHP()
    {
        playerHP=maxPlayerHP;
        gameUIManager.ChangeHPPanel(playerHP, maxPlayerHP);
    }

    void GameOver()
    {
        GameManager.isStartGame=false;

    }

    void OnTriggerEnter(Collider other)
    {
        string tagname=other.gameObject.tag;
        switch(tagname)
        {
            case "Grand":
                koeruAnimator.SetBool("isJump", false);
                
                jumpCollider.SetActive(false);
                standCollider.SetActive(true);
                
                nowJumpCount=jumpCount;
                break;
            case "Bullet":
                
                //Damege();
                break;
            default:
                break;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        string tagname=collision.gameObject.tag;
        switch(tagname)
        {
            
            case "Bullet":
                //Damege();
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
     private IEnumerator FlashRoutine()
    {
        float elapsed = 0f;
        bool isVisible = true;

        while (elapsed < flashDuration)
        {
            isVisible = !isVisible;

            foreach (var r in renderers)
            {
                r.enabled = isVisible;
            }

            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval;
        }

        // 最後は表示ONにしておく
        foreach (var r in renderers)
        {
            r.enabled = true;
        }
    }
}
