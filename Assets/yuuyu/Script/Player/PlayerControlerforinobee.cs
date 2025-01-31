using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlerforinobee : MonoBehaviour
{
    PlayerInput playerInput;
    [SerializeField] float moveSpeed = 2;
    [SerializeField] float jumpPower = 10f;
    [SerializeField] int jumpCount = 1;
    int nowJumpCount;
    [SerializeField] int playerHP = 3;
    Rigidbody prayerRb;
    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        prayerRb = this.GetComponent<Rigidbody>();
        nowJumpCount = jumpCount;
        direction = new Vector3(0, 0, 0);
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
        #region コントローラー
        transform.position += direction * moveSpeed * Time.deltaTime;
        #endregion

        #region キー入力
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, 0,moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= new Vector3(0, 0,moveSpeed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.Space) && (nowJumpCount > 0))
        {
            prayerRb.velocity = Vector3.up * jumpPower;
            nowJumpCount--;
        }
        #endregion
    }

    void OnMove(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();
        direction = new Vector3(value.x, value.y, 0).normalized;
    }

    void OnMoveStop(InputAction.CallbackContext context)
    {
        direction = Vector3.zero;
    }

    void OnJump(InputAction.CallbackContext context)
    {
        prayerRb.velocity = Vector3.up * jumpPower;
        nowJumpCount--;
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
}
