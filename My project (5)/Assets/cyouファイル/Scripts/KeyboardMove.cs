using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardMove : MonoBehaviour
{
    public GameObject mainCamera;
    public Transform mainCameraTransform;
    public Transform subCameraTransform;


    public float currentSpeed = 0f;
    public float walkSpeed = 5;
    public float runSpeed = 10;

    public float jumpHight = 5f;
    public float turnSpeed = 10f;
    public float MouseTurnSpeed = 0f;
    public float customFallSpeed = 20f;
    public float maxFallSpeed = 25f;
    public bool isRun = false;
    public bool isGround = true;
    public float rayDepth = 0f;
    public LayerMask ground;
    public Vector3 moveAmount;
    public Vector3 mouseAmount;
    public bool isFirstPerson = false;
    public float groundCheckDistance;
    public float cameraPitch;


    Animator animator;
    Vector3 move;
    Rigidbody rg;
    float forwardAmount;
    float turnAmount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rg = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Fall();
        if (isFirstPerson)
        {
            float mouseX = mouseAmount.x;
            transform.Rotate(Vector3.up * mouseX * MouseTurnSpeed * Time.deltaTime);

            // 垂直旋转摄像机
            float mouseY = mouseAmount.y;
            cameraPitch -= mouseY * MouseTurnSpeed * Time.deltaTime;
            cameraPitch = Mathf.Clamp(cameraPitch, -60, 60);

            mainCameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0, 0);
        }
    }
    public void FixedUpdate()
    {
        CheckedIsGround();
        if (mainCamera.activeSelf)
        {
            isFirstPerson = true;
        }
        else
        {
            isFirstPerson = false;
        }
        if (isFirstPerson)
            FirstPerson();
        else
            ThirdPerson();
    }
    void Fall()
    {
        Vector3 velocity = rg.linearVelocity;

        if (transform.position.y > 0.1f)
        {
            velocity.y -= customFallSpeed * Time.fixedDeltaTime;

            if (velocity.y < -maxFallSpeed)
            {
                velocity.y = -maxFallSpeed;
            }
        }
        else
        {
            if (velocity.y < 0)
            {
                velocity.y = -0.5f;
            }
        }
        rg.linearVelocity = velocity;
    }
    void UpdateAnimation()
    {
        animator.SetFloat("Forward", forwardAmount);
        animator.SetFloat("Turn", turnAmount);
    }
    public void Move(InputAction.CallbackContext value)
    {
        var vector2 = value.ReadValue<Vector2>();
        moveAmount = new Vector3(vector2.x, 0, vector2.y);
    }
    public void Run(InputAction.CallbackContext button)
    {
        if (!button.performed)
        {
            isRun = false;
        }
        if (button.performed)
        {
            isRun = true;
        }
    }
    public void Jump(InputAction.CallbackContext button)
    {
        if (!isGround || !button.started)
        {
            return;
        }
        animator.SetTrigger("JumpTrigger");
        rg.AddForce(Vector3.up * jumpHight, ForceMode.Impulse);
    }
    public void MouseAmount(InputAction.CallbackContext value)
    {
        var vector2 = value.ReadValue<Vector2>();
        mouseAmount = new Vector3(vector2.x, 0, vector2.y);
    }
    public void CheckedIsGround()
    {
        var a = transform.position + Vector3.up * rayDepth;
        Ray ray = new Ray(a, Vector3.down);
        if (!Physics.Raycast(ray, out RaycastHit hitInfo, groundCheckDistance, ground))
        {
            isGround = false;
        }
        else
        {
            isGround = true;
        }
        Debug.DrawRay(a, Vector3.down * groundCheckDistance, isGround ? Color.green : Color.red);
    }
    public void FirstPerson()
    {
        if (mainCameraTransform == null)
        {
            return;
        }

        float yAmount = rg.linearVelocity.y;

        //カメラの垂直の方向へ移動する量
        Vector3 cameraForward = mainCameraTransform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();


        //カメラの水平の方向へ回す量
        Vector3 cameraRight = mainCameraTransform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();


        //プレイヤーの移動量
        Vector3 playerInput = new Vector3(moveAmount.x, 0, moveAmount.z);

        if (playerInput.magnitude > 1f)
        {
            playerInput.Normalize();
        }


        //走るか歩くか
        currentSpeed = isRun ? runSpeed : walkSpeed;

        //移動ベクトル
        Vector3 moveDir = cameraForward * playerInput.z + cameraRight * playerInput.x;
        Vector3 desiredMove = moveDir * currentSpeed;
        desiredMove.y = yAmount;

        if (!isGround)
        {
            rg.linearVelocity = new Vector3(desiredMove.x, yAmount, desiredMove.z);
        }
        else
        {
            rg.linearVelocity = desiredMove;
        }


        rg.linearVelocity = desiredMove;

        //アニメーション更新
        if (moveAmount == Vector3.zero)
        {
            forwardAmount = 0;
        }
        else
        {
            forwardAmount = isRun ? 2 : 1;
        }
        UpdateAnimation();
    }
    public void ThirdPerson()
    {
        if (subCameraTransform == null)
        {
            return;
        }
        float yAmount = rg.linearVelocity.y;



        //カメラの垂直の方向へ移動する量
        Vector3 cameraForward = subCameraTransform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();


        //カメラの水平の方向へ回す量
        Vector3 cameraRight = subCameraTransform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();


        //プレイヤーの移動量
        Vector3 playerInput = new Vector3(moveAmount.x, 0, moveAmount.z);

        if (playerInput.magnitude > 1f)
        {
            playerInput.Normalize();
        }
        //走るか歩くか
        currentSpeed = isRun ? runSpeed : walkSpeed;

        //移動ベクトル
        Vector3 moveDir = cameraForward * playerInput.z + cameraRight * playerInput.x;
        Vector3 desiredMove = moveDir * currentSpeed;
        desiredMove.y = yAmount;

        if (!isGround)
        {
            rg.linearVelocity = new Vector3(desiredMove.x, yAmount, desiredMove.z);
        }
        else
        {
            rg.linearVelocity = desiredMove;
        }


        rg.linearVelocity = desiredMove;

        //カメラの回転
        Vector3 lookDir = moveDir;
        lookDir.y = 0;
        if (lookDir.sqrMagnitude > 0.001f)
        {
            rg.rotation = Quaternion.Slerp
            (
                rg.rotation,
                Quaternion.LookRotation(lookDir),
                turnSpeed * Time.fixedDeltaTime
            );
        }

        //アニメーション更新
        if (moveAmount == Vector3.zero)
        {
            forwardAmount = 0;
        }
        else
        {
            forwardAmount = isRun ? 2 : 1;
        }
        UpdateAnimation();

    }


}
