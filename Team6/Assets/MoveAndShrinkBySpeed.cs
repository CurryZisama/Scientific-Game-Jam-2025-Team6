using UnityEngine;
using UnityEngine.UI;

public class MoveAndShrinkBySpeed : MonoBehaviour
{
    private Vector3 targetPosition = new Vector3(-7.75f, 2.66f, 0f);
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float shrinkDuration = 1.5f; 
    public Text crystalUI;

    // 1. この public 変数を追加します (PlayerControllerが設定します)
    public bool isRare = false; 

    private Vector3 startScale;
    private float shrinkTimer = 0f;

    void Start()
    {
        startScale = transform.localScale;
    }

    void Update()
    {
        // ... (移動と縮小の処理はそのまま) ...
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        shrinkTimer += Time.deltaTime;
        float t = Mathf.Clamp01(shrinkTimer / shrinkDuration);   
        transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);

        if (transform.position == targetPosition || t >= 1f)
        {
            AddCrystal();
            Destroy(gameObject);
        }
    }

    private void AddCrystal()
    {
        // 2. タグ比較の代わりに、isRare 変数(フラグ)をチェックします
        if (isRare) // "isRare == true" と同じ意味
        {
            // 3. レア鉱石の static スコアを加算
            PlayerController.RareCrystalScore++; 
            crystalUI.text = PlayerController.RareCrystalScore.ToString();
        }
        else 
        {
            // 3. 通常クリスタルの static スコアを加算
            PlayerController.CrystalScore++; 
            crystalUI.text = PlayerController.CrystalScore.ToString();
        }
    }
}