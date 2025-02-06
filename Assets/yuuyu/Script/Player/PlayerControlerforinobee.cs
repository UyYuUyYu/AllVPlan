using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    // InputSystemで得た方向
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
        // 既存のInputSystemからの入力（direction）と
        // キーボードの入力（W,A,S,D）を合算する
        rawInput = direction;  // InputSystemでの入力（2Dベクトル→x, yは使い回すので注意）
        
        // キーボード入力（※InputSystemと重複する場合は調整してください）
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

        // 入力が複数ある場合に正規化して方向だけを取り出す
        rawInput = rawInput.normalized;

        // ブロックされている方向の成分を除去する
        Vector3 effectiveInput = rawInput;
        foreach (Vector3 blockDir in blockedDirections)
        {
            // ブロックする方向は正規化しておく
            Vector3 normalized = blockDir.normalized;
            // もし入力がブロック方向に向かっているなら、その成分を除去する
            float dot = Vector3.Dot(effectiveInput, normalized);
            if (dot > 0)
            {
                Vector3 projection = Vector3.Project(effectiveInput, normalized);
                effectiveInput -= projection;
            }
        }

        // 有効な入力方向で移動する
        transform.position += effectiveInput * moveSpeed * Time.deltaTime;

        // キー入力によるジャンプ処理やその他の処理はそのまま
        if (Input.GetKeyDown(KeyCode.Space) && (nowJumpCount > 0))
        {
            prayerRb.velocity = Vector3.up * jumpPower;
            nowJumpCount--;
        }
    }

    void OnMove(InputAction.CallbackContext context)
    {
        // InputSystemのMoveアクションから取得（Vector2→Vector3に変換）
        var value = context.ReadValue<Vector2>();
        // ここでは、x軸はx、y軸はzとして扱います
        direction = new Vector3(value.x, 0, value.y).normalized;
    }

    void OnMoveStop(InputAction.CallbackContext context)
    {
        direction = Vector3.zero;
    }

    void OnJump(InputAction.CallbackContext context)
    {
        // ジャンプ処理（衝突時と重複しますが、InputSystemでのジャンプも対応）
        if (nowJumpCount > 0)
        {
            prayerRb.velocity = Vector3.up * jumpPower;
            nowJumpCount--;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        string tagname = collision.gameObject.tag;
        switch (tagname)
        {
            case "Grand":
                nowJumpCount = jumpCount;
                break;
            case "Bullet":
                playerHP--;
                break;
            default:
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "energy")
        {
            toggleEnabledCounter.StartCutin();
            Destroy(other.gameObject);
        }
    }

    /// <summary>
    /// 既存の動きを完全に停止させる（moveSpeedを0にする）
    /// ※ブロック方向機能とは別に全停止が必要な場合用
    /// </summary>
    public void DisableMovement()
    {
        moveSpeed = 0f;
    }

    /// <summary>
    /// 移動を再開する（元のスピードに戻す）
    /// ※ブロック方向機能とは別に全解除が必要な場合用
    /// </summary>
    public void EnableMovement()
    {
        moveSpeed = originalSpeed;
    }

    /// <summary>
    /// 指定方向への移動をブロックする
    /// 例：壁側のスクリプトから呼び出される
    /// </summary>
    /// <param name="direction">ブロックする方向（Vector3.right, Vector3.forwardなど）</param>
    public void BlockMovement(Vector3 blockDirection)
    {
        // 同じ方向が登録済みでなければ追加
        if (!blockedDirections.Contains(blockDirection))
        {
            blockedDirections.Add(blockDirection);
        }
    }

    /// <summary>
    /// 指定方向へのブロックを解除する
    /// </summary>
    /// <param name="direction">解除するブロック方向</param>
    public void UnblockMovement(Vector3 blockDirection)
    {
        if (blockedDirections.Contains(blockDirection))
        {
            blockedDirections.Remove(blockDirection);
        }
    }
}
