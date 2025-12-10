using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Select3Script : MonoBehaviour
{
    [SerializeField] int playerNumber; // 1 or 2
    [SerializeField] TextMeshProUGUI sideTextA;
    [SerializeField] TextMeshProUGUI sideTextB;
    [SerializeField] judgeScript judgeScript;

    private GameObject selectedObject = null;
    private Collider currentTarget = null;
    private PlayerInput playerInput;
    private InputAction selectAction;
    private InputAction deselectAction;
    private InfoScript currentInfo;


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
        if (selectedObject != null && selectedObject != obj)
            Deselect(selectedObject);

        selectedObject = obj;

        judgeScript.SetSelection(playerNumber, obj);

        Debug.Log($"{playerNumber}pëIëê¨å˜: {obj.name}");

        SetTarget(obj);
        sideTextB.text = "Selected";
    }

    public void ResetSelect()
    {
        selectedObject = null;
        sideTextA.text = "";
        sideTextB.text = "";
    }

    void Deselect(GameObject obj)
    {
        selectedObject = null;

        judgeScript.DeleteSelection(playerNumber);

        Debug.Log($"{playerNumber}pëIëâèú: {obj.name}");

        sideTextA.text = "";
        sideTextB.text = "";
    }

    public void SetTarget(GameObject targetObject)
    {
        currentInfo = targetObject.GetComponent<InfoScript>();
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (currentInfo != null)
            sideTextA.text = currentInfo.category;
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
