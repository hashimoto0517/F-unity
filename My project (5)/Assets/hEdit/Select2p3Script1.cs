using UnityEngine;
using UnityEngine.InputSystem;

public class Select2p3Script : MonoBehaviour
{
    GameObject selectedObject = null;
    Collider currentTarget = null;
    InputAction selectAction;
    InputAction deselectAction;

    //bool isSelected = false;

    [SerializeField] judgeScript judgeScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectAction = new InputAction("Select", InputActionType.Button);
        selectAction.AddBinding("<Gamepad>/buttonNorth"); // Yボタン
        selectAction.AddBinding("<Mouse>/leftButton");    // 左クリック
        selectAction.Enable();

        deselectAction = new InputAction("Deselect", InputActionType.Button);
        deselectAction.AddBinding("<Gamepad>/buttonEast"); // Yボタン
        deselectAction.AddBinding("<Mouse>/rightButton");    // 右クリック
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
        selectedObject = obj;
        judgeScript.SetBSelection(obj);
        Debug.Log("選択成功: " + obj.name);
        //isSelected = true;
    }

    public void ResetSelect()
    {
        selectedObject = null;
    }

    void Deselect(GameObject obj)
    {
        selectedObject = null;
        judgeScript.DeleteBSelection();
        Debug.Log("選択解除: " + obj.name);
        //isSelected = false;
    }

    public void ChangeTag()
    {
        selectedObject.tag = "Untagged";
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
        deselectAction?.Disable();
        deselectAction?.Dispose();
    }
}

