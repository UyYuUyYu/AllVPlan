using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEffect : MonoBehaviour
{
    [SerializeField] float[] effectSize;
    [SerializeField] GameObject enemyEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EnemyScaleChange(int _num)
    {
        enemyEffect.transform.localScale=new Vector3(effectSize[_num],effectSize[_num],effectSize[_num]);
    }

}
