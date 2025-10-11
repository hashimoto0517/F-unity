using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    [SerializeField] GameObject mainCamera;      // メインカメラ格納用
    [SerializeField] GameObject subCamera;       // サブカメラ格納用 
    [SerializeField] float rotationSpeed = 100f; // カメラの回転速度
    [SerializeField] GameObject player;//プレイヤー格納
    [SerializeField] float distance = 5f;//プレイヤーとの距離
    [SerializeField] float height = 2f;//カメラ高さ
    private InputAction cameraSwitchAction;       // RBボタンの入力
    private InputAction cameraRotateAction;       // 右スティックの入力

    private float pitch = 0f;                    // 上下回転
    private float yaw = 0f;                      // 左右回転

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

        // RBボタン
        cameraSwitchAction = new InputAction("CameraSwitch", InputActionType.Button);
        cameraSwitchAction.AddBinding("<Gamepad>/rightShoulder");
        cameraSwitchAction.Enable();

        // 右スティック
        cameraRotateAction = new InputAction("CameraRotate", InputActionType.Value);
        cameraRotateAction.AddBinding("<Gamepad>/rightStick");
        cameraRotateAction.Enable();

        //初期位置設定
        UpdateCameraPosition(mainCamera);
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
                    UpdateCameraPosition(subCamera);
                    Debug.Log("Switched to SubCamera");
                }
                else
                {
                    mainCamera.SetActive(true);
                    subCamera.SetActive(false);
                    UpdateCameraPosition(mainCamera);
                    Debug.Log("Switched to MainCamera");
                }
            }
        }

        // 右スティックでカメラ回転
        if (cameraRotateAction != null)
        {
            // 右スティックの入力を取得
            Vector2 stickInput = cameraRotateAction.ReadValue<Vector2>();
            GameObject activeCamera = mainCamera.activeSelf ? mainCamera : subCamera;

            if (activeCamera != null)
            {
                //左右回転と上下回転を計算
                yaw += stickInput.x * rotationSpeed * Time.deltaTime;
                pitch -= stickInput.y * rotationSpeed * Time.deltaTime;

                // 回転を制限
                pitch = Mathf.Clamp(pitch, -80f, 80f);

                // カメラの回転を更新
                UpdateCameraPosition(activeCamera);
            }
        }
    }

    private void UpdateCameraPosition(GameObject camera)
    {
        if (player == null || camera == null) return;
        //プレイヤーからカメラ位置の設定
        Vector3 playerPos = player.transform.position;
        float x = playerPos.x + Mathf.Sin(yaw * Mathf.Deg2Rad) * distance;
        float z = playerPos.z + Mathf.Cos(yaw * Mathf.Deg2Rad) * distance;
        float y = playerPos.y + height + Mathf.Sin(pitch * Mathf.Deg2Rad) * distance;
        // カメラの位置設定
        camera.transform.position = new Vector3(x, y, z);

        // カメラをプレイヤーの方向に向ける
        camera.transform.LookAt(playerPos);
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