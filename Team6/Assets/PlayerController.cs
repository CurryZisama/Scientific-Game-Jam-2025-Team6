using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float StartMoveSpeed = 5f;
    private float moveSpeed;

    public int MaxScore = 100;     // 重みに応じた最大スコア
    public float MinMoveSpeed = 1f;

    // スコアの重み
    public float CO2Weight = 0.3f;       // CO2は軽め
    public float ConcreteWeight = 1.0f;  // Concreteは重め

    [SerializeField] private Text CO2ScoreUI;
    [SerializeField] private Text ConcreteScoreUI;
    [SerializeField] private float CreateCrystalTime = 1f;
    [SerializeField] private SpriteRenderer crystalSprite;

    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private Text crystalUI;    // Scene 上の UI

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public static int CO2Score;
    public static int ConcreteScore;

    bool inCrystalZone = false;
    float zoneTimer = 0f;

    private void Start()
    {
        // 自分の GameObject についている Animator を取得
        animator = GetComponent<Animator>();

        // SpriteRenderer も同じく取得
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(x, y, 0);
        transform.Translate(move * moveSpeed * Time.deltaTime);

        // 左右反転
        if (x < 0)
            spriteRenderer.flipX = false; // 右向き
        else if (x > 0)
            spriteRenderer.flipX = true;  // 左向き

        bool isWalking = x != 0 || y != 0;

        // Animator の speed でアニメーション再生 / 停止
        if (isWalking)
            animator.speed = 1f; // 通常速度で再生
        else
            animator.speed = 0f; // 停止 → 最後のフレームで止まる

        // --- 速度計算やクリスタル処理はそのまま ---
        float weightedScore = CO2Score * CO2Weight + ConcreteScore * ConcreteWeight;
        float t = Mathf.Clamp01(weightedScore / MaxScore);
        moveSpeed = Mathf.Lerp(StartMoveSpeed, MinMoveSpeed, t);

        if (inCrystalZone)
        {
            zoneTimer += Time.deltaTime;
            if (CO2Score > 0 && ConcreteScore > 0)
            {
                UpdateCrystalAlpha();
            }

            if (zoneTimer >= CreateCrystalTime)
            {
                DoCrystalReaction();
                zoneTimer = 0f;
            }
        }
    }

    // --- 2Dトリガー ---
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
        if (other.CompareTag("CO2"))
        {
            GetCO2(1);
            Destroy(other.gameObject);
            return;
        }

        if (other.CompareTag("Concrete"))
        {
            GetConcrete(1);
            Destroy(other.gameObject);
            return;
        }

        if (other.CompareTag("CrystalZone"))
        {
            Debug.Log("CrystalZone");
            inCrystalZone = true;
            zoneTimer = 0f;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("CrystalZone"))
        {
            inCrystalZone = false;
            zoneTimer = 0f;
            UpdateCrystalAlpha();
        }
    }

    void GetCO2(int count)
    {
        CO2Score += count;
        CO2ScoreUI.text = CO2Score.ToString();
    }

    void GetConcrete(int count)
    {
        ConcreteScore += count;
        ConcreteScoreUI.text = ConcreteScore.ToString();
    }

    void DoCrystalReaction()
    {
        if (CO2Score > 0 && ConcreteScore > 0)
        {
            GetCO2(-1);
            GetConcrete(-1);
            GameObject obj = Instantiate(crystalPrefab, crystalSprite.gameObject.transform.position, crystalSprite.gameObject.transform.rotation);
            var script = obj.GetComponent<MoveAndShrinkBySpeed>();
            if (script != null)
            {
                script.crystalUI = crystalUI;
            }

        }
    }

    private void UpdateCrystalAlpha()
    {
        if (crystalSprite == null) return;

        // 0〜1の範囲に正規化
        float alpha = Mathf.Clamp01(zoneTimer / CreateCrystalTime);

        Color color = crystalSprite.color;
        color.a = alpha;
        crystalSprite.color = color;
    }
}
