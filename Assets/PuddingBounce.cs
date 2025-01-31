using UnityEngine;

public class PuddingBounce : MonoBehaviour
{
    [Header("Jiggle Settings")]
    [SerializeField] private float jiggleSpeed = 5f;    // プルプルの速さ
    [SerializeField] private float jiggleAmount = 0.05f;// プルプルの大きさ

    [Header("Bounce Settings")]
    [SerializeField] private float bounceForce = 10f;   // ジャンプさせる力

    private Vector3 initialScale;  // 初期の大きさ

    void Start()
    {
        // ゲームオブジェクトの初期スケールを記憶しておく
        initialScale = transform.localScale;
    }

    void Update()
    {
        // サイン波を使って大きさを少しずつ変化させる
        float scaleOffset = Mathf.Sin(Time.time * jiggleSpeed) * jiggleAmount;
        transform.localScale = initialScale + new Vector3(scaleOffset, scaleOffset, scaleOffset);
    }

    // 他のオブジェクトが衝突してきたとき
    private void OnCollisionEnter(Collision collision)
    {
        // 例えば「Player」タグのオブジェクトに対して
        if (collision.gameObject.CompareTag("Player"))
        {
            // Rigidbodyがアタッチされていたら上方向へ速度を与える
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // 既存のx,z方向の速度は保持しつつy方向だけ上書き
                rb.velocity = new Vector3(rb.velocity.x, bounceForce, rb.velocity.z);
            }
        }
    }
}
