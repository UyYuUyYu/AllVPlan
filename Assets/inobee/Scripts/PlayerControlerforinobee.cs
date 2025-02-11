using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Fungus;

public class PlayerControlerforinobee : MonoBehaviour
{
    PlayerInput playerInput;
    [SerializeField] float moveSpeed = 2;
    [SerializeField] float originalSpeed = 5f;
    [SerializeField] float jumpPower = 10f;
    [SerializeField] int jumpCount = 1;
    int nowJumpCount;
    [SerializeField] int playerHP = 3;
    Rigidbody prayerRb;

    [SerializeField] ToggleEnabledCounter toggleEnabledCounter;
    [SerializeField] CommentManager commentManager;

    [SerializeField] GameObject cutin;
    public Flowchart flowchart;

    // InputSystemで得た方向（主にコントローラー入力）
    Vector3 direction;

    // キーボード入力も加味した生の入力を合算するための変数
    Vector3 rawInput;

    // ブロックする方向を保持するリスト
    private List<Vector3> blockedDirections = new List<Vector3>();

    void Start()
    {
        prayerRb = this.GetComponent<Rigidbody>();
        nowJumpCount = jumpCount;
        direction = Vector3.zero;
    }

    void Awake()
    {
        playerInput = this.GetComponent<PlayerInput>();
    }

    void OnEnable()
    {
        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Move"].canceled += OnMoveStop;
        playerInput.actions["Jump"].performed += OnJump;
    }

    void OnDisable()
    {
        playerInput.actions["Move"].performed -= OnMove;
        playerInput.actions["Move"].canceled -= OnMoveStop;
        playerInput.actions["Jump"].performed -= OnJump;
    }

    // Update is called once per frame
    void Update()
    {
        // InputSystemからの入力（direction）とキーボード入力（W,A,S,D）を合算する
        rawInput = direction;  // ここはコントローラー入力で取得した方向

        if (Input.GetKey(KeyCode.D))
        {
            rawInput += Vector3.right;
        }
        if (Input.GetKey(KeyCode.A))
        {
            rawInput += Vector3.left;
        }
        if (Input.GetKey(KeyCode.W))
        {
            rawInput += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            rawInput += Vector3.back;
        }

        // 複数入力があれば正規化して方向のみを保持
        rawInput = rawInput.normalized;

        // ブロックされている方向の成分を除去する
        Vector3 effectiveInput = rawInput;
        foreach (Vector3 blockDir in blockedDirections)
        {
            Vector3 normalized = blockDir.normalized;
            float dot = Vector3.Dot(effectiveInput, normalized);
            if (dot > 0)
            {
                Vector3 projection = Vector3.Project(effectiveInput, normalized);
                effectiveInput -= projection;
            }
        }

        // 移動速度の調整（入力元により倍率を変える）
        float currentMoveSpeed = moveSpeed;
        string currentScheme = playerInput.currentControlScheme;

        if (currentScheme == "Gamepad")
        {
            // コントローラーの場合：斜め移動なら速度2倍
            if (Mathf.Abs(effectiveInput.x) > 0 && Mathf.Abs(effectiveInput.z) > 0)
            {
            Debug.Log("Gamepad");
                currentMoveSpeed *= 2.0f;
            }
            // ※ コントローラーでの上下のみ・左右のみはそのまま
        }
        else
        {
            // キーボード（またはその他）の場合：斜め移動なら1.5倍、上下のみなら3倍
            if (Mathf.Abs(effectiveInput.x) > 0 && Mathf.Abs(effectiveInput.z) > 0)
            {
                currentMoveSpeed *= 1.5f;
            }
            else if (Mathf.Abs(effectiveInput.z) > 0 && Mathf.Approximately(effectiveInput.x, 0f))
            {
                currentMoveSpeed *= 3.0f;
            }
            // 左右のみの場合はそのまま
        }

        transform.position += effectiveInput * currentMoveSpeed * Time.deltaTime;

        // ジャンプ処理（スペースキーの場合のみ）
        if (Input.GetKeyDown(KeyCode.Space) && nowJumpCount > 0)
        {
            prayerRb.velocity = Vector3.up * jumpPower;
            nowJumpCount--;
        }
    }

    void OnMove(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();
        // x軸をx、y軸をzとして扱う（コントローラー入力）
        direction = new Vector3(value.x, 0, value.y).normalized;
    }

    void OnMoveStop(InputAction.CallbackContext context)
    {
        direction = Vector3.zero;
    }

    void OnJump(InputAction.CallbackContext context)
    {
        if (nowJumpCount > 0)
        {
            prayerRb.velocity = Vector3.up * jumpPower;
            nowJumpCount--;
        }
    }

    void OnCollisionEnter(UnityEngine.Collision collision)
    {
        string tagname = collision.gameObject.tag;
        switch (tagname)
        {
            case "Grand":
                nowJumpCount = jumpCount;
                break;
            case "Bullet":
                Debug.Log("collider");
                commentManager.Damage();
                break;
            default:
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "energy")
        {
            toggleEnabledCounter.StartCutin(cutin);
            // Fungusの "Energy" ブロックを実行
            flowchart.ExecuteBlock("Energy");
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Bullet")
        {
            Debug.Log("hit trigger");
            commentManager.Damage();
        }
    }

    public void Damage(int damage)
    {
        Debug.Log("Player damaged: " + damage);
        playerHP -= damage;
        commentManager.Damage();
    }

    /// <summary>
    /// 既存の動きを完全に停止させる（moveSpeedを0にする）
    /// </summary>
    public void DisableMovement()
    {
        moveSpeed = 0f;
    }

    /// <summary>
    /// 移動を再開する（元のスピードに戻す）
    /// </summary>
    public void EnableMovement()
    {
        moveSpeed = originalSpeed;
    }

    /// <summary>
    /// 指定方向への移動をブロックする
    /// </summary>
    /// <param name="blockDirection">ブロックする方向（例：Vector3.right, Vector3.forward）</param>
    public void BlockMovement(Vector3 blockDirection)
    {
        if (!blockedDirections.Contains(blockDirection))
        {
            blockedDirections.Add(blockDirection);
        }
    }

    /// <summary>
    /// 指定方向へのブロックを解除する
    /// </summary>
    /// <param name="blockDirection">解除するブロック方向</param>
    public void UnblockMovement(Vector3 blockDirection)
    {
        if (blockedDirections.Contains(blockDirection))
        {
            blockedDirections.Remove(blockDirection);
        }
    }
}
