using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Select2p3Script : MonoBehaviour
{
    GameObject selectedObject = null;
    Collider currentTarget = null;
    private PlayerInput playerInput;
    InputAction selectAction;
    InputAction deselectAction;

    //bool isSelected = false;
    [SerializeField] TextMeshProUGUI Side2p_2pText;
    [SerializeField] TextMeshProUGUI Side1p_2pText;
    private InfoScript currentInfo;

    [SerializeField] judgeScript judgeScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        selectAction = playerInput.actions.FindAction("Select");
        deselectAction = playerInput.actions.FindAction("Deselect");
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
        Debug.Log("2p選択成功: " + obj.name);
        //isSelected = true;
        SetTarget(obj);
        Side1p_2pText.text = "Selected";

    }

    public void ResetSelect()
    {
        selectedObject = null;
        Side2p_2pText.text = "";
        Side1p_2pText.text = "";
    }

    void Deselect(GameObject obj)
    {
        selectedObject = null;
        judgeScript.DeleteBSelection();
        Debug.Log("選択解除: " + obj.name);
        //isSelected = false;
        Side2p_2pText.text = "";
        Side1p_2pText.text = "";
    }

    public void ChangeTag()
    {
        selectedObject.tag = "Untagged";
    }
    public void SetTarget(GameObject targetObject)
    {
        currentInfo = targetObject.GetComponent<InfoScript>();
        UpdateUI();
    }

    // UI更新
    private void UpdateUI()
    {
        if (currentInfo != null)
        {
            Side2p_2pText.text = currentInfo.category;
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
        deselectAction?.Disable();
        deselectAction?.Dispose();
    }
}

