using UnityEngine;
using UnityEngine.UI; // UI.Text を使う

public class ResultDisplay : MonoBehaviour
{
    [Header("UIの割り当て (UI Text)")]
    public GameObject CLEAR;
    public GameObject FAIL;
    public Text crystalText;       // 通常クリスタルの「数」
    public Text rareCrystalText;   // レア鉱石の「数」
    public Text totalScoreText;    // 「トータルスコア」

    [Header("スコア計算設定")]
    public int crystalScoreValue = 100;     // 通常クリスタルの倍率
    public int rareCrystalScoreValue = 500;   // レア鉱石の倍率

    void Start()
    {
        // ステップ1, 2 で蓄積された static 変数を読み込む
        int crystalCount = PlayerController.CrystalScore;
        int rareCrystalCount = PlayerController.RareCrystalScore;
        bool cleared = GameDirector.isClear;

        // 1. クリア/失敗の表示
        if (cleared) 
        {
            // resultTitleText.text = "世界は豊かになった！";
            // resultTitleText.color = Color.yellow; 
            CLEAR.SetActive(!CLEAR.activeSelf);
        }
        else 
        {
            // resultTitleText.text = "FAILED...";
            // resultTitleText.color = Color.gray;
            FAIL.SetActive(!FAIL.activeSelf);
        }

        // 2. 取得した「数」をそのまま表示
        crystalText.text = crystalCount.ToString();
        rareCrystalText.text = rareCrystalCount.ToString();

        // 3. ★ ご要望の「トータルスコア」を計算
        // ( crystalCount × 100 ) + ( rareCrystalCount × 500 )
        long totalScore = ((long)crystalCount * crystalScoreValue) + 
                          ((long)rareCrystalCount * rareCrystalScoreValue);

        // 4. トータルスコアを表示
        totalScoreText.text = totalScore.ToString();

        // 5. 次のゲームのために static 変数をリセット
        ResetGameData();
    }

    private void ResetGameData()
    {
        PlayerController.CO2Score = 0;
        PlayerController.ConcreteScore = 0; 
        PlayerController.CrystalScore = 0;
        PlayerController.RareCrystalScore = 0;
        GameDirector.isClear = false;
    }
}