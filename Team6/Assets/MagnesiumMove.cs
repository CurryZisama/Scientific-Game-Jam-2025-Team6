using UnityEngine;

public class MagnesiumMove : MonoBehaviour
{
    [SerializeField] private float speed = 10f;   // 移動速度
    [SerializeField] private int maxBounce = 6;   // この回数バウンドしたら画面外へ飛ばす
    private Transform PlayerTransform;

    private Vector2 direction;   // 進行方向
    private SpriteRenderer sr;
    private int bounceCount = 0;
    private bool startExit = false; // 画面外に飛ばすモード

    // 画面端（余裕を1ユニット追加）
    private const float minX = -8.392f;
    private const float maxX = 8.392f;
    private const float minY = -4.476f;
    private const float maxY = 4.476f;
    private const float margin = 1f; // 余裕

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        // Find で Player オブジェクトを探す
        GameObject obj = GameObject.Find("Player");
        if (obj != null)
        {
            PlayerTransform = obj.transform;
        }
        else
        {
            Debug.LogWarning("Player が見つからなかったよ！");
            // ここで fallback してもOK
            PlayerTransform = transform; // とりあえず自分向きとか
        }

        // ターゲットへ向かうベクトル
        Vector2 toTarget = (PlayerTransform.position - transform.position).normalized;

        // 直角方向
        Vector2 right90 = new Vector2(-toTarget.y, toTarget.x);
        Vector2 left90 = new Vector2(toTarget.y, -toTarget.x);

        // ランダムに左右選択
        direction = (Random.value < 0.5f ? right90 : left90).normalized;
    }


    void Update()
    {
        if (!startExit)
        {
            MoveAndBounce();
        }
        else
        {
            MoveOut();
        }
    }

    private void MoveAndBounce()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        Vector3 pos = transform.position;
        bool reflected = false;

        if (pos.x < minX) { pos.x = minX; direction.x *= -1; reflected = true; }
        else if (pos.x > maxX) { pos.x = maxX; direction.x *= -1; reflected = true; }

        if (pos.y < minY) { pos.y = minY; direction.y *= -1; reflected = true; }
        else if (pos.y > maxY) { pos.y = maxY; direction.y *= -1; reflected = true; }

        transform.position = pos;

        if (reflected)
        {
            bounceCount++;
            if (bounceCount >= maxBounce)
            {
                startExit = true;
                // 画面外に飛ぶ方向は現在の方向のまま
                direction = direction.normalized;
            }
        }
    }

    private void MoveOut()
    {
        // 画面外へ直線移動
        transform.Translate(direction * speed * Time.deltaTime);
        Vector3 pos = transform.position;

        // 余裕をもたせた範囲を超えたら削除
        if (pos.x < minX - margin || pos.x > maxX + margin || pos.y < minY - margin || pos.y > maxY + margin)
        {
            Destroy(gameObject);
        }
    }
}
