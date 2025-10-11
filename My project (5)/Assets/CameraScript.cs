using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    [SerializeField] GameObject mainCamera;      // ���C���J�����i�[�p
    [SerializeField] GameObject subCamera;       // �T�u�J�����i�[�p 
    [SerializeField] float rotationSpeed = 100f; // �J�����̉�]���x
    [SerializeField] GameObject player;//�v���C���[�i�[
    [SerializeField] float distance = 5f;//�v���C���[�Ƃ̋���
    [SerializeField] float height = 2f;//�J��������
    private InputAction cameraSwitchAction;       // RB�{�^���̓���
    private InputAction cameraRotateAction;       // �E�X�e�B�b�N�̓���

    private float pitch = 0f;                    // �㉺��]
    private float yaw = 0f;                      // ���E��]

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // �T�u�J�������A�N�e�B�u�ɂ���
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

        // RB�{�^��
        cameraSwitchAction = new InputAction("CameraSwitch", InputActionType.Button);
        cameraSwitchAction.AddBinding("<Gamepad>/rightShoulder");
        cameraSwitchAction.Enable();

        // �E�X�e�B�b�N
        cameraRotateAction = new InputAction("CameraRotate", InputActionType.Value);
        cameraRotateAction.AddBinding("<Gamepad>/rightStick");
        cameraRotateAction.Enable();

        //�����ʒu�ݒ�
        UpdateCameraPosition(mainCamera);
    }

    // Update is called once per frame
    void Update()
    {
        // RB�{�^���ŃJ�����؂�ւ�
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

        // �E�X�e�B�b�N�ŃJ������]
        if (cameraRotateAction != null)
        {
            // �E�X�e�B�b�N�̓��͂��擾
            Vector2 stickInput = cameraRotateAction.ReadValue<Vector2>();
            GameObject activeCamera = mainCamera.activeSelf ? mainCamera : subCamera;

            if (activeCamera != null)
            {
                //���E��]�Ə㉺��]���v�Z
                yaw += stickInput.x * rotationSpeed * Time.deltaTime;
                pitch -= stickInput.y * rotationSpeed * Time.deltaTime;

                // ��]�𐧌�
                pitch = Mathf.Clamp(pitch, -80f, 80f);

                // �J�����̉�]���X�V
                UpdateCameraPosition(activeCamera);
            }
        }
    }

    private void UpdateCameraPosition(GameObject camera)
    {
        if (player == null || camera == null) return;
        //�v���C���[����J�����ʒu�̐ݒ�
        Vector3 playerPos = player.transform.position;
        float x = playerPos.x + Mathf.Sin(yaw * Mathf.Deg2Rad) * distance;
        float z = playerPos.z + Mathf.Cos(yaw * Mathf.Deg2Rad) * distance;
        float y = playerPos.y + height + Mathf.Sin(pitch * Mathf.Deg2Rad) * distance;
        // �J�����̈ʒu�ݒ�
        camera.transform.position = new Vector3(x, y, z);

        // �J�������v���C���[�̕����Ɍ�����
        camera.transform.LookAt(playerPos);
    }
    // �X�N���v�g���j�������ۂɃA�N�V�����𖳌���
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