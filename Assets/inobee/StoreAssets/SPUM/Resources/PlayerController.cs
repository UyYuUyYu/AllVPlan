using UnityEngine;

[RequireComponent(typeof(SPUM_Prefabs))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private SPUM_Prefabs spum;

    void Start()
    {
        // 同じオブジェクトについているSPUM_Prefabsを取得
        spum = GetComponent<SPUM_Prefabs>();

        // アニメーションリスト＆OverrideControllerの初期化
        spum.PopulateAnimationLists();
        spum.OverrideControllerInit();
    }

    void Update()
    {
        float horizontal = 0f;

        // 左右キーのみ入力をチェック (押している間だけ動く)
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            horizontal = -1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            horizontal = 1f;
        }

        // 左右移動＆アニメーション
        if (Mathf.Abs(horizontal) > 0f)
        {
            // 左右に移動
            transform.Translate(Vector3.right * horizontal * moveSpeed * Time.deltaTime);
            // Moveアニメーションを再生 (例としてインデックスは0)
            spum.PlayAnimation(PlayerState.MOVE, 0);
        }
        else
        {
            // 何も押していないときはIDLEアニメ
            spum.PlayAnimation(PlayerState.IDLE, 0);
        }

        // -------- Attack アニメーション（6つ） --------
        // AttackList に6個のクリップがある前提で、キー1〜6で再生
        if (Input.GetKeyDown(KeyCode.Alpha1))
            spum.PlayAnimation(PlayerState.ATTACK, 0);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            spum.PlayAnimation(PlayerState.ATTACK, 1);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            spum.PlayAnimation(PlayerState.ATTACK, 2);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            spum.PlayAnimation(PlayerState.ATTACK, 3);

        if (Input.GetKeyDown(KeyCode.Alpha5))
            spum.PlayAnimation(PlayerState.ATTACK, 4);

        if (Input.GetKeyDown(KeyCode.Alpha6))
            spum.PlayAnimation(PlayerState.ATTACK, 5);

        // -------- Other アニメーション（3つ） --------
        // OtherList に3個のクリップがある前提で、キー7〜9で再生
        if (Input.GetKeyDown(KeyCode.Alpha7))
            spum.PlayAnimation(PlayerState.OTHER, 0);

        if (Input.GetKeyDown(KeyCode.Alpha8))
            spum.PlayAnimation(PlayerState.OTHER, 1);

        if (Input.GetKeyDown(KeyCode.Alpha9))
            spum.PlayAnimation(PlayerState.OTHER, 2);
    }
}
