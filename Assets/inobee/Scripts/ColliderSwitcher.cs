using UnityEngine;
using System.Collections;

public class ColliderSwitcher : MonoBehaviour
{
    [Header("対象のColliderをアサイン")]
    // 実際の物理判定用のMeshCollider
    public MeshCollider meshCollider;
    // 衝突判定用のCapsuleCollider（Is Triggerにチェック）
    public CapsuleCollider capsuleCollider;

    // 衝突中のオブジェクトの名前を保持する変数
    private string collidingObjectName = "";

    // 自身および子要素も含めたRendererの配列
    private Renderer[] objRenderers;

    // 点滅の設定
    [Header("点滅設定")]
    [Tooltip("点滅回数（オン＋オフで1サイクル）")]
    public int flashCount = 5;
    [Tooltip("1回の点滅の間隔（秒）")]
    public float flashInterval = 0.2f;

    void Start()
    {
        // Colliderの自動取得
        if (meshCollider == null)
            meshCollider = GetComponent<MeshCollider>();
        if (capsuleCollider == null)
            capsuleCollider = GetComponent<CapsuleCollider>();

        // 自身および子要素内の全Rendererを取得
        objRenderers = GetComponentsInChildren<Renderer>();
        if (objRenderers.Length == 0)
        {
            Debug.LogWarning("Rendererコンポーネントが見つかりません。点滅機能は動作しません。");
        }
    }

    /// <summary>
    /// MeshColliderに衝突したときの処理
    /// </summary>
   void OnCollisionEnter(Collision collision)
{
    Debug.Log("衝突したオブジェクト: " + collision.gameObject.name);

    if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Grand"))
    {
        meshCollider.enabled = false;
        capsuleCollider.enabled = false;
        StartCoroutine(FlashAndDestroy());
        if (collision.gameObject.CompareTag("Player")){
        collision.gameObject.GetComponent<PlayerControlerforinobee>().Damage(1);
        }
    }
}


    /// <summary>
    /// プレイヤーがCapsuleCollider内に入ったときの処理
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        collidingObjectName = other.gameObject.name;

        if (other.CompareTag("Player"))
        {
            if (meshCollider.enabled)
            {
                meshCollider.enabled = false;

            }
        }
        else
        {
            Debug.Log("衝突相手: " + other.gameObject.name);
        }
    }

    /// <summary>
    /// プレイヤーがCapsuleColliderから出たときの処理
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!meshCollider.enabled)
            {
                meshCollider.enabled = true;

            }
        }

        if (collidingObjectName == other.gameObject.name)
        {
            collidingObjectName = "";
        }
    }

    /// <summary>
    /// 点滅させてからオブジェクトをDestroyするコルーチン
    /// </summary>
    IEnumerator FlashAndDestroy()
    {
        if (objRenderers == null || objRenderers.Length == 0)
        {
            Destroy(gameObject);
            yield break;
        }

        for (int i = 0; i < flashCount; i++)
        {

            foreach (Renderer r in objRenderers)
            {
                r.enabled = false;
            }
            yield return new WaitForSeconds(flashInterval);


            foreach (Renderer r in objRenderers)
            {
                r.enabled = true;
            }
            yield return new WaitForSeconds(flashInterval);
        }

        // Destroy前にRendererをオンに戻す
        foreach (Renderer r in objRenderers)
        {
            r.enabled = true;
        }
        Destroy(gameObject);
    }
}
