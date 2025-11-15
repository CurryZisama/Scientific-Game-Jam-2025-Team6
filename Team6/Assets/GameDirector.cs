using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    public int GameOverCount = 20;
    [SerializeField] private Image meterImage;
    [SerializeField] float TimeLimit = 60f;
    private float GameOverTime;

    [SerializeField] private Image clockImage; // 円形ゲージ用 UI Image
    [SerializeField] private Text clockText;

    public static bool isClear = false;

    private void Start()
    {
        GameOverTime = TimeLimit;
    }

    void Update()
    {
        if (meterImage != null)
        {
            float fill = Mathf.Clamp01((float)CO2Count.InstanceCount / GameOverCount);
            meterImage.fillAmount = fill;
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

}
