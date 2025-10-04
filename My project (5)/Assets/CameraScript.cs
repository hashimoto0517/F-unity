using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    [SerializeField] GameObject mainCamera;      // ���C���J�����i�[�p
    [SerializeField] GameObject subCamera;       // �T�u�J�����i�[�p 
    [SerializeField] float rotationSpeed = 100f; // �J�����̉�]���x
    private InputAction cameraSwitchAction;       // RB�{�^���̓��̓A�N�V����
    private InputAction cameraRotateAction;       // �E�X�e�B�b�N�̓��̓A�N�V����

    private float pitch = 0f;                    // �J�����̏㉺��]�i�s�b�`�j
    private float yaw = 0f;                      // �J�����̍��E��]�i���[�j

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

        // RB�{�^���̃A�N�V������ݒ�
        cameraSwitchAction = new InputAction("CameraSwitch", InputActionType.Button);
        cameraSwitchAction.AddBinding("<Gamepad>/rightShoulder");
        cameraSwitchAction.Enable();

        // �E�X�e�B�b�N�̃A�N�V������ݒ�
        cameraRotateAction = new InputAction("CameraRotate", InputActionType.Value);
        cameraRotateAction.AddBinding("<Gamepad>/rightStick");
        cameraRotateAction.Enable();
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

        // �E�X�e�B�b�N�ŃJ������]
        if (cameraRotateAction != null)
        {
            // �E�X�e�B�b�N�̓��͂��擾�ix: ���E, y: �㉺�j
            Vector2 stickInput = cameraRotateAction.ReadValue<Vector2>();
            GameObject activeCamera = mainCamera.activeSelf ? mainCamera : subCamera;

            if (activeCamera != null)
            {
                // ���[�i���E��]�j�ƃs�b�`�i�㉺��]�j���v�Z
                yaw += stickInput.x * rotationSpeed * Time.deltaTime;
                pitch -= stickInput.y * rotationSpeed * Time.deltaTime;

                // �s�b�`�𐧌��i��≺���������Ȃ��悤��-80�`80�x�ɐ����j
                pitch = Mathf.Clamp(pitch, -80f, 80f);

                // �A�N�e�B�u�ȃJ�����̉�]���X�V
                activeCamera.transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
            }
        }
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