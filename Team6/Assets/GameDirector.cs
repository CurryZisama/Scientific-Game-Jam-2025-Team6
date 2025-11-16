using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    public int GameOverCount = 20;
    [SerializeField] private Image meterImage;
    [SerializeField] float TimeLimit = 60f;
    [SerializeField] private Image damage;

    [SerializeField] private float shakeAmountLow = 5f;   // 7割超え
    [SerializeField] private float shakeAmountHigh = 12f; // 9割超え
    private float GameOverTime;

    private float ShakeSpeed;

    [SerializeField] private Image clockImage; // 円形ゲージ用 UI Image
    [SerializeField] private Text clockText;

    public static bool isClear = false;
    private Vector3 defaultPos;

    float fill = 0;

    private void Start()
    {
        GameOverTime = TimeLimit;

        if (meterImage != null)
            defaultPos = meterImage.rectTransform.localPosition;

        ShakeSpeed = 30f;
    }

    void Update()
    {
        if (meterImage != null)
        {
            fill =  Mathf.Clamp01((float)CO2Count.InstanceCount / GameOverCount);
            meterImage.fillAmount = fill;
        }

        // --- 揺れ判断 ---
        if (fill >= 0.9f)
        {
            Shake(shakeAmountHigh);
            AlphaBlink(0.1f, 4f);
        }
        else if (fill >= 0.7f)
        {
            Shake(shakeAmountLow);
            AlphaBlink(0.1f, 2f);
        }
        else
        {
            meterImage.rectTransform.localPosition = defaultPos;
        }

        // 上限到達でゲームオーバー
        if (CO2Count.InstanceCount >= GameOverCount)
        {
            SceneManager.LoadScene("ResultScene");
            isClear = false ;
            return;
        }

        // 2. 時間でゲームオーバー
        if (GameOverTime > 0f)
        {
            GameOverTime -= Time.deltaTime;

            // 時計 UI を更新
            if (clockImage != null)
            {
                float fill = Mathf.Clamp01(GameOverTime / TimeLimit);
                clockImage.fillAmount = fill;
            }

            // テキスト UI も更新（秒表示）
            if (clockText != null)
            {
                int seconds = Mathf.CeilToInt(GameOverTime);
                clockText.text = seconds.ToString();
            }

            // タイムアップでシーン遷移
            if (GameOverTime <= 0f)
            {
                isClear = true;
                SceneManager.LoadScene("ResultScene");
            }
        }
    }


    void Shake(float amount)
    {
        // ランダム位置へ揺らす
        float x = Mathf.Sin(Time.time * ShakeSpeed) * amount;
        float y = Mathf.Cos(Time.time * ShakeSpeed * 1.3f) * amount;
        meterImage.rectTransform.localPosition = defaultPos + new Vector3(x, y, 0);
    }

    void AlphaBlink(float minA = 0f, float maxA = 0.6f, float speed = 4f)
    {
        if (damage == null) return;

        // サイン波で 0〜1 を往復
        float t = (Mathf.Sin(Time.time * speed) + 1f) * 0.5f;

        // t を α に変換
        float alpha = Mathf.Lerp(minA, maxA, t);

        Color c = damage.color;
        c.a = alpha;
        damage.color = c;
    }
}
