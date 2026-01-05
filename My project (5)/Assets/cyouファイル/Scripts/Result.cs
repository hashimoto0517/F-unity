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
    public Image jyuuketa;
    public Sprite[] number;
    public GameObject jyuugameobject;

    public int numberofmistakes;
    public int lifecount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        numberofmistakes = CorrectNumManegerScript.staticcorrectNumManegerScript.correctNum;
        lifecount = lifeManegerScript.staticlifeManegerScript.currentLife;

        jyuuketa.sprite = number[0];

        if(numberofmistakes<10)
        {
            jyuugameobject.SetActive(false);
            correctresultImage.sprite = number[numberofmistakes];
            liferesultImage.sprite = number[lifecount];
        }


        if (numberofmistakes == 10 && lifecount > 2) 
        {
            jyuuketa.sprite = number[1];
            correctresultImage.sprite = number[0];
            liferesultImage.sprite = number[3];
        }

        if (lifecount >= 2 && numberofmistakes > 9)
        {
            illustration.sprite = perfect;
        }
        else if (lifecount >= 1 && numberofmistakes >= 6)
        {
            illustration.sprite = clear;
        }
        else if (lifecount == 0)
        {
            if (numberofmistakes >= 8)
            {
                illustration.sprite = clear;
            }
            else
            {
                illustration.sprite = lifeDefeat;
            }
        }
        else
        {
            if (numberofmistakes >= 8)
            {
                illustration.sprite = clear;
            }
            else
            {
                illustration.sprite = timeDefeat;
            }
        }


    }

    // Update is called once per frame
    void Update()
    {

    }
}
