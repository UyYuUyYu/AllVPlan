using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TextController : MonoBehaviour
{
    // セリフを表示するUI Text（Canvas上に配置したTextオブジェクト）
    [SerializeField] private Text dialogueText;
    /// <summary>
    /// DamageVoidが呼ばれると、指定されたセリフを表示し、
    /// オブジェクトの表示→縮小、拡大縮小のアニメーションを実行します。
    /// </summary>
    /// <param name="dialogue">表示するセリフ（台詞）
    /// </param>

    /// <summary>
    /// 引数で渡されたセリフをUI Textに表示します。
    /// </summary>
    /// <param name="dialogue">表示するセリフ</param>
    public void ShowDialogue(string dialogue)
    {
        if (dialogueText == null)
        {
            Debug.LogWarning("Textが設定されていません");
            return;
        }
        dialogueText.text = dialogue;
    }
}
