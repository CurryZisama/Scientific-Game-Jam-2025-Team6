using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    InputSystem_Actions input;
    Vector2 moveInput;

    [SerializeField] private Text CO2ScoreUI; // Legacy UI の Text コンポーネント
    [SerializeField] private Text ConcreteScoreUI;

    public static int CO2Score;
    public static int ConcreteScore;

    // 現在重なっているものを保持する
    List<GameObject> overlapping = new List<GameObject>();

    void Awake()
    {
        input = new InputSystem_Actions();
        input.Player.Click.performed += _ => OnClick();
    }

    void OnEnable()
    {
        input.Player.Enable();
    }

    void OnDisable()
    {
        input.Player.Disable();
    }

    void Update()
    {
        moveInput = input.Player.Move.ReadValue<Vector2>();
        Vector3 move = new Vector3(moveInput.x, moveInput.y, 0 );
        transform.Translate(move * moveSpeed * Time.deltaTime);
    }

    // --- 重なり始めたら登録 ---
    void OnTriggerEnter(Collider other)
    {
        if (!overlapping.Contains(other.gameObject))
            overlapping.Add(other.gameObject);
    }

    // --- 離れたら解除 ---
    void OnTriggerExit(Collider other)
    {
        overlapping.Remove(other.gameObject);
    }

    // --- クリックされたときに重なっているものを利用 ---
    void OnClick()
    {
        // null のオブジェクトをリストから削除
        overlapping.RemoveAll(obj => obj == null);

        // コピーしてループ
        foreach (var obj in new List<GameObject>(overlapping))
        {
            if (obj.CompareTag("CO2"))
            {
                GetCO2();
                overlapping.Remove(obj); // リストから削除
                Destroy(obj);            // シーンから消す
            }
            else if (obj.CompareTag("Concrete"))
            {
                GetConcrete();
                overlapping.Remove(obj);
                Destroy(obj);
            }
        }
    }


    void GetCO2()
    {
        CO2Score++;
        CO2ScoreUI.text = "CO2Score: " + CO2Score;
    }

    void GetConcrete()
    {
        ConcreteScore++;
        ConcreteScoreUI.text = "ConcreteScore: " + ConcreteScore;
    }
}
