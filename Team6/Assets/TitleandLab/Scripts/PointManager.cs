using UnityEngine;
using UnityEngine.SceneManagement; // シーン管理に必要
public class PointManager : MonoBehaviour
{
// 1. シングルトン（唯一の存在）にするための仕組み
    public static PointManager Instance { get; private set; }

    // 2. ResultSceneに引き継ぎたいデータ
    public int concreteCount = 0;
    public int crystalCount = 0;
    public bool isClear = false; // クリアしたかどうか (True/False)

    // AwakeはStartよりも早く呼ばれる
    void Awake()
    {
        // 3. シングルトンの実装
        if (Instance == null)
        {
            // Instanceが空なら、自分自身を代入
            Instance = this;
            
            // シーンをまたいでも、このオブジェクトを破棄しないようにする
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // 既にInstanceが存在する場合は、重複させないため自分自身を破棄
            Destroy(this.gameObject);
        }
    }

    // 4. (おまけ) タイトルに戻る時などにデータをリセットする関数
    public void ResetData()
    {
        concreteCount = 0;
        crystalCount = 0;
        isClear = false;
    }
}
