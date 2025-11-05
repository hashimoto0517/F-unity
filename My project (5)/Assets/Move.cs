using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{
    public float speed = 20f; // ˆÚ“®‘¬“x

    float posPlusLim;
    float posMinusLim;

    void Start()
    {
        Vector3 pos = transform.position;
        posPlusLim = pos.x + 39f;
        posMinusLim = pos.x - 39f;
    }

    void Update()
    {
        // ‘OŒã¶‰EˆÚ“®
        Vector3 move = Vector3.forward * Input.GetAxis("Vertical")
                     + Vector3.right * Input.GetAxis("Horizontal");
        transform.Translate(move * Time.deltaTime * speed);

        // ˆÚ“®”ÍˆÍ‚ğ§ŒÀ
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, posMinusLim, posPlusLim);
        pos.z = Mathf.Clamp(pos.z, posMinusLim, posPlusLim);
        transform.position = pos;
    }
}