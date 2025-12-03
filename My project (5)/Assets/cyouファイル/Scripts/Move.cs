using UnityEngine;

public class Movezpx : MonoBehaviour
{
    public int health = 100;
    public GameObject Maincamera;
    Vector3 offset;

    public float speed = 5f;
    public float jumpAmount = 5f;
    public float turnSpeed = 10f;
    public int Damage = 20;
    private bool isDead = false;
    public bool playerwin = false;
    public float customFallSpeed = 20f;
    public float maxFallSpeed = 25f;

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
        float X = Input.GetAxis("Horizontal");
        float Z = Input.GetAxis("Vertical");

        move = new Vector3(X, 0, Z);
        Vector3 localMove = transform.InverseTransformVector(move);

        forwardAmount = localMove.z;
        turnAmount = Mathf.Atan2(localMove.x, localMove.z);
        if (Input.GetButton("Fire1"))
        {
            forwardAmount /= 2f;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Jump");
            rg.AddForce(Vector3.up * jumpAmount, ForceMode.Impulse);
        }

        UpdateAnimator();
        Fall();
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

    public void FixedUpdate()
    {
        // Vector3 velocity = move * speed;
        // velocity.y = rg.linearVelocity.y;
        // rg.linearVelocity = velocity;
        rg.linearVelocity = forwardAmount * transform.forward * speed + rg.linearVelocity.y * Vector3.up;

        rg.MoveRotation(rg.rotation * Quaternion.Euler(0, turnAmount * turnSpeed, 0));
    }
    void UpdateAnimator()
    {
        animator.SetFloat("Forward", forwardAmount);
        animator.SetFloat("Turn", turnAmount);
    }


}
