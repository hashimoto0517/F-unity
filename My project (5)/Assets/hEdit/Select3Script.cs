using UnityEngine;
using UnityEngine.InputSystem;

public class Select3Script : MonoBehaviour
{
    [SerializeField] Material selectedColor;
    [SerializeField] Material normalColor;

    GameObject selectedObject = null;
    Collider currentTarget = null;
    InputAction selectAction;
    InputAction deselectAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectAction = new InputAction("Select", InputActionType.Button);
        selectAction.AddBinding("<Gamepad>/buttonNorth"); // Yボタン
        selectAction.AddBinding("<Mouse>/leftButton");    // 左クリック
        selectAction.Enable();

        deselectAction = new InputAction("Select", InputActionType.Button);
        deselectAction.AddBinding("<Gamepad>/buttonEast"); // Yボタン
        deselectAction.AddBinding("<Mouse>/rightButton");    // 左クリック
        deselectAction.Enable();

    }

    // Update is called once per frame
    void Update()
    {
        if (selectAction != null && selectAction.WasPressedThisFrame())
        {
            TrySelect();
        }

        if (deselectAction != null && deselectAction.WasPressedThisFrame())
        {
            TryDeselect();
        }
    }

    private void TrySelect()
    {
        if (currentTarget == null || !currentTarget.CompareTag("Target"))
            return;
        Select(currentTarget.gameObject);
    }

    private void TryDeselect()
    {
        if (currentTarget == null || !currentTarget.CompareTag("Target"))
            return;
        Deselect(currentTarget.gameObject);
    }

    void Select(GameObject obj)
    {
        //違うものを選択した
        if (selectedObject != null && selectedObject != obj)
        {
            Deselect(selectedObject);
        }

        //新たに選択したオブジェクト
        var rend = obj.GetComponent<Renderer>();
        if (rend != null && selectedObject != obj)
        {
            rend.material = selectedColor;
        }
        //else if (selectedObject != null && selectedObject == obj)//選択解除
        //{
        //    Deselect(selectedObject);
        //}
        else
        {
            Debug.Log("Rendererが見つかりません: " + obj.name);
        }

        selectedObject = obj;
        Debug.Log("選択成功: " + obj.name);
    }

    void Deselect(GameObject obj)
    {
        var rend = obj.GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material = normalColor;
            selectedObject = null;
            Debug.Log("選択解除: " + obj.name);
        }
        else
        {
            Debug.Log("選択解除対象にRendererが見つかりません: " + obj.name);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target")) 
            currentTarget = other;
    }

    void OnTriggerExit(Collider other)
    {
        if (other == currentTarget) 
            currentTarget = null;
    }

    void OnDestroy()
    {
        selectAction?.Disable();
        selectAction?.Dispose();
    }
}

