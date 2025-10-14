using UnityEngine;
using UnityEngine.InputSystem;

public class MoveTes : MonoBehaviour
{
    public float speed = 20f; // ˆÚ“®‘¬“x
                             //public float rotationSpeed = 700f; // ‰ñ“]‘¬“x

    float posPlusLim;
    float posMinusLim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 pos = transform.position;
        posPlusLim = pos.x + 39f;
        posMinusLim = pos.x - 39f;

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
        pos.x = Mathf.Clamp(pos.x, posMinusLim, posPlusLim);
        pos.z = Mathf.Clamp(pos.z, posMinusLim, posPlusLim);
        transform.position = pos;
    }
}
