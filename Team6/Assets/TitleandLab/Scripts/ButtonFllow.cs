using UnityEngine;
using UnityEngine.EventSystems; // UIイベントインターフェースのために必要

// 必要なインターフェースを継承します
public class ButtonFllow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // === 公開変数 (インスペクターで設定) ===
    public Animator targetAnimator;

    public GameObject targetObject;
    
    public string FADE = "FADE";
    
    [Header("拡大・縮小設定")]
    // 拡大時のスケール（例: 1.2倍）
    public Vector3 enlargedScale = new Vector3(1.2f, 1.2f, 1.2f);
    
    // スケール変更の速さ（滑らかさ調整用）
    public float scaleSpeed = 10f; 

    // === プライベート変数 ===

    // ボタンの元のサイズを保存
    private Vector3 originalScale;

    // 現在の目標スケール（拡大中か、元に戻しているか）
    private Vector3 targetScale;

    void Start()
    {
        // 初期スケールを保存し、目標スケールも初期スケールに設定
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void Update()
    {
        // Lerp（線形補間）を使って、現在のスケールを目標スケールへ滑らかに変化させます
        if (transform.localScale != targetScale)
        {
            transform.localScale = Vector3.Lerp(
                transform.localScale,
                targetScale,
                Time.deltaTime * scaleSpeed
            );
        }
        
        // --- 既存のコード（今回は使わないが残す場合） ---
        // Animator や targetObject 関連のコードは、今回のタスクには直接関わらないため、
        // 必要に応じて残すか削除してください。
    }

    // IPointerEnterHandler インターフェースの実装
    // カーソルがボタンに触れたときに呼ばれる
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 目標スケールを拡大サイズに設定
        targetScale = enlargedScale;
    }

    // IPointerExitHandler インターフェースの実装
    // カーソルがボタンから離れたときに呼ばれる
    public void OnPointerExit(PointerEventData eventData)
    {
        // 目標スケールを元のサイズに設定
        targetScale = originalScale;
    }
    
    public void OnButtonClickPlayAnimation()

    {

// Animatorのトリガーを設定し、アニメーションを再生します

// targetObject.SetActive(true);

        targetAnimator.SetTrigger("FADE");


    }



    public void ClickEvent()

    {

        targetObject.SetActive(!targetObject.activeSelf);

    }
}