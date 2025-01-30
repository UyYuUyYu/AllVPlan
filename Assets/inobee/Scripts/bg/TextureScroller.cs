using UnityEngine;

public class TextureScroller : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 0.5f;  // スクロール速度
    private Renderer _renderer;
    private float _offset;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        // 時間を加算してOffsetを決める
        _offset += Time.deltaTime * scrollSpeed;
        
        // メインテクスチャのUVを右方向にずらす
        _renderer.material.SetTextureOffset("_MainTex", new Vector2(_offset, 0));
    }
}
