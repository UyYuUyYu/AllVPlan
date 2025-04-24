using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [Tooltip("弾の移動速度")]
    public float speed = 10f;

    void Update()
    {
        // 発射時の向きに沿って直進移動
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    // 衝突時に呼び出されるメソッド（Collider同士が衝突した場合）
    private void OnCollisionEnter(Collision collision)
    {
        // 衝突したオブジェクトとその名前をログに出力
        Debug.Log("Bullet hit: " + collision.gameObject.name);
    }
    private void  OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            Debug.Log("Player hit by bullet");
        }
         
    }
}
