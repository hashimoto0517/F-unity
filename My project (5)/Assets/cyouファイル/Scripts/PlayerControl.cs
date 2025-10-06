using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float speed = 3f;
    public float runSpeed = 6f;
    public float jumpHight = 10;
    public float playerSpeed;
    public Vector3 pPosition = new Vector3(0, 0, 0);
    public float forwardAmount;



    public float turnAmount;
    public float turnSpeed = 10f;
    public Rigidbody pRigidbody;
    public Animator pAnimator;




    //public LayerMask groundLayer;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pRigidbody = GetComponent<Rigidbody>();
        pAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        speed = 3f;
        pPosition.x = Input.GetAxis("Horizontal");
        pPosition.z = Input.GetAxis("Vertical");

        Vector3 localPosition = transform.InverseTransformVector(pPosition);
        forwardAmount = localPosition.z;

        turnAmount = Mathf.Atan2(localPosition.x, localPosition.z);


        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (pPosition.x != 0 || pPosition.z != 0)
            {
                forwardAmount = 2;
                speed = runSpeed;
            }

        }
        if (Input.GetKeyDown(KeyCode.Space) && pAnimator.GetBool("IsGround"))
        {
            pAnimator.SetTrigger("Jump");
            pRigidbody.AddForce(Vector3.up * jumpHight, ForceMode.Impulse);
        }



        UpAnimation();


    }
    void FixedUpdate()
    {
        Vector3 move = speed * forwardAmount * transform.forward;
        move.y = pRigidbody.linearVelocity.y;
        pRigidbody.linearVelocity = move;
        pRigidbody.MoveRotation(pRigidbody.rotation * Quaternion.Euler(0, turnAmount * turnSpeed, 0));
    }

    void UpAnimation()
    {
        pAnimator.SetFloat("Forward", forwardAmount);
        pAnimator.SetFloat("TurnAmount", turnAmount);
    }


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
            pAnimator.SetBool("IsGround", false);
        }
    }


}
