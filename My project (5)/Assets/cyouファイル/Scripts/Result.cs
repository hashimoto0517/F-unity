using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    public Sprite clear;
    public Sprite lifeDefeat;
    public Sprite perfect;
    public Sprite timeDefeat;
    public Image correctresultImage;
    public Image liferesultImage;
    public Image illustration;
    public Sprite[] number;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int numberofmistakes = CorrectNumManegerScript.staticcorrectNumManegerScript.correctNum;
        int lifecount = lifeManegerScript.staticlifeManegerScript.currentLife;
        correctresultImage.sprite = number[numberofmistakes];
        liferesultImage.sprite = number[lifecount];
        if (lifecount >= 3 && numberofmistakes == 10)
        {
            illustration.sprite = perfect;
        }
        else if (lifecount >= 1 && numberofmistakes >= 6)
        {
            illustration.sprite = clear;
        }
        else if (lifecount == 0 || numberofmistakes < 6)
        {
            illustration.sprite = lifeDefeat;
        }
        else if (lifecount >= 1 || numberofmistakes < 6)
        {
            illustration.sprite = timeDefeat;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
