using UnityEngine;

public class UmbrellaController : MonoBehaviour
{
    [Header("落下速度")]
    public float fallSpeed = 1f;

    [Header("左右にふわふわ揺れる：サイン波の振幅(0で無効)")]
    public float swingDistance = 1f;

    [Header("左右にふわふわ揺れる：サイン波の周波数(0で無効)")]
    public float swingFrequency = 1f;

    [Header("くるくる回転速度")]
    public float rotationSpeed = 50f;

    // 生成時の初期座標
    private float originalX;
    // Z座標を固定
    private float fixedZ;

    void Start()
    {
        // 生成時のX,Z座標を記録
        originalX = transform.position.x;
        fixedZ    = transform.position.z;
    }

    void Update()
    {
        // ==== 落下処理 ====
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        // ==== 左右のふわふわ揺れ（サイン波） ====
        float currentX = originalX;
        if (swingFrequency != 0f && swingDistance != 0f)
        {
            float sinValue = Mathf.Sin(Time.time * swingFrequency) * swingDistance;
            currentX += sinValue;
        }

        // Z座標は固定
        transform.position = new Vector3(currentX, transform.position.y, fixedZ);

        // ==== くるくる回転 ====
        // Z軸回転を例に
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
