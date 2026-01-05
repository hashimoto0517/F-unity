//テスト用だから消す

using UnityEngine;
using UnityEngine.InputSystem;

public class MoveTes : MonoBehaviour
{
    public float speed = 20f; // 移動速度

    float posPlusLim;
    float posMinusLim;
    float zPlusLim;
    float zMinusLim;

    private Vector2 moveInput;

    [SerializeField] Camera mainCamera;
    [SerializeField] Camera subCamera;

    void Start()
    {
        // 初期位置を基準に移動範囲を決定
        Vector3 pos = transform.position;
        posPlusLim = pos.x + 14f;
        posMinusLim = pos.x - 14f;
        zPlusLim = pos.z + 14f;
        zMinusLim = pos.z - 14f;

        // デバッグ用：プレイヤー番号を確認
        int index = GetComponent<PlayerInput>().playerIndex;
        Debug.Log("This is player: " + index);
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        { 
            ToggleViewMode(); 
        }

        // 入力に基づいて移動
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        transform.Translate(move * Time.deltaTime * speed);

        // 移動範囲を制限
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

    void ToggleViewMode()
    { 
        bool mainActive = mainCamera.gameObject.activeSelf;
        mainCamera.gameObject.SetActive(!mainActive);
        subCamera.gameObject.SetActive(mainActive); 
    }
}