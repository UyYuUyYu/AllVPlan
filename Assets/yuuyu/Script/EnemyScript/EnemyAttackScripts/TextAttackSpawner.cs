using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAttackSpawner : MonoBehaviour
{
    [SerializeField] GameObject attackTextPrefab;
    [SerializeField] Transform[] generatePositions;
    // Start is called before the first frame update

    //攻撃してくるTextのtextとスピードと生成位置を指定してAttckTextを生成する
    public void GenerateAttackText(string _text,float _speed,int _posNum)
    {
        GameObject atcckTextObj = Instantiate(attackTextPrefab,generatePositions[_posNum].position,Quaternion.identity);
        atcckTextObj.GetComponent<TextAttack>().SetUpText(_text,_speed);
    }
    [ContextMenu("gene")]
    public void DebugAttckText()
    {
        GenerateAttackText("あああaaaaa",2.0f,0);
    }
}
