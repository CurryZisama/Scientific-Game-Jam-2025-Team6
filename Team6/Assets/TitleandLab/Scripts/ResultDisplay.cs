using UnityEngine;
using TMPro; // TextMeshPro を使う場合は必須

public class ResultDisplay : MonoBehaviour
{
    [Header("UIの割り当て")]
    public TextMeshProUGUI resultTitleText; // (例) "CLEAR!" や "FAILED..."
    public TextMeshProUGUI concreteText;
    public TextMeshProUGUI crystalText;

    void Start()
    {
        // GameManager からデータを読み込む
        int concrete = PointManager.Instance.concreteCount;
        int crystal = PointManager.Instance.crystalCount;
        bool cleared = PointManager.Instance.isClear;

        // 1. クリア/失敗の表示を切り替える
        if (cleared == true) // もしクリアしていたら
        {
            resultTitleText.text = "CLEAR!";
            resultTitleText.color = Color.yellow; // (例) 色を変える
        }
        else // もしクリア失敗（isClear が false）なら
        {
            resultTitleText.text = "FAILED...";
            resultTitleText.color = Color.gray;
        }

        // 2. 取得した数をテキストに反映する
        // ( .ToString() で数値を文字列に変換します )
        concreteText.text = "コンクリート: " + concrete.ToString();
        crystalText.text = "クリスタル: " + crystal.ToString();
    }
}
