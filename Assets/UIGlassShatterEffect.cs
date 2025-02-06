using UnityEngine;
using UnityEngine.UI;

public class UIGlassShatterEffect : MonoBehaviour
{
    [Tooltip("UIガラス破片シェーダーが設定されたマテリアル（ImageコンポーネントのMaterialに設定）")]
    public Material uiGlassShatterMaterial;

    [Tooltip("破壊エフェクトが完了するまでの時間")]
    public float shatterDuration = 2f;

    private float timer = 0f;
    private bool shatterTriggered = false;

    void Update()
    {
        if (shatterTriggered)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / shatterDuration);
            // シェーダーの _TimeFactor を更新
            uiGlassShatterMaterial.SetFloat("_TimeFactor", t);

            // 完全に破壊演出が終了したら停止
            if (t >= 1f)
            {
                shatterTriggered = false;
            }
        }
    }

    /// <summary>
    /// 外部から破壊演出を開始するための関数
    /// </summary>
    public void TriggerShatter()
    {
        timer = 0f;
        shatterTriggered = true;
    }
}
