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
    
    [Header("スコア表示用")]
    public Text newHighScoreText;
    public Text highScore1stText; 
    public Text highScore2ndText;
    public Text highScore3rdText;

    public AudioSource SEAudio;
    public AudioClip[] audioClips;

    void Start()
    {
        if (newHighScoreText != null)
        {
            newHighScoreText.gameObject.SetActive(false);
        }
        // ステップ1, 2 で蓄積された static 変数を読み込む
        int crystalCount = PlayerController.CrystalScore;
        int rareCrystalCount = PlayerController.RareCrystalScore;
        bool cleared = GameDirector.isClear;

        // 2. 取得した「数」をそのまま表示
        crystalText.text = crystalCount.ToString() + "×100";
        rareCrystalText.text = rareCrystalCount.ToString() + "×500";
        
        long totalScore = 0;
        
        // 1. クリア/失敗の表示
        if (cleared) 
        {
            // resultTitleText.text = "世界は豊かになった！";
            // resultTitleText.color = Color.yellow; 
            CLEAR.SetActive(!CLEAR.activeSelf);
            SEAudio.PlayOneShot(audioClips[0]);
            // 3. ★ ご要望の「トータルスコア」を計算
            // ( crystalCount × 100 ) + ( rareCrystalCount × 500 )

            totalScore = ((long)crystalCount * crystalScoreValue) +
                              ((long)rareCrystalCount * rareCrystalScoreValue) + 1000;
            // 4. トータルスコアを表示
            totalScoreText.text = totalScore.ToString();
        }
        else 
        {
            // resultTitleText.text = "FAILED...";
            // resultTitleText.color = Color.gray;
            FAIL.SetActive(!FAIL.activeSelf);
            SEAudio.PlayOneShot(audioClips[1]);
            // 3. ★ ご要望の「トータルスコア」を計算
            // ( crystalCount × 100 ) + ( rareCrystalCount × 500 )
        
            totalScore = ((long)crystalCount * crystalScoreValue) + 
                              ((long)rareCrystalCount * rareCrystalScoreValue);
            // 4. トータルスコアを表示
            totalScoreText.text = totalScore.ToString();
        }

        CheckAndSaveHighScore(totalScore);
        DisplayHighScores();
        ResetGameData();
    }
    private void CheckAndSaveHighScore(long newScore)
    {
        // PlayerPrefsから過去のスコアを読み込む (文字列として保存)
        // "0" は、もしデータが無かった場合の初期値
        long score1 = long.Parse(PlayerPrefs.GetString("HighScore1", "0"));
        long score2 = long.Parse(PlayerPrefs.GetString("HighScore2", "0"));
        long score3 = long.Parse(PlayerPrefs.GetString("HighScore3", "0"));

        bool isNewHighScore = false;

        // 1位～3位にランクインしたかチェック
        if (newScore > score1)
        {
            // 新1位！ (3位を消し、2位を3位に、1位を2位に、新しいスコアを1位に)
            score3 = score2;
            score2 = score1;
            score1 = newScore;
            isNewHighScore = true;
        }
        else if (newScore > score2)
        {
            // 新2位！
            score3 = score2;
            score2 = newScore;
            isNewHighScore = true;
        }
        else if (newScore > score3)
        {
            // 新3位！
            score3 = newScore;
            isNewHighScore = true;
        }

        // もしハイスコアを更新していたら
        if (isNewHighScore)
        {
            // 1. テキストを表示する
            if (newHighScoreText != null)
            {
                newHighScoreText.gameObject.SetActive(true);
            }

            // 2. 新しいスコアを PlayerPrefs に文字列として保存
            PlayerPrefs.SetString("HighScore1", score1.ToString());
            PlayerPrefs.SetString("HighScore2", score2.ToString());
            PlayerPrefs.SetString("HighScore3", score3.ToString());
            
            // 3. データを本体に書き込む (重要)
            PlayerPrefs.Save();
        }
    }
    private void DisplayHighScores()
    {
        // PlayerPrefsから保存されたスコアを読み込む
        // (CheckAndSaveHighScoreの後なので、最新のランキングが読み込まれます)
        long score1 = long.Parse(PlayerPrefs.GetString("HighScore1", "0"));
        long score2 = long.Parse(PlayerPrefs.GetString("HighScore2", "0"));
        long score3 = long.Parse(PlayerPrefs.GetString("HighScore3", "0"));

        // 各Textコンポーネントにスコアを表示
        if (highScore1stText != null)
        {
            highScore1stText.text = score1.ToString();
        }
        
        if (highScore2ndText != null)
        {
            highScore2ndText.text = score2.ToString();
        }
        
        if (highScore3rdText != null)
        {
            highScore3rdText.text = score3.ToString();
        }
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