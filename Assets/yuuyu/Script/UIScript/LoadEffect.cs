using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadEffect : MonoBehaviour
{
    private Image _image;
    float countTime;
    // Start is called before the first frame update
    void Start()
    {
        _image = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        countTime+=Time.deltaTime;
        _image.fillAmount = countTime / 4.0f;
    }
}
