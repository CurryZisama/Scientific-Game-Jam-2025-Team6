using UnityEngine;
using UnityEngine.SceneManagement; // シーン管理に必要

public class GameDirector : MonoBehaviour
{
    [SerializeField] private float time = 10f; // カウントダウン時間

    void Update()
    {
        time -= Time.deltaTime; // フレームごとに減らす

        if (time <= 0f)
        {
            //SceneManager.LoadScene("NextSceneName"); // 移動したいシーン名に変更
            Debug.Log("ゲーム終了、次のSceneへ");
        }
    }
}
