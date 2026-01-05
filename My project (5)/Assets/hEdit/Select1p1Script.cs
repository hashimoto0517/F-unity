using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class Select1Script : MonoBehaviour
{
    [SerializeField] int playerNumber; // 1 or 2
    [SerializeField] Image sideA;
    [SerializeField] Image sideB;
    [SerializeField] judgeScript judgeScript;

    private GameObject selectedObject = null;
    private Collider currentTarget = null;
    private PlayerInput playerInput;
    private InputAction selectAction;
    private InputAction deselectAction;
    private InfoScript currentInfo;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip trySelectClip;
    [SerializeField] AudioClip SelectClip;

    [SerializeField] AudioClip correctClip;
    [SerializeField] AudioClip incorrectClip;

    [SerializeField] Camera mainCamera; 
    [SerializeField] Camera subCamera;
    bool isFirstPerson;
    [SerializeField] float rayDistance = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        selectAction = playerInput.actions.FindAction("Select");
        deselectAction = playerInput.actions.FindAction("Deselect");

        sideA.gameObject.SetActive(false);
        sideB.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        isFirstPerson = mainCamera.gameObject.activeInHierarchy;
        if (isFirstPerson)
        {
            SelectFirst();
        }

        if (selectAction != null && selectAction.WasPressedThisFrame())
        {
            TrySelect();
        }

        if (deselectAction != null && deselectAction.WasPressedThisFrame())
        {
            TryDeselect();
        }
    }

    void SelectFirst()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.CompareTag("Target"))
                currentTarget = hit.collider;
            else
                currentTarget = null;
        }
        else
        {
            currentTarget = null;
        }
    }

    private void TrySelect()
    {
        if (currentTarget == null || !currentTarget.CompareTag("Target"))
        {
            audioSource.PlayOneShot(trySelectClip);
            return;
        }
        Select(currentTarget.gameObject);
    }

    private void TryDeselect()
    {
        if (currentTarget == null || !currentTarget.CompareTag("Target"))
            return;
        audioSource.PlayOneShot(trySelectClip);
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
        sideB.gameObject.SetActive(true);

        audioSource.PlayOneShot(SelectClip);
    }

    public void ResetSelect()
    {
        selectedObject = null;
        sideA.gameObject.SetActive(false);
        sideB.gameObject.SetActive(false);
    }

    void Deselect(GameObject obj)
    {
        selectedObject = null;

        judgeScript.DeleteSelection(playerNumber);

        Debug.Log($"{playerNumber}pëIëâèú: {obj.name}");

        sideA.gameObject.SetActive(false);
        sideB.gameObject.SetActive(false);
    }

    public void SetTarget(GameObject targetObject)
    {
        currentInfo = targetObject.GetComponent<InfoScript>();
        UpdateUI();
    }

    public void CorrectSEPlay()
    {
        audioSource.PlayOneShot(correctClip);
    }

    public void incorrectSEPlay()
    {
        audioSource.PlayOneShot(incorrectClip);
    }

    private void UpdateUI()
    {
        if (currentInfo != null)
        {
            sideA.sprite = currentInfo.categoryImage;
            sideA.gameObject.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isFirstPerson && other.CompareTag("Target"))
            currentTarget = other;
    }

    void OnTriggerExit(Collider other)
    {
        if (!isFirstPerson && other == currentTarget)
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
