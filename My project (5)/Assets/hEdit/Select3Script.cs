using UnityEngine;
using UnityEngine.InputSystem;

public class Select3Script : MonoBehaviour
{
    [SerializeField] Material selectedColor;
    [SerializeField] Material nomalColor;
    [SerializeField] int dis = 5;
    [SerializeField] float angle = 45f;

    GameObject selectedObject = null;
    Collider currentTarget = null;
    InputAction selectAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectAction = new InputAction("Select", InputActionType.Button);
        selectAction.AddBinding("<Gamepad>/buttonNorth"); // Yボタン
        selectAction.AddBinding("<Mouse>/leftButton");    // 左クリック
        selectAction.Enable();

    }

    // Update is called once per frame
    void Update()
    {
        if (selectAction != null && selectAction.WasPressedThisFrame())
        {
            TrySelect();
        }
    }
    private void TrySelect()
    {
        if (currentTarget == null || !currentTarget.CompareTag("Target")) 
            return;

        Vector3 dir = currentTarget.transform.position - transform.position;
        if (Vector3.Angle(transform.forward, dir) > angle) 
            return;

        if (Physics.Raycast(transform.position, dir.normalized, out RaycastHit hit, dis) && hit.collider == currentTarget)
        {
            Select(currentTarget.gameObject);
        }
    }

    void Select(GameObject obj)
    {
        //違うものを選択した
        if (selectedObject != null && selectedObject != obj)
        {
            Deselect(selectedObject);
        }

        //新たに選択したオブジェクトのRendererいれる
        var rend = obj.GetComponent<Renderer>();
        if (rend != null && selectedObject != obj)
        {
            rend.material = selectedColor;
        }
        else if (selectedObject != null && selectedObject == obj)
        {
            Deselect(selectedObject);
        }
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
            rend.material = nomalColor;
            Debug.Log("選択解除: " + obj.name);
        }
        else
        {
            Debug.Log("選択解除対象にRendererが見つかりません: " + obj.name);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Target")) currentTarget = other;
    }

    void OnTriggerExit(Collider other)
    {
        if (other == currentTarget) currentTarget = null;
    }

    void OnDestroy()
    {
        selectAction?.Disable();
        selectAction?.Dispose();
    }
}

