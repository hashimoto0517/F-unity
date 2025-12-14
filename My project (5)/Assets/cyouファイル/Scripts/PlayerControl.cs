using System.Runtime.Serialization.Formatters;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{

    public float currentSpeed;
    public float speed = 10f;
    public float runSpeed = 15f;
    public float jumpHight = 25;
    public float forwardAmount;
    public Vector3 moveAmount;




    public float turnAmount;
    public float turnSpeed = 20f;
    private Rigidbody pRigidbody;
    private Animator pAnimator;



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

    public float customFallSpeed = 65;
    public float maxFallSpeed = 100;
    public AudioSource playerAudio;
    public AudioSource actionAudio;
    public AudioClip runAudio;
    public AudioClip walkAudio;
    public AudioClip jumpAudio;



    // public bool isFrontObstacle = false;
    // public float obstacleExtraJump = 70;
    // public float obstacleExtraRunJump = 100;

    // public float frontCheckDistance = 0.6f;
    // public float rayHight = 0.2f;
    //CapsuleCollider capsule;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pRigidbody = GetComponent<Rigidbody>();
        pAnimator = GetComponent<Animator>();
        //playerAudio = GetComponent<AudioSource>();
        //capsule = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        RayCheckGround();
        StopAudio();
    }
    void FixedUpdate()
    {
        StepClimbCheck();
        Fall();
        //CheckFrontObstacle_Capsule();
        //CheckFrontObstacle();

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
        if (!isground || !button.started)
        {
            return;
        }
        // if (playerAudio.isPlaying)
        // {
        //     playerAudio.Stop();
        // }
        pAnimator.SetTrigger("JumpTrigger");
        actionAudio.PlayOneShot(jumpAudio);
        pRigidbody.AddForce(Vector3.up * jumpHight, ForceMode.Impulse);
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

        if (!isground)
        {
            pRigidbody.linearVelocity = new Vector3(desiredMove.x, yAmount, desiredMove.z);
        }
        else
        {
            pRigidbody.linearVelocity = desiredMove;
        }


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

        if (!isground)
        {
            pRigidbody.linearVelocity = new Vector3(desiredMove.x, yAmount, desiredMove.z);
        }
        else
        {
            pRigidbody.linearVelocity = desiredMove;
        }


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

    void Fall()
    {
        Vector3 velocity = pRigidbody.linearVelocity;

        if (!isground)
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
        pRigidbody.linearVelocity = velocity;
    }

    // void CheckFrontObstacle()
    // {
    //     Vector3 current = transform.position + Vector3.up * rayHight;
    //     Ray ray = new Ray(current, transform.forward);
    //     if (Physics.Raycast(ray, out RaycastHit hit, frontCheckDistance, groundLayer))
    //     {
    //         isFrontObstacle = true;
    //     }
    //     else
    //     {
    //         isFrontObstacle = false;
    //     }
    //     Debug.DrawRay(current, transform.forward * frontCheckDistance, isFrontObstacle ? Color.red : Color.green);
    // }

    // void CheckFrontObstacle_Capsule()
    // {
    //     if (!capsule)
    //     {
    //         return;
    //     }
    //     float height = Mathf.Max(capsule.height, capsule.radius * 2);
    //     Vector3 center = transform.TransformPoint(capsule.center);

    //     Vector3 position1 = center + Vector3.up * (height / 2 - capsule.radius);
    //     Vector3 position2 = center + Vector3.down * (height / 2 - capsule.radius);
    //     if (Physics.CapsuleCast(position1, position2, capsule.radius * 0.95f, transform.forward, out RaycastHit hit, frontCheckDistance))
    //     {
    //         isFrontObstacle = true;
    //     }
    //     else
    //     {
    //         isFrontObstacle = false;
    //     }
    //     Debug.DrawLine(position1, position2 + transform.forward * frontCheckDistance, Color.yellow);
    // }
    public void PlayFootStepAudio()
    {
        if (!isground)
        {
            return;
        }
        if (forwardAmount < 0.1f)
        {
            return;
        }
        if (playerAudio.isPlaying)
        {
            return;
        }
        AudioClip audioClip = forwardAmount > 1.5f ? runAudio : walkAudio;
        if (audioClip == null)
        {
            return;
        }
        //playerAudio.PlayOneShot(audioClip);
        playerAudio.clip = audioClip;
        playerAudio.Play();
    }
    void StopAudio()
    {
        if (!isground && playerAudio.isPlaying)
        {
            playerAudio.Stop();
            return;
        }
        if (forwardAmount < 0.1f && playerAudio.isPlaying)
        {
            playerAudio.Stop();
        }
    }
}
