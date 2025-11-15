using UnityEngine;

public class BounceObject : MonoBehaviour
{
    public float speed = 5f; // 移動速度
    private Rigidbody2D rb;
    private Camera mainCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;

        // ランダム方向の単位ベクトルを作り、速度をかける
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
        rb.velocity = direction * speed;
    }

    void Update()
    {
        BounceAtScreenEdges();
    }

    void BounceAtScreenEdges()
    {
        Vector2 pos = transform.position;
        Vector2 screenMin = mainCamera.ViewportToWorldPoint(Vector2.zero);
        Vector2 screenMax = mainCamera.ViewportToWorldPoint(Vector2.one);

        if ((pos.x < screenMin.x && rb.velocity.x < 0) || (pos.x > screenMax.x && rb.velocity.x > 0))
        {
            rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
        }

        if ((pos.y < screenMin.y && rb.velocity.y < 0) || (pos.y > screenMax.y && rb.velocity.y > 0))
        {
            rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;
        rb.velocity = Vector2.Reflect(rb.velocity, normal);
    }
}
