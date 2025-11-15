using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    public int GameOverCount = 20;
    public float GameOverTime = 60f;

    [SerializeField] private Image clockImage; // 円形ゲージ用 UI Image

    void Update()
    {
        // 1. スコア上限でゲームオーバー
        if (CO2Count.InstanceCount >= GameOverCount)
        {
            SceneManager.LoadScene("EndScene");
            return;
        }

        // 2. 時間でゲームオーバー
        if (GameOverTime > 0f)
        {
            GameOverTime -= Time.deltaTime;

            // 時計 UI を更新
            if (clockImage != null)
            {
                float fill = Mathf.Clamp01(GameOverTime / 60f); // 60秒を満タンとする場合
                clockImage.fillAmount = fill;
            }

            // タイムアップでシーン遷移
            if (GameOverTime <= 0f)
            {
                SceneManager.LoadScene("EndScene");
            }
        }
    }
}
