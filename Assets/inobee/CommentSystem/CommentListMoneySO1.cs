using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CommentListData", menuName = "Comments/CommentListMoney", order = 1)]
public class CommentListMoneySO1 : ScriptableObject
{
    [System.Serializable]
    public class CommentData
    {
        public string name;     // ユーザー名
        public int amount;      // 金額（整数）
    }

    public List<CommentData> MoneyList = new List<CommentData>();
}
