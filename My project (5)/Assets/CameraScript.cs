using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    [SerializeField] GameObject mainCamera;      // ���C���J�����i�[�p
    [SerializeField] GameObject subCamera;       // �T�u�J�����i�[�p
    [SerializeField] float rotationSpeed = 100f; // �J�����̉�]���x
    [SerializeField] GameObject player;          // �v���C���[�i�[
    [SerializeField] float distance = 5f;        // �v���C���[�Ƃ̋���
    [SerializeField] float height = 2f;          // �J��������
    private InputAction cameraSwitchAction;      // RB�{�^���̓���
    private InputAction cameraRotateAction;      // �E�X�e�B�b�N�̓���

    private float mainCameraPitch = 0f;          // ��l�̃J�����̏㉺��]
    private float mainCameraYaw = 0f;            // ��l�̃J�����̍��E��]
    private float subCameraPitch = 0f;           // �O�l�̃J�����̏㉺��]
    private float subCameraYaw = 0f;             // �O�l�̃J�����̍��E��]

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

        if (player == null)
        {
            player = GameObject.Find("Cylinder"); // Cylinder������
            if (player == null)
            {
                Debug.LogError("Player (Cylinder) not found in the scene!");
            }
        }

        // RB
        cameraSwitchAction = new InputAction("CameraSwitch", InputActionType.Button);
        cameraSwitchAction.AddBinding("<Gamepad>/rightShoulder");
        cameraSwitchAction.Enable();

        // �E�X�e�B�b�N
        cameraRotateAction = new InputAction("CameraRotate", InputActionType.Value);
        cameraRotateAction.AddBinding("<Gamepad>/rightStick");
        cameraRotateAction.Enable();

        // �����ʒu�ݒ�
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
        // RB�ŃJ�����؂�ւ�
        if (cameraSwitchAction != null && cameraSwitchAction.WasPressedThisFrame())
        {
            if (mainCamera != null && subCamera != null)
            {
                if (mainCamera.activeSelf)
                {
                    mainCamera.SetActive(false);
                    subCamera.SetActive(true);
                    // ���C���J�����̎����������T�u�J�����Ɉ����p��
                    if (player != null && mainCamera != null)
                    {
                        Vector3 mainCameraForward = mainCamera.transform.forward;
                        Vector3 mainCameraDirection = mainCameraForward.normalized;
                        // yaw�𐅕���������v�Z
                        subCameraYaw = Mathf.Atan2(-mainCameraDirection.x, -mainCameraDirection.z) * Mathf.Rad2Deg;
                        // pitch���v�Z
                        subCameraPitch = -Mathf.Asin(mainCameraDirection.y) * Mathf.Rad2Deg;
                        subCameraPitch = Mathf.Clamp(subCameraPitch, -30f, 30f); // ���R�ȍ����ɂ���
                    }
                    UpdateThirdPersonCameraPosition(subCamera); // �O�l�̃J�����ʒu���X�V
                    Debug.Log("Switched to SubCamera (Third Person)");
                }
                else
                {
                    mainCamera.SetActive(true);
                    subCamera.SetActive(false);
                    // �T�u�J�����̎������������C���J�����Ɉ����p��
                    if (player != null && subCamera != null)
                    {
                        Vector3 subCameraForward = subCamera.transform.forward;
                        Vector3 subCameraDirection = subCameraForward.normalized;
                        //�������v�Z
                        mainCameraYaw = Mathf.Atan2(subCameraDirection.x, subCameraDirection.z) * Mathf.Rad2Deg;
                        mainCameraPitch = -Mathf.Asin(subCameraDirection.y) * Mathf.Rad2Deg;
                        mainCameraPitch = Mathf.Clamp(mainCameraPitch, -80f, 80f);
                    }
                    UpdateFirstPersonCameraPosition(mainCamera); //�J�����ʒu���X�V
                    Debug.Log("Switched to MainCamera (First Person)");
                }
            }
        }

        if (cameraRotateAction != null)
        {
            // ���͂��擾
            Vector2 stickInput = cameraRotateAction.ReadValue<Vector2>();
            GameObject activeCamera = mainCamera.activeSelf ? mainCamera : subCamera;

            if (activeCamera != null && player != null)
            {
                // ��]���X�V
                if (mainCamera.activeSelf)
                {
                    // ��l�̃J�����̉�]
                    mainCameraYaw += stickInput.x * rotationSpeed * Time.deltaTime;
                    mainCameraPitch -= stickInput.y * rotationSpeed * Time.deltaTime;
                    mainCameraPitch = Mathf.Clamp(mainCameraPitch, -80f, 80f);
                    UpdateFirstPersonCameraPosition(mainCamera); // ��l�́i���]�j
                }
                else
                {
                    // �O�l�̃J�����̉�]
                    subCameraYaw += stickInput.x * rotationSpeed * Time.deltaTime;
                    subCameraPitch -= stickInput.y * rotationSpeed * Time.deltaTime;
                    subCameraPitch = Mathf.Clamp(subCameraPitch, -30f, 30f); // �s�b�`����
                    UpdateThirdPersonCameraPosition(subCamera); // �O�l��
                }
            }
        }
    }
    // �O�l�̃J�����̈ʒu�ƌ������X�V
    private void UpdateThirdPersonCameraPosition(GameObject camera)
    {
        if (player == null || camera == null) return;

        // �J�����̈ʒu���v�Z
        Vector3 playerPos = player.transform.position;
        float x = playerPos.x + Mathf.Sin(subCameraYaw * Mathf.Deg2Rad) * distance;
        float z = playerPos.z + Mathf.Cos(subCameraYaw * Mathf.Deg2Rad) * distance;
        float y = playerPos.y + height + Mathf.Sin(subCameraPitch * Mathf.Deg2Rad) * distance * 0.5f; // �����̕ω���}����

        // �J�����̈ʒu�ݒ�
        camera.transform.position = new Vector3(x, y, z);

        // �J�������v���C���[�Ɍ�����
        Vector3 directionToPlayer = playerPos - camera.transform.position;
        camera.transform.rotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
    }

    // ��l�̃J�����̈ʒu�ƌ������X�V
    private void UpdateFirstPersonCameraPosition(GameObject camera)
    {
        if (player == null || camera == null) return;

        // �J�������v���C���[�̈ʒu�ɐݒ�
        Vector3 playerPos = player.transform.position + new Vector3(0, height, 0);
        camera.transform.position = playerPos;

        // �J������]���X�V
        camera.transform.rotation = Quaternion.Euler(mainCameraPitch, mainCameraYaw, 0f);
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