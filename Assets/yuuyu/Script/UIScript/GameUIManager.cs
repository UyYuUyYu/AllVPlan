using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] GameObject[] KoeruPowerPanel;
    [SerializeField] GameObject[] PlayerHPPanel;
    [SerializeField] GameObject specialPanel;
    [SerializeField] float seconds = 1f;

    void Start()
    {
    }

    void Update()
    {
    }

    public void ChangeKoeruPowerPanel(int _nowKoeruPower, int _maxKoeruPower)
    {
        for (int i= 1; i <= _maxKoeruPower; i++)
        {
            if (i <= _nowKoeruPower)
                KoeruPowerPanel[i-1].GetComponent<Image>().enabled = true;
            else
                KoeruPowerPanel[i-1].GetComponent<Image>().enabled = false;
        }
    }

    public void ChangeHPPanel(int _nowPlayerHP, int _maxPlayerHP)
    {
        for (int i= 1; i <= _maxPlayerHP; i++)
        {
            if (i <= _nowPlayerHP)
                PlayerHPPanel[i-1].GetComponent<Image>().enabled = true;
            else
                PlayerHPPanel[i-1].GetComponent<Image>().enabled = false;
        }
    }

    // ──────────────────────────────────────────────────────────
    // ▼▼▼ ここから追加：パネルを数秒だけ表示するメソッド ▼▼▼
    // ──────────────────────────────────────────────────────────

    /// <summary>
    /// 指定したパネルを指定秒数だけアクティブにする
    /// </summary>
    /// <param name="panel">対象パネル</param>
    /// <param name="seconds">表示する秒数</param>
    public void ShowPanelForSeconds()
    {
        // コルーチンを呼び出し
        StartCoroutine(ShowPanelCoroutine(specialPanel, seconds));
    }

    /// <summary>
    /// 実際にパネルを表示 → 数秒待機 → 非表示に戻すコルーチン
    /// </summary>
    IEnumerator ShowPanelCoroutine(GameObject panel, float duration)
    {
        // パネルをアクティブに
        panel.SetActive(true);

        // 指定時間待機
        yield return new WaitForSeconds(duration);

        // パネルを非アクティブに
        panel.SetActive(false);
    }
}
