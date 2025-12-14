using UnityEngine;
using UnityEngine.UI;

public class correctUIScript : MonoBehaviour
{
    [SerializeField] CorrectNumManegerScript correctNumManegerScript;

    [SerializeField] private Image correctTensImage;
    [SerializeField] private Image correctUnitsImage;

    [SerializeField] private Image denomTensImage;
    [SerializeField] private Image denomUnitsImage;
    
    [SerializeField] private Sprite[] numberSprites; // 0〜9 のスプライト


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateCorrectUI();
        UpdateDenominatorUI();
    }

    // Update is called once per frame
    public void UpdateCorrectUI()
    {
        int value = correctNumManegerScript.correctNum;

        int tens = value / 10;
        int units = value % 10;

        // 十の位が 0 のときは非表示
        if (tens == 0)
            correctTensImage.enabled = false;
        else
        {
            correctTensImage.enabled = true;
            correctTensImage.sprite = numberSprites[tens];
        }

        correctUnitsImage.sprite = numberSprites[units];
    }

    public void UpdateDenominatorUI()
    {
        int value = correctNumManegerScript.denominator;

        int tens = value / 10;
        int units = value % 10;

        // 十の位が 0 のときは非表示
        if (tens == 0)
            denomTensImage.enabled = false;
        else
        {
            denomTensImage.enabled = true;
            denomTensImage.sprite = numberSprites[tens];
        }

        denomUnitsImage.sprite = numberSprites[units];
    }
}
