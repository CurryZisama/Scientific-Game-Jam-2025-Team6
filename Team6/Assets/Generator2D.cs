using UnityEngine;

public class Generator2D : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Vector2 xRange = new Vector2(-8.5f, 8.5f);
    [SerializeField] private Vector2 yRange = new Vector2(-3.5f, 3.5f);

    [Header("Spawn Settings")]
    [SerializeField] private float baseSpawnInterval = 10f; // 初期の間隔
    [SerializeField] private int baseSpawnCount = 1;        // 初期の出現数

    [Header("Difficulty Settings")]
    [SerializeField] private float difficultyIncreaseRate = 0.01f; // 時間経過で増えるむずかしさ
    [SerializeField] private float maxDifficulty = 1f;              // 最大難易度
    [SerializeField] private float frequencyMultiplier = 0.5f;     // 難易度が最大の時、spawnInterval をどれだけ短くするか
    [SerializeField] private int maxExtraCount = 3;                // 難易度最大で一度に増える最大数

    private float timer = 0f;
    private float difficulty = 0f; // 0～maxDifficulty

    void Start()
    {
        SpawnRandom(baseSpawnCount);
        timer = baseSpawnInterval;
    }

    void Update()
    {
        // 難易度を少しずつ増やす
        difficulty += difficultyIncreaseRate * Time.deltaTime;
        difficulty = Mathf.Clamp(difficulty, 0f, maxDifficulty);

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            // 難易度に応じて増える数を計算
            int extraCount = Mathf.FloorToInt(maxExtraCount * difficulty / maxDifficulty);
            int spawnCount = baseSpawnCount + extraCount;

            SpawnRandom(spawnCount);

            // 難易度に応じて間隔を短くする
            float interval = baseSpawnInterval * (1f - difficulty / maxDifficulty * frequencyMultiplier);
            timer = interval + Random.Range(-0.5f, 0.5f);
        }
    }

    public void SpawnRandom(int count)
    {
        for (int i = 0; i < count; i++)
        {
            float randomX = Random.Range(xRange.x, xRange.y);
            float randomY = Random.Range(yRange.x, yRange.y);

            Vector2 spawnPos = new Vector2(randomX, randomY);
            Instantiate(prefab, spawnPos, Quaternion.identity);
        }
    }
}
