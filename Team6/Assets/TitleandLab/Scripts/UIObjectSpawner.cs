using UnityEngine;

public class UIObjectSpawner : MonoBehaviour
{
    [Header("生成するプレハブ")]
    // インスペクターで設定するプレハブの配列（2つ）
    public GameObject[] prefabOptions;

    [Header("生成エリア（UIパネル）")]
    // プレハブを生成し、移動させる範囲となるUIパネル
    public RectTransform spawnArea;

    [Header("生成タイミング")]
    public float minSpawnDelay = 1.0f; // 最短の生成間隔（秒）
    public float maxSpawnDelay = 3.0f; // 最長の生成間隔（秒）

    // 次の生成までのタイマー
    private float spawnTimer;

    void Start()
    {
        // 最初のタイマーをセット
        ResetSpawnTimer();
    }

    void Update()
    {
        // タイマーを減らす
        spawnTimer -= Time.deltaTime;

        // タイマーが0以下になったら
        if (spawnTimer <= 0)
        {
            SpawnObject(); // 生成を実行
            ResetSpawnTimer(); // 次のタイマーをセット
        }
    }

    // 次の生成タイマーをランダムに設定する
    void ResetSpawnTimer()
    {
        spawnTimer = Random.Range(minSpawnDelay, maxSpawnDelay);
    }

    // オブジェクトを生成する
    void SpawnObject()
    {
        // 1. 2つのプレハブからランダムに1つを選ぶ
        int index = Random.Range(0, prefabOptions.Length);
        GameObject prefabToSpawn = prefabOptions[index];

        // 2. 生成する
        // Instantiateの第2引数に `spawnArea` を指定し、パネルの子として生成する
        GameObject newInstance = Instantiate(prefabToSpawn, spawnArea);

        // 3. 生成位置を決める（パネルの右端、高さはランダム）
        Rect spawnRect = spawnArea.rect;
        
        // パネルの右端のX座標
        float spawnX = spawnRect.width / 2f;
        
        // パネルの高さの範囲内でランダムなY座標
        float spawnY = Random.Range(-spawnRect.height / 2f, spawnRect.height / 2f);

        // 4. 生成したオブジェクトの位置を設定
        // RectTransformコンポーネントを取得して、anchoredPositionを設定
        newInstance.GetComponent<RectTransform>().anchoredPosition = new Vector2(spawnX, spawnY);
    }
}
