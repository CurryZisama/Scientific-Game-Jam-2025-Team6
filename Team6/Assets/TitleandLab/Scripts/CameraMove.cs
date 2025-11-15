using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMove : MonoBehaviour
{
    // === 公開変数 (インスペクターで設定) ===
    [Header("追従設定")]
    // カメラの移動速度（大きいほど早く追従）
    public float followSpeed = 5.0f;

    // カメラが初期位置から動ける最大距離（例: 2.0f = X/Y方向に最大2メートル動ける）
    public float maxMoveDistance = 2.0f;

    // マウスの動きに対するカメラの反応度 (大きいほどマウスの僅かな動きで大きくカメラが動く)
    public float sensitivity = 0.1f;

    // === プライベート変数 ===
    // カメラの初期位置を保存
    private Vector3 initialPosition;

    void Start()
    {
        // ゲーム開始時のカメラの位置を初期位置として保存します
        initialPosition = transform.position;
    }

    void Update()
    {
        // 1. マウスの入力処理とオフセットの計算
        // 「新しい入力システム」でマウスのスクリーン座標 (Vector2) を取得
        Vector2 mousePos = Mouse.current.position.ReadValue();

        // (0, 0) が画面の中心になるように正規化 (例: -0.5〜0.5)
        float mouseX = (mousePos.x / Screen.width) - 0.5f;
        float mouseY = (mousePos.y / Screen.height) - 0.5f;

        // sensitivityを適用して、マウスの動きに対する移動目標のオフセットを決定
        Vector3 targetOffset = new Vector3(
            mouseX * sensitivity,
            mouseY * sensitivity,
            0 // カメラはZ軸方向（奥行き）には動かしません
        );

        // 2. 最終的な目標位置の計算
        // 初期位置に計算したオフセットを加えたものが、理想的な目標位置
        Vector3 desiredTargetPosition = initialPosition + targetOffset;

        // 3. 最大移動距離による制限（クランプ）
        // 初期位置から目標位置へのベクトルを計算
        Vector3 displacementFromInitial = desiredTargetPosition - initialPosition;

        // ベクトルの長さを最大移動距離 (maxMoveDistance) で制限します
        // これにより、カメラが initialPosition から maxMoveDistance の球体範囲外に出るのを防ぎます
        displacementFromInitial = Vector3.ClampMagnitude(displacementFromInitial, maxMoveDistance);

        // 制限されたベクトルを初期位置に加え、最終的な目標位置を決定
        Vector3 finalTargetPosition = initialPosition + displacementFromInitial;

        // 4. 現在の位置から目標位置へ滑らかに移動
        // Lerp（線形補間）を使って、現在の位置から目標位置へ滑らかに移動させます
        // followSpeed が大きいほど早く追従し、滑らかさが減ります。
        transform.position = Vector3.Lerp(
            transform.position,
            finalTargetPosition,
            Time.deltaTime * followSpeed
        );
    }
}
