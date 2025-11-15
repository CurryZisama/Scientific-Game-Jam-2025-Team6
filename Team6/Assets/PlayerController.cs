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

    [SerializeField] private AudioSource pick;
    [SerializeField] private AudioSource createCrystal;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public static int CO2Score;
    public static int ConcreteScore;

    bool inCrystalZone = false;
    float zoneTimer = 0f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        moveSpeed = StartMoveSpeed;
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(x, y, 0);

        // 斜め移動を正規化
        if (move.magnitude > 0f)
            move = move.normalized;

        transform.Translate(move * moveSpeed * Time.deltaTime);

        // 画面内に閉じ込める
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -8.33f, 8.33f);
        pos.y = Mathf.Clamp(pos.y, -4.08f, 4.08f);
        transform.position = pos;

        // 左右反転
        if (x < 0) spriteRenderer.flipX = false;
        else if (x > 0) spriteRenderer.flipX = true;

        // アニメ再生/停止
        bool isWalking = x != 0 || y != 0;
        animator.speed = isWalking ? 1f : 0f;

        // 移動速度調整
        float weightedScore = CO2Score * CO2Weight + ConcreteScore * ConcreteWeight;
        float t = Mathf.Clamp01(weightedScore / MaxScore);
        moveSpeed = Mathf.Lerp(StartMoveSpeed, MinMoveSpeed, t);

        // クリスタルゾーン処理
        if (inCrystalZone)
        {
            if (zoneTimer == 0f && CO2Score > 0 && ConcreteScore > 0)
            {
                // 作り始めた瞬間に音を鳴らす
                createCrystal.time = 0f;
                createCrystal.Play();
            }

            if (CO2Score > 0 && ConcreteScore > 0)
            {
                zoneTimer += Time.deltaTime;
                UpdateCrystalAlpha();
            }
                
            if (zoneTimer >= CreateCrystalTime)
            {
                // タイマー終了 → 実際にクリスタル生成
                DoCrystalReaction();
                zoneTimer = 0f;
                createCrystal.Stop(); // 再生途中なら止める
            }
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
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
            inCrystalZone = true;
            zoneTimer = 0f;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("CrystalZone"))
        {
            createCrystal.Stop();
            inCrystalZone = false;
            zoneTimer = 0f;
            UpdateCrystalAlpha();
        }
    }

    void GetCO2(int count)
    {
        CO2Score += count;
        CO2ScoreUI.text = CO2Score.ToString();
        if (count > 0) pick.Play();
    }

    void GetConcrete(int count)
    {
        ConcreteScore += count;
        ConcreteScoreUI.text = ConcreteScore.ToString();
        if (count > 0) pick.Play();
    }

    void DoCrystalReaction()
    {
        if (CO2Score > 0 && ConcreteScore > 0)
        {
            // スコアを減らす
            GetCO2(-1);
            GetConcrete(-1);

            zoneTimer = 0f;
            UpdateCrystalAlpha();

            // クリスタル生成
            GameObject obj = Instantiate(crystalPrefab, crystalSprite.transform.position, crystalSprite.transform.rotation);
            var script = obj.GetComponent<MoveAndShrinkBySpeed>();
            if (script != null)
            {
                script.crystalUI = crystalUI;
            }

            // 生成時に音を鳴らす
            createCrystal.time = 0f; // Clip の再生位置を最初に戻す
            createCrystal.Play();
        }
    }

    private void UpdateCrystalAlpha()
    {
        if (crystalSprite == null) return;
        float alpha = Mathf.Clamp01(zoneTimer / CreateCrystalTime);
        Color color = crystalSprite.color;
        color.a = alpha;
        crystalSprite.color = color;
    }
}
