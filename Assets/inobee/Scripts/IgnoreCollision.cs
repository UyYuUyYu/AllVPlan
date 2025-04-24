using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
    private Collider myCollider;

    void Awake()
    {
        // 自分のColliderを取得
        myCollider = GetComponent<Collider>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // 衝突したオブジェクトが「Hoip」タグを持っている場合
        if (collision.gameObject.CompareTag("Hoip"))
        {
            Debug.Log("衝突したオブジェクトがHoipです");
            Collider hoipCollider = collision.collider;
            // 自分のColliderと衝突したHoipオブジェクトのCollider間の衝突を無視する
            Physics.IgnoreCollision(myCollider, hoipCollider, true);
        }
    }
}
