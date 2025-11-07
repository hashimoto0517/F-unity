using UnityEngine;
using UnityEngine.InputSystem;

public class cameraController : MonoBehaviour
{
    [SerializeField] GameObject mainCamera;      // メインカメラ格納用
    [SerializeField] GameObject subCamera;       // サブカメラ格納用
    [SerializeField] float rotationSpeed = 100f; // カメラの回転速度
    [SerializeField] GameObject player;          // プレイヤー格納
    [SerializeField] float distance = 5f;        // プレイヤーとの距離
    [SerializeField] float height = 2f;          // カメラ高さ
    private InputAction cameraSwitchAction;      // RBボタンの入力
    private InputAction cameraRotateAction;      // 右スティックの入力

    private float mainCameraPitch = 0f;          // 一人称カメラの上下回転
    private float mainCameraYaw = 0f;            // 一人称カメラの左右回転
    private float subCameraPitch = 0f;           // 三人称カメラの上下回転
    private float subCameraYaw = 0f;             // 三人称カメラの左右回転

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

        if (player == null)
        {
            player = GameObject.Find("Cylinder"); // Cylinderを検索
            if (player == null)
            {
                Debug.LogError("Player (Cylinder) not found in the scene!");
            }
        }

        // RB
        cameraSwitchAction = new InputAction("CameraSwitch", InputActionType.Button);
        cameraSwitchAction.AddBinding("<Gamepad>/rightShoulder");
        cameraSwitchAction.Enable();

        // 右スティック
        cameraRotateAction = new InputAction("CameraRotate", InputActionType.Value);
        cameraRotateAction.AddBinding("<Gamepad>/rightStick");
        cameraRotateAction.Enable();

        // 初期位置設定
        if (mainCamera.activeSelf)
        {
            UpdateFirstPersonCameraPosition(mainCamera);
        }
        else
        {
            UpdateThirdPersonCameraPosition(subCamera);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // RBでカメラ切り替え
        if (cameraSwitchAction != null && cameraSwitchAction.WasPressedThisFrame())
        {
            if (mainCamera != null && subCamera != null)
            {
                if (mainCamera.activeSelf)
                {
                    mainCamera.SetActive(false);
                    subCamera.SetActive(true);
                    // メインカメラの視線方向をサブカメラに引き継ぐ
                    if (player != null && mainCamera != null)
                    {
                        Vector3 mainCameraForward = mainCamera.transform.forward;
                        Vector3 mainCameraDirection = mainCameraForward.normalized;
                        // yawを水平方向から計算
                        subCameraYaw = Mathf.Atan2(-mainCameraDirection.x, -mainCameraDirection.z) * Mathf.Rad2Deg;
                        // pitchを計算
                        subCameraPitch = -Mathf.Asin(mainCameraDirection.y) * Mathf.Rad2Deg;
                        subCameraPitch = Mathf.Clamp(subCameraPitch, -30f, 30f); // 自然な高さにする
                    }
                    UpdateThirdPersonCameraPosition(subCamera); // 三人称カメラ位置を更新
                    Debug.Log("Switched to SubCamera (Third Person)");
                }
                else
                {
                    mainCamera.SetActive(true);
                    subCamera.SetActive(false);
                    // サブカメラの視線方向をメインカメラに引き継ぐ
                    if (player != null && subCamera != null)
                    {
                        Vector3 subCameraForward = subCamera.transform.forward;
                        Vector3 subCameraDirection = subCameraForward.normalized;
                        //向きを計算
                        mainCameraYaw = Mathf.Atan2(subCameraDirection.x, subCameraDirection.z) * Mathf.Rad2Deg;
                        mainCameraPitch = -Mathf.Asin(subCameraDirection.y) * Mathf.Rad2Deg;
                        mainCameraPitch = Mathf.Clamp(mainCameraPitch, -80f, 80f);
                    }
                    UpdateFirstPersonCameraPosition(mainCamera); //カメラ位置を更新
                    Debug.Log("Switched to MainCamera (First Person)");
                }
            }
        }

        if (cameraRotateAction != null)
        {
            // 入力を取得
            Vector2 stickInput = cameraRotateAction.ReadValue<Vector2>();
            GameObject activeCamera = mainCamera.activeSelf ? mainCamera : subCamera;

            if (activeCamera != null && player != null)
            {
                // 回転を更新
                if (mainCamera.activeSelf)
                {
                    // 一人称カメラの回転
                    mainCameraYaw += stickInput.x * rotationSpeed * Time.deltaTime;
                    mainCameraPitch -= stickInput.y * rotationSpeed * Time.deltaTime;
                    mainCameraPitch = Mathf.Clamp(mainCameraPitch, -80f, 80f);
                    UpdateFirstPersonCameraPosition(mainCamera); // 一人称（自転）
                }
                else
                {
                    // 三人称カメラの回転
                    subCameraYaw += stickInput.x * rotationSpeed * Time.deltaTime;
                    subCameraPitch -= stickInput.y * rotationSpeed * Time.deltaTime;
                    subCameraPitch = Mathf.Clamp(subCameraPitch, -30f, 30f); // ピッチ制限
                    UpdateThirdPersonCameraPosition(subCamera); // 三人称
                }
            }
        }
    }
    // 三人称カメラの位置と向きを更新
    private void UpdateThirdPersonCameraPosition(GameObject camera)
    {
        if (player == null || camera == null) return;

        // カメラの位置を計算
        Vector3 playerPos = player.transform.position;
        float x = playerPos.x + Mathf.Sin(subCameraYaw * Mathf.Deg2Rad) * distance;
        float z = playerPos.z + Mathf.Cos(subCameraYaw * Mathf.Deg2Rad) * distance;
        float y = playerPos.y + height + Mathf.Sin(subCameraPitch * Mathf.Deg2Rad) * distance * 0.5f; // 高さの変化を抑える

        // カメラの位置設定
        camera.transform.position = new Vector3(x, y, z);

        // カメラをプレイヤーに向ける
        Vector3 directionToPlayer = playerPos - camera.transform.position;
        camera.transform.rotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
    }

    // 一人称カメラの位置と向きを更新
    private void UpdateFirstPersonCameraPosition(GameObject camera)
    {
        if (player == null || camera == null) return;

        // カメラをプレイヤーの位置に設定
        Vector3 playerPos = player.transform.position + new Vector3(0, height, 0);
        camera.transform.position = playerPos;

        // カメラ回転を更新
        camera.transform.rotation = Quaternion.Euler(mainCameraPitch, mainCameraYaw, 0f);
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