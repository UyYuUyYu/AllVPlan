using UnityEngine;

[RequireComponent(typeof(SPUM_Prefabs))]
public class PlayerAnimation : MonoBehaviour
{
    private SPUM_Prefabs spum;

    void Start()
    {
        // 同じオブジェクトにアタッチされている SPUM_Prefabs を取得
        spum = GetComponent<SPUM_Prefabs>();

        // 必要に応じてアニメーションリストを作成・オーバーライドコントローラ初期化
        spum.PopulateAnimationLists();
        spum.OverrideControllerInit();
    }

    /// <summary>
    /// OTHER リストのインデックス 2 のアニメーションを再生
    /// ダメージアニメーション
    /// </summary>
    public void DamageAnimation()
    {
        spum.PlayAnimation(PlayerState.OTHER, 2);
    }
}
