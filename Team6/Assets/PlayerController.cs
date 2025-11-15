using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // 新 Input System を使用

public class PlayerController : MonoBehaviour
{
    public float StartMoveSpeed = 5f;
    private float moveSpeed;

    public int MaxScore = 100;
    public float MinMoveSpeed = 1f;

    public float CO2Weight = 0.3f;
    public float ConcreteWeight = 1.0f;

    [SerializeField] private Text CO2ScoreUI;
    [SerializeField] private Text ConcreteScoreUI;
    [SerializeField] private float CreateCrystalTime = 1f;

    [SerializeField] private Text crystalUI;
    [SerializeField] private Text rareCrystalUI;

    [SerializeField] private AudioSource pick;
    [SerializeField] private AudioSource createCrystal;
    [SerializeField] private AudioSource rareCreateCrystal;
    [SerializeField] private AudioSource getMagnesium;

    [SerializeField] private SpriteRenderer crystalSprite;      // 普通のクリスタルUI
    [SerializeField] private SpriteRenderer rareCrystalSprite;  // レア鉱石UI
    [SerializeField] private GameObject crystalPrefab;          // 普通のクリスタルPrefab
    [SerializeField] private GameObject rareCrystalPrefab;      // レア鉱石Prefab
    [SerializeField] private float rareSpawnChance = 0.1f;      // レア生成確率

    [SerializeField] private SpriteRenderer GoSprite;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public static int CO2Score;
    public static int ConcreteScore;
    public static int CrystalScore;
    public static int RareCrystalScore;

    bool inCrystalZone = false;
    float zoneTimer = 0f;

    bool willSpawnRare = false;

    bool firstHaveMaterials = true;

    private float alpha = 0f;
    private int direction = 1; // 1: 増加, -1: 減少
    float fadeSpeed = 2f;    // 透明度変化速度

    private ParticleSystem aura;

    private void Start()
    {
        aura = GetComponentInChildren<ParticleSystem>();
        aura.Stop();
        aura.Clear();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        moveSpeed = StartMoveSpeed;
    }

    void Update()
    {
        // 移動入力（新Input System）
        Vector2 moveInput = Vector2.zero;

        if (firstHaveMaterials)
        {
            if(CO2Score > 0 && ConcreteScore > 0)
            {
                // αを変化させる
                alpha += direction * fadeSpeed * Time.deltaTime;

                // 0〜1の範囲に制限
                if (alpha > 1f)
                {
                    alpha = 1f;
                    direction = -1; // 減少に切り替え
                }
                else if (alpha < 0f)
                {
                    alpha = 0f;
                    direction = 1; // 増加に切り替え
                }

                // αを適用
                Color c = GoSprite.color;
                c.a = alpha;
                GoSprite.color = c;

            }
        }

        if (Keyboard.current != null)
        {
            float x = 0f;
            float y = 0f;

            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) x -= 1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) x += 1f;
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) y += 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) y -= 1f;

            moveInput = new Vector2(x, y);
        }

        // ゲームパッド入力
        if (Gamepad.current != null)
        {
            Vector2 stick = Gamepad.current.leftStick.ReadValue();
            moveInput += stick;  // 左スティックの値を足す
        }

        // 正規化
        if (moveInput.magnitude > 1f) moveInput.Normalize();

        // 移動
        transform.Translate(new Vector3(moveInput.x, moveInput.y, 0f) * moveSpeed * Time.deltaTime);

        // 画面内に閉じ込める
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -8.33f, 8.33f);
        pos.y = Mathf.Clamp(pos.y, -4.08f, 4.08f);
        transform.position = pos;

        // 左右反転
        if (moveInput.x < 0) spriteRenderer.flipX = false;
        else if (moveInput.x > 0) spriteRenderer.flipX = true;

        // アニメ再生/停止
        animator.speed = moveInput.magnitude > 0f ? 1f : 0f;

        // 移動速度調整
        float weightedScore = CO2Score * CO2Weight + ConcreteScore * ConcreteWeight;
        float t = Mathf.Clamp01(weightedScore / MaxScore);
        moveSpeed = Mathf.Lerp(StartMoveSpeed, MinMoveSpeed, t);

        // クリスタルゾーン処理
        if (inCrystalZone)
        {
            if (zoneTimer == 0f && CO2Score > 0 && ConcreteScore > 0)
            {
                // 効果音
                if (willSpawnRare == true) rareCreateCrystal.Play();
                else createCrystal.Play();
            }

            if (CO2Score > 0 && ConcreteScore > 0)
            {
                zoneTimer += Time.deltaTime;
                UpdateCrystalAlpha();
            }

            if (zoneTimer >= CreateCrystalTime)
            {
                DoCrystalReaction();
                createCrystal.Stop();
                rareCreateCrystal.Stop();
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

        if (other.CompareTag("Magnesium"))
        {
            aura.Play();
            if(!willSpawnRare) getMagnesium.Play();
            willSpawnRare = true;
            Destroy(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("CrystalZone"))
        {
            createCrystal.Stop();
            rareCreateCrystal.Stop();
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
        if (CO2Score <= 0 || ConcreteScore <= 0) return;

        firstHaveMaterials = false;
        Color c = GoSprite.color;
        c.a = 0;
        GoSprite.color = c;

        GetCO2(-1);
        GetConcrete(-1);

        zoneTimer = 0f;
        UpdateCrystalAlpha();

        GameObject prefabToSpawn;
        SpriteRenderer spriteToUse;

        if (willSpawnRare == true && rareCrystalPrefab != null)
        {
            prefabToSpawn = rareCrystalPrefab;
            spriteToUse = rareCrystalSprite;
        }
        else
        {
            prefabToSpawn = crystalPrefab;
            spriteToUse = crystalSprite;
        }

        willSpawnRare = false;
        aura.Stop();
        aura.Clear();

        GameObject obj = Instantiate(prefabToSpawn, spriteToUse.transform.position, spriteToUse.transform.rotation);
        var script = obj.GetComponent<MoveAndShrinkBySpeed>();
        if (script != null)
        {
            if (prefabToSpawn == rareCrystalPrefab) script.crystalUI = rareCrystalUI;
            else script.crystalUI = crystalUI;
        }
    }

    private void UpdateCrystalAlpha()
    {
        float alpha = Mathf.Clamp01(zoneTimer / CreateCrystalTime);

        if (willSpawnRare == true && rareCrystalSprite != null)
        {
            rareCrystalSprite.color = new Color(
                rareCrystalSprite.color.r, rareCrystalSprite.color.g, rareCrystalSprite.color.b, alpha);
            if (crystalSprite != null) crystalSprite.color = new Color(crystalSprite.color.r, crystalSprite.color.g, crystalSprite.color.b, 0f);
        }
        else if (willSpawnRare == false && crystalSprite != null)
        {
            crystalSprite.color = new Color(
                crystalSprite.color.r, crystalSprite.color.g, crystalSprite.color.b, alpha);
            if (rareCrystalSprite != null) rareCrystalSprite.color = new Color(rareCrystalSprite.color.r, rareCrystalSprite.color.g, rareCrystalSprite.color.b, 0f);
        }
        else
        {
            // 両方透明にする
            if (crystalSprite != null) crystalSprite.color = new Color(crystalSprite.color.r, crystalSprite.color.g, crystalSprite.color.b, 0f);
            if (rareCrystalSprite != null) rareCrystalSprite.color = new Color(rareCrystalSprite.color.r, rareCrystalSprite.color.g, rareCrystalSprite.color.b, 0f);
        }
    }
}
