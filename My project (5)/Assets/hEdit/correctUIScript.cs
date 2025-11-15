using TMPro;
using UnityEngine;

public class correctUIScript : MonoBehaviour
{
    [SerializeField] CorrectNumManegerScript correctNumManegerScript;
    TextMeshProUGUI CorrectNumText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CorrectNumText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLifeUI();
    }

    void UpdateLifeUI()
    {
        CorrectNumText.text = correctNumManegerScript.correctNum + " / " + correctNumManegerScript.denominator;
    }

}
