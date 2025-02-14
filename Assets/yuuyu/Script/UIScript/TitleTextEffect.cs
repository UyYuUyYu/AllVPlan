using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleTextEffect : MonoBehaviour
{
    public float speed = 0.5f;
    private float time;
    private Text text;
   
    void Start()
    {
        text = this.gameObject.GetComponent<Text>();
    }

    
    void Update()
    {
        text.color = GetTextColorAlpha(text.color);
    }
    Color GetTextColorAlpha(Color color)
    {
        time += Time.deltaTime * speed * 5.0f;
        color.a = Mathf.Sin(time)+1.0f;

        return color;
    }
}
