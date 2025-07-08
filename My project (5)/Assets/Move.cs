using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{
    public float speed = 5f; // ˆÚ“®‘¬“x
    //public float rotationSpeed = 700f; // ‰ñ“]‘¬“x
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int maxDisplayCount = 2;
        for (int i = 0; i < maxDisplayCount && i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ‘OŒãˆÚ“®
        Vector3 move = Vector3.forward * Input.GetAxis("Vertical") + Vector3.right * Input.GetAxis("Horizontal");
        transform.Translate(move * Time.deltaTime * speed);

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -14.5f, 14.5f);
        pos.z = Mathf.Clamp(pos.z, -14.5f, 14.5f);
        transform.position = pos;
    }
}
