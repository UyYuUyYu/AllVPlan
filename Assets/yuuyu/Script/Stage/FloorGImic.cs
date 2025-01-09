using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGImic : MonoBehaviour
{
    GameObject player;
    BoxCollider floorCollider;
    bool idCheckPassing=false;
    bool isEnterObj=false;

    float offset;
    // Start is called before the first frame update
    void Start()
    {
        floorCollider=this.GetComponent<BoxCollider>();
        player=GameObject.Find("Player");
        offset=(player.transform.localScale.y/2)+(this.transform.localScale.y/2);

    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y > (this.transform.position.y+offset))
        {
            // 上にいる場合はコライダーを有効化
            floorCollider.isTrigger = false;
        }
        else
        {
            // 下にいる場合はコライダーを無効化
            floorCollider.isTrigger= true;
        }
    }
    /*
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isEnterObj=true;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player")&&idCheckPassing)
        {
            floor.GetComponent<BoxCollider>().isTrigger =false;
        }
    }
    void OnTriggerExit(Collider other)
    {

        if(other.CompareTag("Player"))
        {
            if(isEnterObj)
                idCheckPassing=true;
            if(idCheckPassing)
            {
                floor.GetComponent<BoxCollider>().isTrigger =true;
            }
            isEnterObj=false;    
        }
    }
    */
}
