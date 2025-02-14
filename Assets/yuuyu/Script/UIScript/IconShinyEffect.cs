using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coffee.UIExtensions;

public class IconShinyEffect : MonoBehaviour
{

    [SerializeField] float effectTime;
    ShinyEffectForUGUI m_shiny;
    float time=0;
    
    void Start()
    {
        m_shiny=this.GetComponent<ShinyEffectForUGUI>();
         
        // 指定した秒数かけて再生
        //m_shiny.Play( 1 );
        
        // 指定した秒数かけて再生（タイムスケールを無視）
       //m_shiny.Play( 1, AnimatorUpdateMode.UnscaledTime );
    }

    
    void Update()
    {
        time+=Time.deltaTime;
        if(time>1.0f)
        {
            // 指定した秒数かけて再生
            m_shiny.Play( effectTime );
            time=0;
        }
    }
}
