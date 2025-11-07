using System.Runtime.Serialization.Formatters;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    Vector3 moveAmount;


    public float currentSpeed;
    public float speed = 3f;
    public float runSpeed = 5f;
    public float jumpHight = 5;
    public float forwardAmount;




    public float turnAmount;
    public float turnSpeed = 20f;
    public Rigidbody pRigidbody;
    public Animator pAnimator;



    public float rayDepth = 0.13f;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;
    public bool isground = true;
    public bool isRun = false;
    public Vector3 localPosition;



    public float stepHeight = 0.5f;
    public float stepSpeed = 2.5f;
    public float stepCheckDistance = 0.4f;



    public Transform mainCameraTransform;
    public Transform subCameraTransform;
    public GameObject mainCamera;
    public bool isFirstPerson = false;





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pRigidbody = GetComponent<Rigidbody>();
        pAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        RayCheckGround();
    }
    void FixedUpdate()
    {
        StepClimbCheck();

        //一人称視点かどうかチェックすること
        if (mainCamera.activeSelf)
        {
            isFirstPerson = true;
        }
        else
        {
            isFirstPerson = false;
        }

        //三人称視点
        if (!isFirstPerson)
        {
            ThirdPerson();
        }


        //一人称視点
        if (isFirstPerson)
        {
            FirstPerson();
        }

    }


    //アニメーションの更新
    void UpdateAnimation()
    {
        pAnimator.SetFloat("Forward", forwardAmount);
        pAnimator.SetFloat("TurnAmount", turnAmount);
    }
    //新しいInputSystem
    public void Move(InputAction.CallbackContext value)
    {
        var vector2 = value.ReadValue<Vector2>();
        moveAmount = new Vector3(vector2.x, 0, vector2.y);
    }
    public void Jump(InputAction.CallbackContext button)
    {
        if (isground && button.started)
        {
            pAnimator.SetTrigger("JumpTrigger");
            pRigidbody.AddForce(Vector3.up * jumpHight, ForceMode.Impulse);
        }
    }
    public void Run(InputAction.CallbackContext button)
    {
        //isRun = button.ReadValueAsButton();

        if (button.performed)
        {
            isRun = true;
        }
        else if (button.canceled)
        {
            isRun = false;
        }
    }

    //プレイヤーが床にいるかどうか
    void RayCheckGround()
    {
        var a = transform.position + Vector3.up * rayDepth;
        Ray ray = new Ray(a, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, groundCheckDistance, groundLayer))
        {
            isground = true;
        }
        else
        {
            isground = false;
        }
        Debug.DrawRay(a, Vector3.down * groundCheckDistance, isground ? Color.green : Color.red);

    }

    //段差に上る
    void StepClimbCheck()
    {
        Vector3 rayUp = transform.position + Vector3.up * 0.1f;
        Vector3 rayDown = transform.position + Vector3.up * stepHeight;
        if (Physics.Raycast(rayUp, transform.forward, out RaycastHit hitlow, stepCheckDistance))
        {
            if (!Physics.Raycast(rayDown, transform.forward, out RaycastHit hitHigh, stepCheckDistance))
            {
                pRigidbody.position += new Vector3(0, stepSpeed * Time.fixedDeltaTime, 0);
            }
        }
        Debug.DrawRay(rayUp, transform.forward * stepCheckDistance, Color.yellow);
        Debug.DrawRay(rayDown, transform.forward * stepCheckDistance, Color.green);
    }

    public void FirstPerson()
    {
        if (mainCameraTransform == null)
        {
            return;
        }

        float yAmount = pRigidbody.linearVelocity.y;

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
        currentSpeed = isRun ? runSpeed : speed;

        //移動ベクトル
        Vector3 moveDir = cameraForward * playerInput.z + cameraRight * playerInput.x;
        Vector3 desiredMove = moveDir * currentSpeed;
        desiredMove.y = yAmount;


        pRigidbody.linearVelocity = desiredMove;

        //カメラの回転
        Vector3 lookDir = moveDir;
        lookDir.y = 0;
        if (lookDir.sqrMagnitude > 0.001f)
        {
            pRigidbody.rotation = Quaternion.Slerp(
                pRigidbody.rotation,
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
    public void ThirdPerson()
    {
        if (subCameraTransform == null)
        {
            return;
        }

        float yAmount = pRigidbody.linearVelocity.y;

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
        currentSpeed = isRun ? runSpeed : speed;

        //移動ベクトル
        Vector3 moveDir = cameraForward * playerInput.z + cameraRight * playerInput.x;
        Vector3 desiredMove = moveDir * currentSpeed;
        desiredMove.y = yAmount;


        pRigidbody.linearVelocity = desiredMove;

        //カメラの回転
        Vector3 lookDir = moveDir;
        lookDir.y = 0;
        if (lookDir.sqrMagnitude > 0.001f)
        {
            pRigidbody.rotation = Quaternion.Slerp(
                pRigidbody.rotation,
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
