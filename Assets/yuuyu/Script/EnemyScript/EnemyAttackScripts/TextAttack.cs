using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAttack : MonoBehaviour
{
    [SerializeField]float moveSpeed=2.0f;
    private bool isStartAttack=false;
    private TextMesh textMesh;
    private BoxCollider boxCollider;

    // Update is called once per frame

    void Awake()
    {
       
    }
    void Update()
    {
        if(isStartAttack)
            transform.position+=new Vector3(-moveSpeed*Time.deltaTime,0,0);
    }
    public void SetUpText(string _text,float _speed)
    {
        textMesh = GetComponent<TextMesh>();
        boxCollider = GetComponent<BoxCollider>();

        moveSpeed=_speed;
        textMesh.text=_text;
        isStartAttack=true;

        textMesh.text = textMesh.text; // 再評価を促す
        textMesh.GetComponent<Renderer>().enabled = false;
        textMesh.GetComponent<Renderer>().enabled = true;

        Bounds bounds = textMesh.GetComponent<Renderer>().bounds;
        float z=boxCollider.size.z;
        print(z);
        Vector3 size = bounds.size;
        size.z=z;

        // BoxColliderのサイズと中心をRendererのBoundsに合わせる
        boxCollider.size = size;
        boxCollider.center = textMesh.transform.InverseTransformPoint(bounds.center);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
            other.gameObject.GetComponentInParent<PlayerControler>().Damege();
            Destroy(this.gameObject);
        }

    }
}
