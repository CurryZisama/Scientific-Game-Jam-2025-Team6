using UnityEngine;

public class MagnesiumMove : MonoBehaviour
{
    [SerializeField] private float fadeSpeed = 1.0f; // 1秒でαが1減るイメージ

    private SpriteRenderer sr;
    private Color col;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = sr.color;  // 初期色を取得
    }
    void Update()
    {
        // αを減らす
        col.a -= fadeSpeed * Time.deltaTime;

        // 反映
        sr.color = col;

        // αが0以下なら消去
        if (col.a <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
