using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaayerControler : MonoBehaviour
{
    
    [SerializeField]float moveSpeed=2;
    [SerializeField]float jumpPower=10f;
    [SerializeField] int jumpCount=1;
    int nowJumpCount;
    Rigidbody prayerRb;
    // Start is called before the first frame update
    void Start()
    {
        prayerRb=this.GetComponent<Rigidbody>();
        nowJumpCount=jumpCount;
    }

    // Update is called once per frame
    void Update()
    {
        #region キー入力
        if(Input.GetKey(KeyCode.D))
        {
            transform.position+=new Vector3(moveSpeed*Time.deltaTime,0,0);
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.position-=new Vector3(moveSpeed*Time.deltaTime,0,0);
        }
        if(Input.GetKeyDown(KeyCode.Space)&&(nowJumpCount>0))
        {
            prayerRb.velocity = Vector3.up * jumpPower;
            nowJumpCount--;
            //prayerRb.AddForce(new Vector3(0,jumpPower,0),ForceMode.Impulse);
        }
        #endregion

    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Grand"))
        {
            nowJumpCount=jumpCount;
        }
    }
}
