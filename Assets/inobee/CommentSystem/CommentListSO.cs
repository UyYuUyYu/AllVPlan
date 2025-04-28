using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CommentListData", menuName = "Comments/CommentList", order = 1)]
public class CommentListSO : ScriptableObject
{
    [System.Serializable]
    public class CommentData
    {
        public string name;
        public string message;
        public Sprite avatar;
        public Color BGColor; // Add color property for the name
    }

    public List<CommentData> normalList = new List<CommentData>();
    public List<CommentData> damageList = new List<CommentData>();

    public List<CommentData> attackList = new List<CommentData>();
}
