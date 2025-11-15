using UnityEngine;
using UnityEngine.EventSystems; // (IPointerEnterHandler に必要)
using UnityEngine.UI;           // (Button コンポーネントに必要)

// [RequireComponent(typeof(Button))] // この行は、Buttonが無いとエラーを出すおまじない
public class ButtonSound : MonoBehaviour, IPointerEnterHandler
{
    // === インスペクターで設定 ===
    
    [Header("再生する音（AudioClip）")]
    public AudioClip hoverSound; // カーソルが乗った時の音
    public AudioClip clickSound; // クリックした時の音

    [Header("音を再生するスピーカー（AudioSource）")]
    // シーン内のどこかにあるAudioSourceをアタッチ
    public AudioSource audioSource;

    // === プライベート変数 ===
    private Button button;

    void Start()
    {
        // 1. このオブジェクトのButtonコンポーネントを取得
        button = GetComponent<Button>();

        // 2. ButtonのOnClickイベントに、自前のPlayClickSound関数を「追加」する
        if (button != null && clickSound != null && audioSource != null)
        {
            button.onClick.AddListener(PlayClickSound);
        }
    }

    // 3. IPointerEnterHandler のために必要な関数（カーソルが乗った時に自動で呼ばれる）
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null && audioSource != null)
        {
            // PlayOneShot: SEの再生に最適。音が重なっても再生できる
            audioSource.PlayOneShot(hoverSound);
        }
    }

    // 4. OnClickイベントに登録するための関数
    private void PlayClickSound()
    {
        // OnPointerEnter とは別に、クリック音を再生
        audioSource.PlayOneShot(clickSound);
    }
}
