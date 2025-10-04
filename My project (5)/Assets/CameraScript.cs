using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    [SerializeField] GameObject mainCamera;      // メインカメラ格納用
    [SerializeField] GameObject subCamera;       // サブカメラ格納用 
    [SerializeField] float rotationSpeed = 100f; // カメラの回転速度
    private InputAction cameraSwitchAction;       // RBボタンの入力アクション
    private InputAction cameraRotateAction;       // 右スティックの入力アクション

    private float pitch = 0f;                    // カメラの上下回転（ピッチ）
    private float yaw = 0f;                      // カメラの左右回転（ヨー）

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // サブカメラを非アクティブにする
        if (subCamera != null)
        {
            subCamera.SetActive(false);
        }
        else
        {
            Debug.LogError("SubCamera is not assigned in the Inspector!");
        }

        if (mainCamera == null)
        {
            Debug.LogError("MainCamera is not assigned in the Inspector!");
        }

        // RBボタンのアクションを設定
        cameraSwitchAction = new InputAction("CameraSwitch", InputActionType.Button);
        cameraSwitchAction.AddBinding("<Gamepad>/rightShoulder");
        cameraSwitchAction.Enable();

        // 右スティックのアクションを設定
        cameraRotateAction = new InputAction("CameraRotate", InputActionType.Value);
        cameraRotateAction.AddBinding("<Gamepad>/rightStick");
        cameraRotateAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        // RBボタンでカメラ切り替え
        if (cameraSwitchAction != null && cameraSwitchAction.WasPressedThisFrame())
        {
            if (mainCamera != null && subCamera != null)
            {
                if (mainCamera.activeSelf)
                {
                    mainCamera.SetActive(false);
                    subCamera.SetActive(true);
                    Debug.Log("Switched to SubCamera");
                }
                else
                {
                    mainCamera.SetActive(true);
                    subCamera.SetActive(false);
                    Debug.Log("Switched to MainCamera");
                }
            }
        }

        // 右スティックでカメラ回転
        if (cameraRotateAction != null)
        {
            // 右スティックの入力を取得（x: 左右, y: 上下）
            Vector2 stickInput = cameraRotateAction.ReadValue<Vector2>();
            GameObject activeCamera = mainCamera.activeSelf ? mainCamera : subCamera;

            if (activeCamera != null)
            {
                // ヨー（左右回転）とピッチ（上下回転）を計算
                yaw += stickInput.x * rotationSpeed * Time.deltaTime;
                pitch -= stickInput.y * rotationSpeed * Time.deltaTime;

                // ピッチを制限（上や下を見すぎないように-80～80度に制限）
                pitch = Mathf.Clamp(pitch, -80f, 80f);

                // アクティブなカメラの回転を更新
                activeCamera.transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
            }
        }
    }

    // スクリプトが破棄される際にアクションを無効化
    void OnDestroy()
    {
        if (cameraSwitchAction != null)
        {
            cameraSwitchAction.Disable();
        }
        if (cameraRotateAction != null)
        {
            cameraRotateAction.Disable();
        }
    }
}