using UnityEngine;
using TMPro; // TextMeshPro を使う場合

public class ResultDisplay : MonoBehaviour
{
    [Header("UIの割り当て")]
    public TextMeshProUGUI resultTitleText; 
    public TextMeshProUGUI concreteText;
    public TextMeshProUGUI crystalText; // クリスタルの合計用

    void Start()
    {
        // GameManagerは不要！ PlayerController から直接データを読み込む
        int concrete = PlayerController.ConcreteScore;
        
        
        // 2種類のクリスタルスコアを合計する
        int totalCrystals = PlayerController.CrystalScore + PlayerController.RareCrystalScore;
        
        // bool cleared = GameDirector.iscleared;

        // 1. クリア/失敗の表示
        // if (cleared) 
        // {
        //     resultTitleText.text = "CLEAR!";
        // }
        // else 
        // {
        //     resultTitleText.text = "FAILED...";
        // }

        // 2. 取得した数をテキストに反映
        concreteText.text = "コンクリート: " + concrete.ToString();
        crystalText.text = "クリスタル: " + totalCrystals.ToString();

        // (おまけ) 次のゲームのためにデータをリセットする場合
        // PlayerController.ConcreteScore = 0;
        // PlayerController.CrystalScore = 0;
        // PlayerController.RareCrystalScore = 0;
        // PlayerController.isClear = false;
    }
}
