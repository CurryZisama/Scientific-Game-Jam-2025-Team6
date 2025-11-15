using UnityEngine;
using UnityEngine.UI;

public class ResultDisplay : MonoBehaviour
{
    [Header("UIの割り当て")]
    public Text resultTitleText;   // (例) "CLEAR!" や "FAILED..."
    public Text crystalText;       // 通常クリスタルの数を表示するText
    public Text rareCrystalText;   // レア鉱石の数を表示するText
    public Text totalScoreText;    // トータルスコアを表示するText

    [Header("スコア計算設定")]
    public int crystalScoreValue = 100;       // 通常クリスタルのスコア倍率
    public int rareCrystalScoreValue = 500;     // レア鉱石のスコア倍率

    void Start()
    {
        // PlayerController の static 変数からデータを読み込む
        int crystalCount = PlayerController.CrystalScore;
        int rareCrystalCount = PlayerController.RareCrystalScore;
        bool cleared = GameDirector.isClear;

        // 1. クリア/失敗の表示 (これはそのまま)
        if (cleared) 
        {
            resultTitleText.text = "世界は豊かになった！";
            resultTitleText.color = Color.green; // (例)
        }
        else 
        {
            resultTitleText.text = "世界は炎に包まれた";
            resultTitleText.color = Color.gray; // (例)
        }

        // 2. 取得した「数」をテキストに反映
        crystalText.text = crystalCount.ToString();
        rareCrystalText.text = rareCrystalCount.ToString();

        // 3. トータルスコアを計算
        // (スコアが大きくなる可能性を考慮し、long型で計算します)
        long totalScore = ((long)crystalCount * crystalScoreValue) + 
                          ((long)rareCrystalCount * rareCrystalScoreValue);

        // 4. トータルスコアをテキストに反映
        totalScoreText.text = totalScore.ToString();
        
        // 5. (重要) 次のゲームのために static 変数をリセット
        ResetGameData();
    }

    // ゲームデータをリセットする関数
    private void ResetGameData()
    {
        PlayerController.CO2Score = 0;
        PlayerController.ConcreteScore = 0; // コンクリートも念のためリセット
        PlayerController.CrystalScore = 0;
        PlayerController.RareCrystalScore = 0;
        GameDirector.isClear = false;
    }
}