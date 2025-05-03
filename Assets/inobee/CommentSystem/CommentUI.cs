using UnityEngine;
using UnityEngine.UI;

public class CommentUI : MonoBehaviour
{
    public Image avatarImage;
    public Text nameText;
    public Text messageText;

    // ScriptableObject のデータを受け取って反映
    public void SetData(CommentListSO.CommentData data)
    {
        if (avatarImage != null)
            avatarImage.sprite = data.avatar;

        if (nameText != null)
            nameText.text = data.name;

        if (messageText != null)
            messageText.text = data.message;
    }
}
