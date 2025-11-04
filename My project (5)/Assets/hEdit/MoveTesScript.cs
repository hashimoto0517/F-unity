using UnityEngine;
using UnityEngine.InputSystem;

public class MoveTes : MonoBehaviour
{
    public float speed = 20f; // 移動速度
                             //public float rotationSpeed = 700f; // 回転速度

    float posPlusLim;
    float posMinusLim;
    float zPlusLim;
    float zMinusLim;

    private Vector2 moveInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 pos = transform.position;
        posPlusLim = pos.x + 14f;
        posMinusLim = pos.x - 14f;
        zPlusLim = pos.z + 14f ;
        zMinusLim = pos.z - 14f;

        int index = GetComponent<PlayerInput>().playerIndex;
        Camera cam = GetComponentInChildren<Camera>();
        Debug.Log("This is player: " + index);

        int maxDisplayCount = 2;
        for (int i = 0; i < maxDisplayCount && i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
            cam.targetDisplay = index;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 前後移動
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        transform.Translate(move * Time.deltaTime * speed);

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, posMinusLim, posPlusLim);
        pos.z = Mathf.Clamp(pos.z, zMinusLim, zPlusLim);
        transform.position = pos;
    }

    // Input Systemのイベントから呼ばれる
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}
