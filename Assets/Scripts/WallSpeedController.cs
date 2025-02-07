using UnityEngine;

public enum BlockDirection
{
    PositiveX,  // +x方向（右方向）
    NegativeX,  // -x方向（左方向）
    PositiveZ,  // +z方向（前方向）
    NegativeZ   // -z方向（後方向）
}

public class WallSpeedController : MonoBehaviour
{
    [Header("ブロックする方向の設定")]
    public BlockDirection blockDirection;

    /// <summary>
    /// BlockDirectionに応じたVector3に変換する
    /// </summary>
    /// <returns>ブロックする方向のベクトル</returns>
    private Vector3 GetBlockVector()
    {
        switch (blockDirection)
        {
            case BlockDirection.PositiveX:
                return Vector3.right;
            case BlockDirection.NegativeX:
                return Vector3.left;
            case BlockDirection.PositiveZ:
                return Vector3.forward;
            case BlockDirection.NegativeZ:
                return Vector3.back;
            default:
                return Vector3.zero;
        }
    }

    // プレイヤーが壁のTrigger Colliderに入ったとき
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControlerforinobee playerMovement = other.GetComponent<PlayerControlerforinobee>();
            if (playerMovement != null)
            {
                playerMovement.BlockMovement(GetBlockVector());
            }
            else
            {
                Debug.LogWarning("PlayerMovementコンポーネントがプレイヤーに見つかりません。");
            }
        }
    }

    // プレイヤーが壁のTrigger Colliderから出たとき
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControlerforinobee playerMovement = other.GetComponent<PlayerControlerforinobee>();
            if (playerMovement != null)
            {
                playerMovement.UnblockMovement(GetBlockVector());
            }
            else
            {
                Debug.LogWarning("PlayerMovementコンポーネントがプレイヤーに見つかりません。");
            }
        }
    }
}
