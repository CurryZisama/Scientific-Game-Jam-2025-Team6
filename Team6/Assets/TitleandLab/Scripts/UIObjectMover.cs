using UnityEngine;

public class UIObjectMover : MonoBehaviour
{
    // インスペクターで設定する移動速度
    public float moveSpeed = 100f; 

    private RectTransform rectTransform;
    private float destroyXPosition; // このX座標より左に行ったら消える

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        // 親オブジェクト（＝移動エリアのパネル）のRectTransformを取得
        RectTransform parentRect = transform.parent.GetComponent<RectTransform>();

        // 親の左端の座標を計算（PivotやAnchorが中央(0.5, 0.5)の場合）
        // 画面外に出た瞬間に消すため、自身の幅も考慮するとより正確ですが、
        // まずは親の端を基準にします。
        destroyXPosition = -(parentRect.rect.width / 2f);
    }

    void Update()
    {
        // 1. 左に移動させる
        // anchoredPositionを直接変更して移動します
        rectTransform.anchoredPosition -= new Vector2(moveSpeed * Time.deltaTime, 0);

        // 2. 画面外（親パネルの左端）に出たか判定
        if (rectTransform.anchoredPosition.x <= destroyXPosition)
        {
            // 画面外に出たら、自分自身を破棄（消去）する
            Destroy(gameObject);
        }
    }
}