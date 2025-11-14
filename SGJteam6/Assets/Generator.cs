using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] private GameObject prefab;  // InspectorでPrefabをセット
    [SerializeField] private Vector2 xRange = new Vector2(-5f, 5f); // X範囲
    [SerializeField] private Vector2 yRange = new Vector2(-5f, 5f); // Y範囲

    [SerializeField] private float spawnInterval = 10f; // 平均生成間隔
    private float timer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnRandom(5);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            int count = Random.Range(1, 4); // 1～3個生成
            SpawnRandom(count);

            // 次の生成までの時間をランダムに少し変える
            timer = spawnInterval + Random.Range(-0.5f, 0.5f);
        }
    }

    public void SpawnRandom(int count)
    {
        for (int i = 0; i < count; i++)
        {
            float randomX = Random.Range(xRange.x, xRange.y);
            float randomY = Random.Range(yRange.x, yRange.y);

            Vector3 spawnPos = new Vector3(randomX, randomY, -2f); // Z=0
            Instantiate(prefab, spawnPos, Quaternion.identity);
        }
    }
}
