using System.Runtime.Serialization.Formatters;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    Vector3 moveAmount;


    public float currentSpeed;
    public float speed = 3f;
    public float runSpeed = 2.5f;
    public float jumpHight = 5;
    public Vector3 pPosition = new Vector3(0, 0, 0);
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


    public float playerRadius = 0.2f;
    public float stepHeight = 0.5f;
    public float stepSpeed = 5f;
    public float stepCheckDistance = 0.3f;



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
        stepClimbCheck();
        currentSpeed = (moveAmount != Vector3.zero && isRun) ? runSpeed : speed;
        if (moveAmount != Vector3.zero)
        {
            forwardAmount = isRun ? 1 + localPosition.z : localPosition.z;
        }
        else
        {
            forwardAmount = 0f + localPosition.z;
        }

        Vector3 move = currentSpeed * forwardAmount * transform.forward;
        move.y = pRigidbody.linearVelocity.y;
        pRigidbody.linearVelocity = move;
        pRigidbody.MoveRotation(pRigidbody.rotation * Quaternion.Euler(0, turnAmount * turnSpeed, 0));
        UpAnimation();
    }


    //アニメーションの更新
    void UpAnimation()
    {
        pAnimator.SetFloat("Forward", forwardAmount);
        pAnimator.SetFloat("TurnAmount", turnAmount);
    }



    //ジャンプと地面の当たり判定
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            pAnimator.SetBool("IsGround", true);
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isground = false;
        }
        if (!isground)
        {
            pAnimator.SetBool("IsGround", false);
        }
    }



    //新しいInputSystem
    public void Move(InputAction.CallbackContext value)
    {
        var vector2 = value.ReadValue<Vector2>();
        moveAmount = new Vector3(vector2.x, 0, vector2.y);
        localPosition = transform.InverseTransformVector(moveAmount);
        // forwardAmount = localPosition.z;
        turnAmount = Mathf.Atan2(localPosition.x, localPosition.z);
        UpAnimation();
    }
    public void Jump(InputAction.CallbackContext button)
    {
        if (isground && button.started)
        {
            //pAnimator.SetTrigger("Jump");
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
    void stepClimbCheck()
    {
        Vector3 rayUp = transform.position + Vector3.up * 0.1f;
        Vector3 rayDown = transform.position + Vector3.up * stepHeight;
        if (Physics.Raycast(rayUp, transform.forward, out RaycastHit hitlow, stepCheckDistance))
        {
            if (!Physics.Raycast(rayDown, transform.forward, out RaycastHit hitHigh, stepCheckDistance))
            {
                pRigidbody.position += new Vector3(0, stepSpeed * Time.fixedDeltaTime, 0);
            }
            // if (!Physics.SphereCast(rayDown, playerRadius, transform.forward, out RaycastHit hitHigh, stepCheckDistance))
            // {
            //     pRigidbody.position += new Vector3(0, stepSpeed * Time.fixedDeltaTime, 0);
            // }
        }
        Debug.DrawRay(rayUp, transform.forward * stepCheckDistance, Color.yellow);
        Debug.DrawRay(rayDown, transform.forward * stepCheckDistance, Color.green);
    }



}
