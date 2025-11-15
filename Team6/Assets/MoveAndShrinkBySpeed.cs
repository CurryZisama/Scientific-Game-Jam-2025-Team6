using UnityEngine;
using UnityEngine.UI;

public class MoveAndShrinkBySpeed : MonoBehaviour
{
    private Vector3 targetPosition = new Vector3(-7.75f, 2.66f, 0f);
    [SerializeField] private float moveSpeed = 20f;

    [SerializeField] private float shrinkDuration = 1.5f; // ← ここがポイント：何秒で消えるか
    public Text crystalUI;

    private Vector3 startScale;
    private float shrinkTimer = 0f;

    void Start()
    {
        startScale = transform.localScale;
    }

    void Update()
    {
        // 移動（一定速度）
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // 時間で縮小
        shrinkTimer += Time.deltaTime;
        float t = Mathf.Clamp01(shrinkTimer / shrinkDuration);   // 0 → 1
        transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);

        // 到達 or 縮小完了で削除
        if (transform.position == targetPosition || t >= 1f)
        {
            AddCrystal();
            Destroy(gameObject);
        }
    }

    private void AddCrystal()
    {
        int current = 0;

        if (!int.TryParse(crystalUI.text, out current))
        {
            current = 0;
        }

        current += 1;

        if (this.gameObject.CompareTag("rare"))
        {
            PlayerController.CrystalScore = current;
        }
        else { PlayerController.RareCrystalScore = current; }

        crystalUI.text = current.ToString();
    }
}
